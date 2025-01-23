using System.Diagnostics.CodeAnalysis;
using Rewrite.Core.Marker;
namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public class JContainer<T>(
    Space before,
    IList<JRightPadded<T>> elements,
    Markers markers
) : JContainer
{
    [NonSerialized] private WeakReference<PaddingHelper>? _padding;

    public PaddingHelper Padding
    {
        get
        {
            PaddingHelper? p;
            if (_padding == null)
            {
                p = new PaddingHelper(this);
                _padding = new WeakReference<PaddingHelper>(p);
            }
            else
            {
                _padding.TryGetTarget(out p);
                if (p == null || p.c != this)
                {
                    p = new PaddingHelper(this);
                    _padding.SetTarget(p);
                }
            }

            return p;
        }
    }

    public Space Before => before;

    public IList<JRightPadded<T>> Elements => elements;

    public Markers Markers => markers;

    public IList<T> GetElements()
    {
        return JRightPadded<T>.GetElements(elements!);
    }

    public static JContainer<J2> WithElements<J2>(JContainer<J2> before, IList<J2>? elements)
        where J2 : J
    {
        if (elements == null)
        {
            return before.Padding.WithElements([]);
        }

        return before.Padding.WithElements(JRightPadded<T>.WithElements(before.Elements, elements));
    }

    public static JContainer<J2>? WithElementsNullable<J2>(JContainer<J2>? before, IList<J2>? elements)
        where J2 : J
    {
        if (elements == null || elements.Count == 0)
        {
            return null;
        }

        if (before == null)
        {
            return Build(Space.EMPTY, JRightPadded<J2>.WithElements([], elements), Markers.EMPTY);
        }

        return before.Padding.WithElements(JRightPadded<T>.WithElements(before.Elements, elements));
    }

    public static JContainer<T1> Build<T1>(Space before, IList<JRightPadded<T1>> elements, Markers markers)
    {
        return new JContainer<T1>(before, elements, markers);
    }

    public static JContainer<T1> Build<T1>(List<JRightPadded<T1>> elements)
    {
        return new JContainer<T1>(Space.EMPTY, elements, Markers.EMPTY);
    }

    public sealed record PaddingHelper(JContainer<T> c)
    {
        public IList<JRightPadded<T>> Elements => c.Elements;

        public JContainer<T> WithElements(IList<JRightPadded<T>> elements)
        {
            return c.Elements == elements ? c : Build(c.Before, elements, c.Markers);
        }
    }

    public JContainer<T> WithBefore(Space newBefore)
    {
        return Before == newBefore ? this : new JContainer<T>(newBefore, Elements, Markers);
    }

    public JContainer<T> WithMarkers(Markers newMarkers)
    {
        return Markers == newMarkers ? this : new JContainer<T>(Before, Elements, newMarkers);
    }

    public static JContainer<T> Empty()
    {
        return new JContainer<T>(Space.EMPTY, [], Markers.EMPTY);
    }
}
public static class JContainerExtensions
{
    public static JContainer<T> WithElements<T>(this JContainer<T> before, IList<T>? elements)
        where T : J
    {
        if (elements == null)
        {
            return before.Padding.WithElements([]);
        }

        return before.Padding.WithElements(JRightPadded<T>.WithElements(before.Elements, elements));
    }

    public static JContainer<T>? WithElementsNullable<T>(JContainer<T>? before, IList<T>? elements)
        where T : J
    {
        if (elements == null || elements.Count == 0)
        {
            return null;
        }

        if (before == null)
        {
            return JContainer.Create(JRightPadded<T>.WithElements([], elements));
        }

        return before.Padding.WithElements(JRightPadded<T>.WithElements(before.Elements, elements));
    }
}
public class JContainer
{

    public static JContainer<T> Create<T>(IList<JRightPadded<T>> elements) => new (Space.EMPTY, elements, Markers.EMPTY);
    public static JContainer<T> Create<T>(IList<JRightPadded<T>> elements, Space space) => new (space, elements, Markers.EMPTY);
    public static JContainer<T> Create<T>(IList<JRightPadded<T>> elements, Space before, Markers markers) => new (before, elements, markers);
    public record Location(Space.Location BeforeLocation, JRightPadded.Location ElementLocation)
    {
        public static readonly Location ANY = new(Space.Location.ANY, JRightPadded.Location.ANY);
        public static readonly Location ANNOTATION_ARGUMENTS = new(Space.Location.ANNOTATION_ARGUMENTS, JRightPadded.Location.ANNOTATION_ARGUMENT);
        public static readonly Location CASE = new(Space.Location.CASE, JRightPadded.Location.CASE);
        public static readonly Location CASE_CASE_LABELS = new(Space.Location.CASE_CASE_LABELS, JRightPadded.Location.CASE_CASE_LABELS);
        public static readonly Location IMPLEMENTS = new(Space.Location.IMPLEMENTS, JRightPadded.Location.IMPLEMENTS);
        public static readonly Location PERMITS = new(Space.Location.PERMITS, JRightPadded.Location.PERMITS);
        public static readonly Location LANGUAGE_EXTENSION = new(Space.Location.LANGUAGE_EXTENSION, JRightPadded.Location.LANGUAGE_EXTENSION);
        public static readonly Location METHOD_DECLARATION_PARAMETERS = new(Space.Location.METHOD_DECLARATION_PARAMETERS, JRightPadded.Location.METHOD_DECLARATION_PARAMETER);
        public static readonly Location METHOD_INVOCATION_ARGUMENTS = new(Space.Location.METHOD_INVOCATION_ARGUMENTS, JRightPadded.Location.METHOD_INVOCATION_ARGUMENT);
        public static readonly Location NEW_ARRAY_INITIALIZER = new(Space.Location.NEW_ARRAY_INITIALIZER, JRightPadded.Location.NEW_ARRAY_INITIALIZER);
        public static readonly Location NEW_CLASS_ARGUMENTS = new(Space.Location.NEW_CLASS_ARGUMENTS, JRightPadded.Location.NEW_CLASS_ARGUMENTS);
        public static readonly Location RECORD_STATE_VECTOR = new(Space.Location.RECORD_STATE_VECTOR, JRightPadded.Location.RECORD_STATE_VECTOR);
        public static readonly Location THROWS = new(Space.Location.THROWS, JRightPadded.Location.THROWS);
        public static readonly Location TRY_RESOURCES = new(Space.Location.TRY_RESOURCES, JRightPadded.Location.TRY_RESOURCE);
        public static readonly Location TYPE_BOUNDS = new(Space.Location.TYPE_BOUNDS, JRightPadded.Location.TYPE_BOUND);
        public static readonly Location TYPE_PARAMETERS = new(Space.Location.TYPE_PARAMETERS, JRightPadded.Location.TYPE_PARAMETER);
        public static readonly Location DECONSTRUCTION_PATTERN_NESTED = new(Space.Location.DECONSTRUCTION_PATTERN_NESTED, JRightPadded.Location.DECONSTRUCTION_PATTERN_NESTED);
    }
}
