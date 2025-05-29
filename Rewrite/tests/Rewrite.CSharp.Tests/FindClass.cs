using System.ComponentModel;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;
using static Rewrite.Core.Tree;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Rewrite.CSharp.Tests;

[DisplayName("Find Class")]
[Description("Search for all the classes in the given source")]
public class FindClass([Option(displayName: "Description", description: "A special sign to specifically highlight the class found by the recipe", example: "~~>")] string? description = null): Recipe
{

    public override ITreeVisitor<Core.Tree, IExecutionContext> GetVisitor()
    {
        return new FindClassVisitor(description);
    }

    private class FindClassVisitor(string? description = null) : CSharpVisitor<IExecutionContext>
    {
        public override Cs VisitClassDeclaration(Cs.ClassDeclaration classDeclaration, IExecutionContext ctx)
        {
            var tree = (MutableTree<Cs.ClassDeclaration>)base.VisitClassDeclaration(classDeclaration, ctx)!;
            return tree.WithMarkers(tree.Markers.AddIfAbsent<SearchResult>(new SearchResult(RandomId(), description)));
        }
    }
}

