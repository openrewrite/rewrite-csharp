namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class MemberReference
    {
        Expression Expression.WithType(JavaType? newType) => WithType(Type);
    }
}
