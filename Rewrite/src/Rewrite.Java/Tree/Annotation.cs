namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Annotation : Expression<Annotation>
    {
        public JavaType? Type => AnnotationType.Type;
        public Annotation WithType(JavaType? type) => WithAnnotationType(AnnotationType.WithType(type));
    }
}
