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
}