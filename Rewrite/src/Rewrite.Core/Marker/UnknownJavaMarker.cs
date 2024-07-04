namespace Rewrite.Core.Marker;

public record UnknownJavaMarker(Guid Id, IDictionary<string, object> Data) : Marker
{
    public bool Equals(Marker? other)
    {
        return other is UnknownJavaMarker && other.Id.Equals(Id);
    }
}