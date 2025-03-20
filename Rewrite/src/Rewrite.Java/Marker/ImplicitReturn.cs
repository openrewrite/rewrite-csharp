namespace Rewrite.RewriteJava.Marker;

public record ImplicitReturn(Guid Id)  : Rewrite.Core.Marker.Marker
{
    public bool Equals(Core.Marker.Marker? other)
    {
        throw new NotImplementedException();
    }
}
