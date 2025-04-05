namespace Rewrite.Core.Marker;

public record SearchResult(Guid Id, string? Description = null) : Marker
{
    public bool Equals(Marker? other)
    {
        return other is SearchResult && other.Id.Equals(Id);
    }

    public static T Found<T>(MutableTree<T> tree, string? Description = null)
    where T : class, MutableTree
    {
        return tree.WithMarkers(tree.Markers.AddIfAbsent(new SearchResult(Tree.RandomId(), Description)));
    }

    public string Print(Cursor cursor, Func<string, string> commentWrapper, bool verbose)
    {
        return commentWrapper(Description == null ? "" : "(" + Description + ")");
    }
}