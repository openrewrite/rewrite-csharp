namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Unknown
    {
        public JavaType? Type => null;

        public Unknown WithType(JavaType? type) => this;
    }
}
