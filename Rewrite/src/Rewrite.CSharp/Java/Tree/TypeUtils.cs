namespace Rewrite.RewriteJava.Tree;

public static class TypeUtils
{
    public static string ToFullyQualifiedName(string fqn)
    {
        return fqn.Replace('$', '.');
    }

    public static bool FullyQualifiedNamesAreEqual(string? fqn1, string? fqn2)
    {
        if (fqn1 != null && fqn2 != null)
        {
            return fqn1.Equals(fqn2) || fqn1.Length == fqn2.Length
                && ToFullyQualifiedName(fqn1).Equals(ToFullyQualifiedName(fqn2));
        }

        return fqn1 == null && fqn2 == null;
    }

    /// <summary>
    /// Converts a JavaType to JavaType.FullyQualified if possible.
    /// </summary>
    /// <param name="type">The JavaType to convert.</param>
    /// <returns>The type as JavaType.FullyQualified if it is a FullyQualified type and not an Unknown type, otherwise null.</returns>
    public static JavaType.FullyQualified? AsFullyQualified(JavaType? type)
    {
        if (type is JavaType.FullyQualified && !(type is JavaType.Unknown))
        {
            return (JavaType.FullyQualified)type;
        }
        return null;
    }
}