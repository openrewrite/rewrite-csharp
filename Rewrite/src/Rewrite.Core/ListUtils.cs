
namespace Rewrite.Core;

public static class ListUtils
{
    /// <summary>
    /// Maps elements in a list using the provided mapping function while preserving list structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list</typeparam>
    /// <param name="list">Input list to map</param>
    /// <param name="map">Mapping function that takes index and element</param>
    /// <returns>New mapped list or original if unchanged</returns>
    public static IList<T> Map<T>(IList<T>? list, Func<int, T, T?> map)
    {
        if (list == null || !list.Any())
        {
            return list!;
        }

        IList<T> newList = list;
        bool nullEncountered = false;

        for (int i = 0; i < list.Count; i++)
        {
            T tree = list[i];
            T? newTree = map(i, tree);

            if (!EqualityComparer<T>.Default.Equals(newTree!, tree))
            {
                if (!ReferenceEquals(newList, list))
                {
                    newList = new List<T>(list);
                }
                newList[i] = newTree!;
            }
            nullEncountered |= newTree == null;
        }

        if (!ReferenceEquals(newList, list) && nullEncountered)
        {
            newList.RemoveAll(x => x == null);
        }

        return newList!;
    }

    /// <summary>
    /// Maps the last element of a list using the provided mapping function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list</typeparam>
    /// <param name="ls">The input list</param>
    /// <param name="mapLast">The mapping function to apply to the last element</param>
    /// <returns>A new list with the last element mapped, or the original list if unchanged</returns>
    public static IList<T>? MapLast<T>(IList<T>? ls, Func<T, T> mapLast)
    {
        if (ls == null || ls.Count == 0)
        {
            return ls;
        }

        T last = ls[ls.Count - 1];
        T newLast = mapLast(last);

        if (!EqualityComparer<T>.Default.Equals(last, newLast))
        {
            var newLs = new List<T>(ls);
            if (newLast == null)
            {
                newLs.RemoveAt(ls.Count - 1);
            }
            else
            {
                newLs[ls.Count - 1] = newLast;
            }
            return newLs;
        }
        return ls;
    }

    /// <summary>
    /// Maps the first element of a list using the provided mapping function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list</typeparam>
    /// <param name="ls">The input list</param>
    /// <param name="mapFirst">The mapping function to apply to the first element</param>
    /// <returns>A new list with the first element mapped, or the original list if unchanged</returns>
    public static IList<T> MapFirst<T>(IList<T>? ls, Func<T?, T?> mapFirst)
    {
        if (ls == null || !ls.Any())
        {
            return ls!;
        }

        T first = ls.First();
        T? newFirst = mapFirst(first);

        if (!ReferenceEquals(first, newFirst))
        {
            var newLs = new List<T>(ls);
            if (newFirst == null)
            {
                newLs.RemoveAt(0);
            }
            else
            {
                newLs[0] = newFirst;
            }
            return newLs;
        }

        return ls;
    }

    /// <summary>
    /// Removes all elements from the list that match the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list from which to remove elements.</param>
    /// <param name="predicate">A delegate that defines the conditions of the elements to remove.</param>
    public static void RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list), "The list cannot be null.");

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate), "The predicate cannot be null.");

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (predicate(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }

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
            if (!EqualityComparer<T>.Default.Equals(t, mappedItem!))
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
            return new List<T>();
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
