using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteJava.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteJava;

public class FindClass : Recipe
{
    public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor()
    {
        return new FindClassVisitor();
    }

    private class FindClassVisitor : JavaVisitor<ExecutionContext>
    {
        public override J VisitClassDeclaration(J.ClassDeclaration classDeclaration, ExecutionContext ctx)
        {
            return SearchResult.Found(
                (MutableTree<J.ClassDeclaration>)base.VisitClassDeclaration(classDeclaration, ctx)!);
        }
    }
}