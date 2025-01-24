namespace Rewrite.RewriteCSharp.Internal;

public class StringUtils
{
    /// <summary>
    /// Determines the common leading whitespace (margin) shared by two strings.
    /// </summary>
    /// <param name="s1">The first string to compare, or <c>null</c>.</param>
    /// <param name="s2">The second string to compare, or <c>null</c>.</param>
    /// <returns>
    /// A string representing the common leading whitespace between <paramref name="s1"/> and <paramref name="s2"/>.
    /// Returns <c>null</c> if both strings are <c>null</c>.
    /// </returns>
    /// <remarks>
    /// - If one string is <c>null</c>, the leading whitespace from the other string (after its last newline) is returned.
    /// - If neither string is <c>null</c>, the function calculates the shared prefix of leading whitespace
    ///   (spaces or tabs) between the two strings.
    /// - If no common margin exists, an empty string is returned.
    /// </remarks>
    /// <example>
    /// <code>
    /// string s1 = "    line one";
    /// string s2 = "    line two";
    /// string? result = CommonMargin(s1, s2);
    /// // Result: "    "
    /// </code>
    /// <code>
    /// string? result = CommonMargin(null, "  indented line");
    /// // Result: "  "
    /// </code>
    /// </example>
    public static string? CommonMargin(string? s1, string? s2)
    {
        if (s1 is null && s2 is null)
            return null;
        if (s1 == null)
        {
            return s2!.Substring(s2.LastIndexOf('\n') + 1);
        }
        if (s2 == null)
        {
            return s1!.Substring(s1.LastIndexOf('\n') + 1);
        }

        for (int i = 0; i < s1.Length && i < s2.Length; i++)
        {
            if (s1[i] != s2[i] || !char.IsWhiteSpace(s1[i]))
            {
                return s1.Substring(0, i);
            }
        }

        return s2.Length < s1.Length ? s2 : s1;
    }
}
