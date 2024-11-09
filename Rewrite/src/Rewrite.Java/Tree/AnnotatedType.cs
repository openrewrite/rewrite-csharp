namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class AnnotatedType
    {
        public JavaType? Type => TypeExpression.Type;

        public AnnotatedType WithType(JavaType? type) => WithTypeExpression(TypeExpression.WithType(type));

    }
}
