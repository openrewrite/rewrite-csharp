namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Empty
    {
        public JavaType? Type => null;
        public Empty WithType(JavaType? type) => this;
    }
}
