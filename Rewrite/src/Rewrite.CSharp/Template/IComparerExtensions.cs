using System.Collections;

namespace Rewrite.RewriteCSharp;

public static class IComparerExtensions
{
    public static IComparer AsUntyped<T>(this IComparer<T> comparer) => new ComparerAdapter<T>(comparer);
    
    
    private class ComparerAdapter<T>(IComparer<T> comparer) : IComparer, IComparer<T>
    {
        public int Compare(object? x, object? y) => Compare((T?)x, (T?)y);

        public int Compare(T? x, T? y) => comparer.Compare(y, x);
    }
}