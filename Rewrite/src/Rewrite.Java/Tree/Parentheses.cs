namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Parentheses<J2>
    {
        public JavaType? Type => Tree switch
        {
            Expression expression => expression.Type,
            TypedTree tree => tree.Type,
            _ => null
        };

        public Parentheses<J2> WithType(JavaType? newType) =>
            Tree switch
            {
                Expression expression => WithTree((J2)expression.WithType(newType)),
                NameTree nameTree => WithTree((J2)nameTree.WithType(newType)),
                _ => this
            };
    }
}
