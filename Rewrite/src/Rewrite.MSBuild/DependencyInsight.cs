using Microsoft.Build.Exceptions;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.MSBuild.Marker;
using Rewrite.RewriteXml;
using Rewrite.RewriteXml.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.MSBuild;

public class DependencyInsight(string? packagePattern) : Recipe
{
    public override ITreeVisitor<Tree, ExecutionContext> GetVisitor()
    {
        return new ProjectDependenciesVisitor(packagePattern);
    }

    private class ProjectDependenciesVisitor(string? packagePattern) : XmlVisitor<ExecutionContext>
    {
        private const int Utf8Bom = '\uFEFF';

        public override Xml VisitDocument(Xml.Document document, ExecutionContext ctx)
        {
            if (document.Markers.FindFirst<ProjectDependencies>() != null)
            {
                return document;
            }

            var outputCapture = new PrintOutputCapture<int>(0);
            new XmlPrinter<int>().Visit(document, outputCapture);
            var printed = outputCapture.GetOut();
            // strip UTF-8 BOM if present
            printed = printed.Length > 0 && printed[0] == Utf8Bom ? printed[1..] : printed;
            try
            {
                var dependencies = ProjectDependencyResolver.TransitiveProjectDependencies(printed, packagePattern);
                var dependenciesArray = dependencies as Dependency[] ?? dependencies.ToArray();
                if (dependenciesArray.Length != 0)
                {
                    var marker = new ProjectDependencies(Tree.RandomId(), document.SourcePath,
                        new List<Dependency>(dependenciesArray));
                    return document.WithMarkers(document.Markers.AddIfAbsent(marker));
                }

                return document;
            }
            catch (Exception e)
            {
                // avoid compile-time dependency on `Microsoft.Build`
                if (e.GetType().FullName == "Microsoft.Build.Exceptions.InvalidProjectFileException" ||
                    e is ArgumentException &&
                    e.Message == "Value cannot be null or an empty string. (Parameter 'value')" ||
                    e.Message.EndsWith(" unexpectedly not a rooted path"))
                {
                    return Markup.WarnMarkup(document, e);
                }

                return Markup.ErrorMarkup(document, e);
            }
        }
    }
}