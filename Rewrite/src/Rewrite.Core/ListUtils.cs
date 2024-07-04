using System.Collections.Immutable;

namespace Rewrite.Core;

public static class ListUtils
{
    public static IList<T> Map<T>(IList<T>? list, Func<T, T?> map)
    {
        if (list == null || list.Count == 0)
        {
            return list;
        }

        var mappedList = new List<T>();
        var areAllElementsSame = true;

        foreach (var t in list)
        {
            var mappedItem = map(t);
            if (mappedItem != null)
            {
                mappedList.Add(mappedItem);
            }

            // Compare the current input and mapped items for equality.
            if (!EqualityComparer<T>.Default.Equals(t, mappedItem))
            {
                areAllElementsSame = false;
            }
        }

        return areAllElementsSame ? list : mappedList;
    }

    public static IList<T> Concat<T>(IList<T>? ls, T? t)
    {
        if (t == null && ls == null)
        {
            return ImmutableList<T>.Empty;
        }
        else if (t == null)
        {
            return ls!;
        }

        var newLs = ls == null ? [] : new List<T>(ls);
        newLs.Add(t);
        return newLs;
    }
}