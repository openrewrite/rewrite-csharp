
using JetBrains.Annotations;

public static class Extensions
{
    public static string Append(this string original, [CanBeNull] string toAppend)
    {
        return $"{original}{toAppend}";
    }
}
