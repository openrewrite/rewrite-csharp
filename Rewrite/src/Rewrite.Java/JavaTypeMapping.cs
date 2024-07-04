using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava;

public interface JavaTypeMapping<in T>
{
    JavaType Type(T? t);
}