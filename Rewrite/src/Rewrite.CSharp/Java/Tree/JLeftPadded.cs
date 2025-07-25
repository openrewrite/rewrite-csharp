using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public sealed partial class JLeftPadded<T>(
    Space before,
    T element,
    Markers markers
) : JLeftPadded,IHasPrefix, IHasMarkers
{
    public Space Before => before;

    IHasPrefix IHasPrefix.WithPrefix(Space prefix) => WithBefore(prefix);
    public JLeftPadded<T> WithBefore(Space newBefore)
    {
        return ReferenceEquals(newBefore, before) ? this : new JLeftPadded<T>(newBefore, element, markers);
    }

    public T Element => element;

    public JLeftPadded<T> WithElement(T newElement)
    {
        return ReferenceEquals(newElement, element) ? this : new JLeftPadded<T>(before, newElement, markers);
    }

    public static JLeftPadded<T>? WithElement(JLeftPadded<T>? before, T? element)
    {
        if (element == null) {
            return null;
        }
        return before == null ? new JLeftPadded<T>(Space.EMPTY, element, Markers.EMPTY) : before.WithElement(element);
    }

    public override Markers Markers => markers;

    public override JLeftPadded<T> WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, markers) ? this : new JLeftPadded<T>(before, element, newMarkers);
    }

    Space IHasPrefix.Prefix => Before;
}

public static class JLeftPaddedExtensions
{
    [PublicAPI]
    public static JLeftPadded<T> AsLeftPadded<T>(this T element) => JLeftPadded.Create(element, Space.EMPTY, Markers.EMPTY);
    public static JLeftPadded<T> AsLeftPadded<T>(this T element, Space before) => element.AsLeftPadded(before, Markers.EMPTY);

    [PublicAPI]
    public static JLeftPadded<T> AsLeftPadded<T>(this T element, Space before, Markers markers)
    {
        return new JLeftPadded<T>(before, element, markers);
    }
}
[PublicAPI]
public abstract partial class JLeftPadded : IHasMarkers
{
    [PublicAPI]
    public static JLeftPadded<T> Create<T>(T element, Space before) => Create(element, before, Markers.EMPTY);
    public static JLeftPadded<T> Create<T>(T element, Space before, Markers markers)
    {
        return new JLeftPadded<T>(before, element, markers);
    }

    public abstract JLeftPadded WithMarkers(Markers newMarkers);
    public partial record Location(Space.Location BeforeLocation)
    {
        // public static readonly Location ASSERT_DETAIL = new(Space.Location.ASSERT_DETAIL_PREFIX);
        // public static readonly Location ASSIGNMENT = new(Space.Location.ASSIGNMENT);
        // // public static readonly Location ASSIGNMENT_OPERATION_OPERATOR = new(Space.Location.ASSIGNMENT_OPERATION_OPERATOR);
        // // public static readonly Location BINARY_OPERATOR = new(Space.Location.BINARY_OPERATOR);
        // public static readonly Location CLASS_KIND = new(Space.Location.CLASS_KIND);
        // public static readonly Location EXTENDS = new(Space.Location.EXTENDS);
        // public static readonly Location FIELD_ACCESS_NAME = new(Space.Location.FIELD_ACCESS_NAME);
        // public static readonly Location IMPORT_ALIAS_PREFIX = new(Space.Location.IMPORT_ALIAS_PREFIX);
        // public static readonly Location LANGUAGE_EXTENSION = new(Space.Location.LANGUAGE_EXTENSION);
        // public static readonly Location MEMBER_REFERENCE_NAME = new(Space.Location.MEMBER_REFERENCE_NAME);
        // public static readonly Location METHOD_DECLARATION_DEFAULT_VALUE = new(Space.Location.METHOD_DECLARATION_DEFAULT_VALUE);
        // public static readonly Location STATIC_IMPORT = new(Space.Location.STATIC_IMPORT);
        // public static readonly Location TERNARY_TRUE = new(Space.Location.TERNARY_TRUE);
        // public static readonly Location TERNARY_FALSE = new(Space.Location.TERNARY_FALSE);
        // public static readonly Location TRY_FINALLY = new(Space.Location.TRY_FINALLY);
        // // public static readonly Location UNARY_OPERATOR = new(Space.Location.UNARY_OPERATOR);
        // public static readonly Location VARIABLE_INITIALIZER = new(Space.Location.VARIABLE_INITIALIZER);
        // public static readonly Location WHILE_CONDITION = new(Space.Location.WHILE_CONDITION);
        // public static readonly Location WILDCARD_BOUND = new(Space.Location.WILDCARD_BOUND);
    }

    public abstract Markers Markers { get; }
}
