using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;

public static class Extensions
{

    public static IList<T> Elements<T>(this IList<JRightPadded<T>> ls)
        where T : class
    {
        return ls.Select(e => e.Element).ToList();
    }

    public static IList<JRightPadded<T>> WithElements<T>(this IList<JRightPadded<T>> before, IList<T> elements)
        where T : J
    {
        // a cheaper check for the most common case when there are no changes
        if (elements.Count == before.Count)
        {
            var hasChanges = false;
            for (var i = 0; i < before.Count; i++)
            {
                if (!ReferenceEquals(before[i].Element, elements[i]))
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

        var after = new List<JRightPadded<T>>(elements.Count);
        var beforeById = before.ToDictionary(j => j.Element.Id, e => e);

        foreach (var t in elements)
        {
            if (beforeById.TryGetValue(t.Id, out var found))
            {
                after.Add(found.WithElement(t));
            }
            else
            {
                after.Add(new JRightPadded<T>(t, Space.EMPTY, Markers.EMPTY));
            }
        }

        return after;
    }

}
