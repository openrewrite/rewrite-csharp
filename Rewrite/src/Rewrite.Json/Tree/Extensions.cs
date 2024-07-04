using System.Collections;
using Rewrite.Core.Marker;

namespace Rewrite.RewriteJson.Tree;

public static class Extensions
{
    public static IList<T> Elements<T>(this IList<JsonRightPadded<T>> ls)
        where T : class, Json
    {
        return ls.Select(e => e.Element).ToList();
    }

    public static IList<JsonRightPadded<T>> WithElements<T>(this IList<JsonRightPadded<T>> before, IList<T> elements)
        where T : class, Json
    {
        // a cheaper check for the most common case when there are no changes
        if (elements.Count == before.Count)
        {
            var hasChanges = false;
            for (int i = 0; i < before.Count; i++)
            {
                if (before[i].Element != elements[i])
                {
                    hasChanges = true;
                    break;
                }
            }

            if (!hasChanges)
            {
                return before;
            }
        }

        var after = new List<JsonRightPadded<T>>(elements.Count);
        var beforeById = before.ToDictionary(j => j.Element.Id, e => e);

        foreach (var t in elements)
        {
            if (beforeById.TryGetValue(t.Id, out var found))
            {
                after.Add(found.WithElement(t));
            }
            else
            {
                after.Add(new JsonRightPadded<T>(t, Space.EMPTY, Markers.EMPTY));
            }
        }

        return after;
    }
}