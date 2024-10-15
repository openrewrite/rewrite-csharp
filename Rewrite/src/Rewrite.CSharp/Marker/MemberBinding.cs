namespace Rewrite.RewriteCSharp.Marker;

public record MemberBinding(Guid Id) : Core.Marker.Marker
{
    public Guid Id { get; init; } = Id;

    public MemberBinding() : this(Core.Tree.RandomId())
    {
    }

    public virtual bool Equals(Core.Marker.Marker? other)
    {
        return other is MemberBinding && other.Id == Id;
    }
}
