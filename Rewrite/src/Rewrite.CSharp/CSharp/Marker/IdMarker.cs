namespace Rewrite.RewriteCSharp.Marker;

public class IdMarker : Rewrite.Core.Marker.Marker
{
    public IdMarker(Guid id)
    {
        Id = id;
    }

    public bool Equals(Core.Marker.Marker? other)
    {
        return other is IdMarker idMarker && idMarker.Id == Id;
    }

    public Guid Id { get; }

    public string Print(Cursor cursor, Func<string, string> commentWrapper, bool verbose)
    {
        return commentWrapper(Id.ToString());
    }
}