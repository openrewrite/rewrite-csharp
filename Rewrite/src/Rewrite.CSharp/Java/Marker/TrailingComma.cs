using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava.Marker;

public record TrailingComma(Guid Id, Space Suffix) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is TrailingComma && other.Id.Equals(Id);
    }
}