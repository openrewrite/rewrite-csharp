using System.ComponentModel;
using System.Text;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Recipes;

[DisplayName("Invert Assertion")]
[Description("Find for all the `Assert.True(!someBool)` and transform it into `Assert.False(someBool)`.")]
public class InvertAssertion : Recipe
{
    private const string ASSERT_TRUE = "Xunit.Assert.True";
    
    public override ITreeVisitor<Tree, IExecutionContext> GetVisitor()
    {
        return new InvertAssertionVisitor();
    }

    private class InvertAssertionVisitor : JavaVisitor<IExecutionContext>
    {
        public override J.MethodInvocation VisitMethodInvocation(J.MethodInvocation method, IExecutionContext ctx)
        {
            // Assert.True(!a);
            var mi = (base.VisitMethodInvocation(method, ctx) as J.MethodInvocation)!;

            if (!ASSERT_TRUE.EndsWith(ExtractName(mi)) || !IsUnaryOperatorNot(mi)) return mi;

            var arguments = (Cs.Argument)mi.Arguments[0];
            var unary = (J.Unary)arguments.Expression;

            return mi.WithArguments([unary.Expression]).WithName(mi.Name.WithSimpleName("False"));
        }

        private static string ExtractName(J.MethodInvocation mi)
        {
            return (mi.Select is J.Identifier i ? (i.SimpleName + ".") : "") + mi.Name.SimpleName;
        }

        private static bool IsUnaryOperatorNot(J.MethodInvocation method)
        {
            return method.Arguments is [ Cs.Argument { Expression : J.Unary { Operator: J.Unary.Types.Not }}];
        }
    };
}
