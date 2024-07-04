using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface MutableTree<out T> : Tree where T : class
{
    T WithMarkers(Markers markers);
}