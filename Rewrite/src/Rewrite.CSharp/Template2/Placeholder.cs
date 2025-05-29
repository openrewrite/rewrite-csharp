using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Template2;

public class Placeholder(J? node, string? type)
{
    private readonly J? _node = node;
    public string? Type {get;} = type;

    public J? Node => _node;

    public static Placeholder InsertionPoint = new Placeholder(null, null);
    public static Placeholder Any() => new Placeholder(null, null);
    public static Placeholder Any<T>() => Any(typeof(T).AssemblyQualifiedName!);
    public static Placeholder Any(string type) => new Placeholder(null, type);
}

public static class PlaceholderExtensions
{
    public static Placeholder Any(this J? node)
    {
        return new Placeholder(node, null);
    }
    
}