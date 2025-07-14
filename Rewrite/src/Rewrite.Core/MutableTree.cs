using Rewrite.Core.Marker;

namespace Rewrite.Core;

public partial interface MutableTree : Tree
{
}
public partial interface MutableTree<out T> : MutableTree where T : class
{
    // new T WithMarkers(Markers markers);
    // MutableTree MutableTree.WithMarkers(Markers markers) => (MutableTree)WithMarkers(markers);
}