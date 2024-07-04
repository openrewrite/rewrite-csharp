namespace Rewrite.RewriteJava.Marker;

public record OmitParentheses(Guid Id) : Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        return other is OmitParentheses && other.Id.Equals(Id);
    }
}