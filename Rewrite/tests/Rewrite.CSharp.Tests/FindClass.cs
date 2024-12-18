using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;
using static Rewrite.Core.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.CSharp.Tests;

public class FindClass([Option(displayName: "Description", description: "A special sign to specifically highlight the class found by the recipe", example: "~~>")] string? description = null): Recipe
{
    public override string DisplayName => "Find Class";

    public override string Description => "Search for all the classes in the given source";

    public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor()
    {
        return new FindClassVisitor(description);
    }

    private class FindClassVisitor(string? description = null) : CSharpVisitor<ExecutionContext>
    {
        public override Cs VisitClassDeclaration(Cs.ClassDeclaration classDeclaration, ExecutionContext ctx)
        {
            var tree = (MutableTree<Cs.ClassDeclaration>)base.VisitClassDeclaration(classDeclaration, ctx)!;
            return tree.WithMarkers(tree.Markers.AddIfAbsent<SearchResult>(new SearchResult(RandomId(), description)));
        }
    }
}

