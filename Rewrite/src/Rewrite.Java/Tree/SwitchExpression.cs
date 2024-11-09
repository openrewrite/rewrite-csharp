using System.Runtime.CompilerServices;

namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class SwitchExpression
    {
        public JavaType? Type
        {
            get
            {
                StrongBox<JavaType> type = new();
                new SwitchExpressionJavaVisitor().Visit(this, type);
                return type.Value;
            }
        }

        public SwitchExpression WithType(JavaType? type) => this;

        class SwitchExpressionJavaVisitor : JavaVisitor<StrongBox<JavaType>>
        {
            public override J VisitBlock(J.Block block, StrongBox<JavaType> javaType)
            {
                if (block.Statements.Count != 0)
                {
                    var caze = (J.Case)block.Statements[0];
                    javaType.Value = caze.Expressions[0].Type!;
                }

                return block;
            }
        }
    }
}
