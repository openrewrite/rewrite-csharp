using Microsoft.CodeAnalysis;

namespace Rewrite.Analyzers;

public static class Extensions
{
    public static bool InheritsFrom(this INamedTypeSymbol symbol, string type)
    {
        var current = symbol.BaseType;
        while (current != null)
        {
            if (current.Name == type)
                return true;
            current = current.BaseType;
        }
        return false;
    }
    public static string Ident(this object source, int identLevels)
    {
        var lines = source.ToString().TrimStart(' ').Split('\n');
        var ident = new string(' ', identLevels * 4);
        return string.Join("\n", lines.Select((x, i) => $"""{ (i > 0 ? ident : "") }{x}"""));
    }

    public static string Render<T>(this IEnumerable<T> source, Func<T, string> template, string separator = "", string openToken = "", string closeToken = "", bool renderEmpty = true)
    {
        if (!renderEmpty && source.Count() == 0)
            return "";
        return $"{openToken}{string.Join(separator, source.Select(template))}{closeToken}";
    }

    public static string Render<T>(this IEnumerable<T> source, Func<T, int, string> template, string separator = "")
    {
        return string.Join(separator, source.Select(template));
    }
}
