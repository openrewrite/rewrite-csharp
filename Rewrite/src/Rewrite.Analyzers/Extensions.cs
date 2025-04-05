using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers;

public static class Extensions
{
    private static HashSet<string> _reservedWords =
    [
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
        "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
        "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
        "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
    ];
    public static string EnsureSafeIdentifier(this string s) => $"{s[0].ToString().ToLower()}{s.Remove(0, 1)}";
    public static string ToCamelCase(this string s) => $"{s[0].ToString().ToLower()}{s.Remove(0, 1)}";

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
        var lines = source.ToString()?.TrimStart(' ').Split('\n') ?? [];
        var ident = new string(' ', identLevels * 4);
        return string.Join("\n", lines.Select((x, i) => $"""{ (i > 0 ? ident : "") }{x}"""));
    }

    public static bool HasAttribute(this SyntaxList<AttributeListSyntax> attributes, string name)
    {
        string fullname, shortname;
        var attrLen = "Attribute".Length;
        if (name.EndsWith("Attribute"))
        {
            fullname = name;
            shortname = name.Remove(name.Length - attrLen, attrLen);
        }
        else
        {
            fullname = name + "Attribute";
            shortname = name;
        }

        return attributes.Any(al => al.Attributes.Any(a => a.Name.ToString() == shortname || a.Name.ToString() == fullname));
    }

    public static T? FindParent<T>(this SyntaxNode node) where T : class
    {
        var current = node;
        while(true)
        {
            current = current.Parent;
            if (current == null || current is T)
                return current as T;
        }
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
