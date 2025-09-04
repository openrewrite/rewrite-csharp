namespace Rewrite.Analyzers.Extensions;

internal static class EnumerableExtensions
{
	public static IEnumerable<TProjection> SelectWhereNotNull<T, TProjection>(this IEnumerable<T> source, Func<T, TProjection?> projection)
	{
		foreach (var element in source)
		{
			var projectedElement = projection(element);
			if (projectedElement is not null)
			{
				yield return projectedElement;
			}
		}
	} 
    
    //
    // public static string Render<T>(this IEnumerable<T> source, Func<T, string> template, string separator = "", string openToken = "", string closeToken = "", bool renderEmpty = true)
    // {
    //     if (!renderEmpty && !source.Any())
    //         return "";
    //     return $"{openToken}{string.Join(separator, source.Select(template))}{closeToken}";
    // }
    //
    // public static string Render<T>(this IEnumerable<T> source, Func<T, int, string> template, string separator = "")
    // {
    //     return string.Join(separator, source.Select(template));
    // }
}