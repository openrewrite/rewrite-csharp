using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava;

internal class JavaTypeCache
{
    private readonly IDictionary<string, JavaType> _cache = new Dictionary<string, JavaType>();

    public JavaType? this[string typeName]
    {
        get => _cache.TryGetValue(typeName, out var type) ? type : null;
        set => _cache[typeName] = value;
    }
}