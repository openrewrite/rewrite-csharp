namespace Rewrite.Remote;

internal record TypeComparer : IComparer<Type>
{
    public int Compare(Type? x, Type? y)
    {
        return x == y ? 0 : x!.IsAssignableFrom(y) ? 1 : -1;
    }
}
