namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class ControlParentheses<J2>
    {
        public JavaType? Type => Tree switch
        {
            Expression expression => expression.Type,
            NameTree nameTree => nameTree.Type,
            _ => null
        };

        public ControlParentheses<J2> WithType(JavaType? newType) => Tree switch
        {
            Expression expression => WithTree((J2)expression.WithType(newType)),
            NameTree nameTree => WithTree((J2)nameTree.WithType(newType)),
            _ => this
        };

    }
}
