using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public class JsonRightPadded<T>(
    T element,
    Space after,
    Markers markers
) where T : Json
{
    public T Element => element;

    public JsonRightPadded<T> WithElement(T newElement)
    {
        return ReferenceEquals(newElement, element) ? this : new JsonRightPadded<T>(newElement, after, markers);
    }

    public Space After => after;

    public JsonRightPadded<T> WithAfter(Space newAfter)
    {
        return ReferenceEquals(newAfter, after) ? this : new JsonRightPadded<T>(element, newAfter, markers);
    }

    public Markers Markers => markers;

    public JsonRightPadded<T> WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, markers) ? this : new JsonRightPadded<T>(element, after, newMarkers);
    }

}