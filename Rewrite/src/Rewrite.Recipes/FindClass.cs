using System.ComponentModel;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Recipes;

[DisplayName("Find Class")]
[Description("Search for all the classes in the given source")]
public class FindClass: Recipe
{
    [DisplayName("Description")]
    [Description("A special sign to specifically highlight the class found by the recipe")]
    [Example("~~>")]
    public string? Description { get; set; }
    public override ITreeVisitor<Tree, IExecutionContext> GetVisitor()
    {
        return new FindClassVisitor(Description);
    }

    private class FindClassVisitor(string? description = null) : CSharpVisitor<IExecutionContext>
    {
        public override J? VisitClassDeclaration(J.ClassDeclaration classDeclaration, IExecutionContext ctx)
        {
            return ApplyMarker(base.VisitClassDeclaration(classDeclaration, ctx));

        }

        public override J? VisitClassDeclaration(Cs.ClassDeclaration classDeclaration, IExecutionContext p)
        {
            return ApplyMarker(base.VisitClassDeclaration(classDeclaration, p));
        }

        private J? ApplyMarker(J? tree)
        {
            return tree?.WithMarkers(tree.Markers.AddIfAbsent(new SearchResult(Tree.RandomId(), description)));
        }
    }
}
