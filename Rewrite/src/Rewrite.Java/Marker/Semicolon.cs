namespace Rewrite.RewriteJava.Marker;

public record Semicolon(Guid Id) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is Semicolon && other.Id.Equals(Id);
    }
}