namespace Rewrite.RewriteCSharp.Marker;

public record MemberBinding(Guid Id) : Core.Marker.Marker
{
    public virtual bool Equals(Core.Marker.Marker? other)
    {
        return other is MemberBinding && other.Id == Id;
    }

}
