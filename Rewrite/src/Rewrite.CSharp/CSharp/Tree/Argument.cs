using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public partial interface Cs : J
{

    public partial class Argument
    {
        public JavaType? Type => Expression.Type;
        public Argument WithType(JavaType? type) => WithExpression(Expression.WithType(type));
    }
}
