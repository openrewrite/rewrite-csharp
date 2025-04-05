using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface MutableTree : Tree
{
    public MutableTree WithMarkers(Markers markers);
}
public interface MutableTree<out T> : MutableTree where T : class
{
    new T WithMarkers(Markers markers);
    MutableTree MutableTree.WithMarkers(Markers markers) => (MutableTree)WithMarkers(markers);
}