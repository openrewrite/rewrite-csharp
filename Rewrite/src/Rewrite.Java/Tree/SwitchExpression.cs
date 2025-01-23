using System.Runtime.CompilerServices;

namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class SwitchExpression
    {

        class SwitchExpressionJavaVisitor : JavaVisitor<StrongBox<JavaType>>
        {
            public override J VisitBlock(J.Block block, StrongBox<JavaType> javaType)
            {
                if (block.Statements.Count != 0)
                {
                    var caze = (J.Case)block.Statements[0];
                    javaType.Value = (caze.CaseLabels[0] as Expression)?.Type!;
                }

                return block;
            }
        }
    }
}
