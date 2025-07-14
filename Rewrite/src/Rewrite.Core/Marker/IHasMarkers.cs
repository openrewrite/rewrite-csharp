namespace Rewrite.Core.Marker;

public partial interface IHasMarkers
{
    public Markers Markers { get; }
    public IHasMarkers WithMarkers(Markers markers);
}
