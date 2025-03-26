namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Wildcard
    {
        public JavaType? Type => null;

        public Wildcard WithType(JavaType? type) => this;
    }
}
