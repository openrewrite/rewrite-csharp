namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class NullableType
    {
        public JavaType? Type => TypeTree.Type;

        public NullableType WithType(JavaType? type) => WithTypeTree(TypeTree.WithType(type));
    }
}
