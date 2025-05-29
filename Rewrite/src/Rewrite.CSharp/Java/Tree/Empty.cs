namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Empty
    {
        public static Empty Create() => new Empty(Core.Tree.RandomId(), Space.EMPTY,  Markers.EMPTY);
        public JavaType? Type => null;
        public Empty WithType(JavaType? type) => this;
    }
}
