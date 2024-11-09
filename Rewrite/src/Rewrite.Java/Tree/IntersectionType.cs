namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class IntersectionType
    {
        public JavaType? Type => new JavaType.Intersection(
            Bounds
            .Select(b => b.Type)
            .Cast<JavaType>()
            .ToList());
        public IntersectionType WithType(JavaType? type) => this;
    }
}
