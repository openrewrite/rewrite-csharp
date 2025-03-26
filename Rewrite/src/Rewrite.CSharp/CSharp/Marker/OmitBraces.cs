namespace Rewrite.RewriteCSharp.Marker;

public record OmitBraces(Guid Id) : Core.Marker.Marker
{
    public OmitBraces() : this(Core.Tree.RandomId()) { }
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is OmitBraces && other.Id.Equals(Id);
    }
}
