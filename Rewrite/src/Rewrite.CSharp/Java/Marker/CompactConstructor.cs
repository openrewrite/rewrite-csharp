namespace Rewrite.RewriteJava.Marker;

public record CompactConstructor(Guid Id) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is CompactConstructor && other.Id.Equals(Id);
    }
}