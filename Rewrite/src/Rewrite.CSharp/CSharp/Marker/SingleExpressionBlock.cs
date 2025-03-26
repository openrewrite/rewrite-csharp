namespace Rewrite.RewriteCSharp.Marker;

public record SingleExpressionBlock(Guid Id) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is SingleExpressionBlock && other.Id.Equals(Id);
    }
}