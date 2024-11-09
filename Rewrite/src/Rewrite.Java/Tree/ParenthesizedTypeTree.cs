namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class ParenthesizedTypeTree
    {
        public JavaType? Type => ParenthesizedType.Type;
        public ParenthesizedTypeTree WithType(JavaType? type) => WithParenthesizedType(ParenthesizedType.WithType(type));
    }
}
