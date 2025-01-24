using System.Collections.Immutable;

namespace Rewrite.Core;

public static class ListUtils
{
    // todo: AS: this method is used in contexts where tree is changed in immutable way.
    // this immutability is broken by how this method tries to optimize as it sometimes reuses same list when a copy should have been made. since this collection is writable,
    // it's possible that changes mutation in one set affects it in other place, which is not the intent of immutable copy
    public static IList<T> Map<T>(this IList<T> list, Func<T, T?> map)
    {
        if (list.Count == 0)
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

    /// <summary>
    /// Concatenates two lists together, handling null and empty cases.
    /// </summary>
    /// <typeparam name="T">The type of elements in the lists</typeparam>
    /// <param name="ls">First list to concatenate</param>
    /// <param name="t">Second list to concatenate</param>
    /// <returns>A new concatenated list, or null if both inputs are null</returns>
    public static IList<T>? ConcatAll<T>(IList<T>? ls, IList<T>? t)
    {
        if (ls == null && t == null)
        {
            return null;
        }
        else if (t == null || !t.Any())
        {
            return ls;
        }
        else if (ls == null || !ls.Any())
        {
            return t;
        }

        var newLs = new List<T>(ls);
        newLs.AddRange(t);
        return newLs;
    }
}
