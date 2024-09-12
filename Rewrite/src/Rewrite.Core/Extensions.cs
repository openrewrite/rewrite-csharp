namespace Rewrite.Core;

public static class Extensions
{
    [Pure]
    internal static bool SafeSequenceEqual<TSource>(this IEnumerable<TSource>? first, IEnumerable<TSource>? second)
    {
        if (ReferenceEquals(first, second)) return true; // they are either pointing to same collection or both are null
        if (first == null || second == null) return false;
        return first.SequenceEqual(second);
    }
}
