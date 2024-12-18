using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;

public partial interface J<T> : J  where T : J
{
    public new T WithPrefix(Space prefix);
    J J.WithPrefix(Space prefix) => WithPrefix(prefix);
    public new T WithMarkers(Markers markers);
    J J.WithMarkers(Markers markers) => WithMarkers(markers);
}
public partial interface J
{
    public J WithPrefix(Space prefix);
    public J WithMarkers(Markers markers);
}
