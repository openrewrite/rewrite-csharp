namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Erroneous
    {
        public JavaType? Type => JavaType.Unknown.Instance;
        public Erroneous WithType(JavaType? type) => this;
    }
}
