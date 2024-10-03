using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
#if DEBUG_VISITOR
[DebuggerStepThrough]
#endif
public sealed class JRightPadded<T>(
    T element,
    Space after,
    Markers markers
)
{
    public T Element => element;

    public Space After => after;

    public JRightPadded<T> WithElement(T newElement)
    {
        return ReferenceEquals(newElement, element) ? this : new JRightPadded<T>(newElement, after, markers);
    }

    public static IList<T> GetElements(IList<JRightPadded<T>?>? ls)
    {
        if (ls == null)
        {
            return [];
        }

        var list = new List<T>(ls.Count);
        foreach (var l in ls)
        {
            if (l == null)
            {
                continue;
            }

            var elem = l.Element;
            list.Add(elem);
        }

        return list;
    }


    public static JRightPadded<T>? WithElement(JRightPadded<T>? before, T? element)
    {
        if (element == null)
        {
            return null;
        }

        return before == null ? new JRightPadded<T>(element, Space.EMPTY, Markers.EMPTY) : before.WithElement(element);
    }

    public static IList<JRightPadded<J2>> WithElements<J2>(IList<JRightPadded<J2>> before, IList<J2> elements)
        where J2 : J
    {
        // a cheaper check for the most common case when there are no changes
        if (elements.Count == before.Count)
        {
            var hasChanges = false;
            for (var i = 0; i < before.Count; i++)
            {
                if (!ReferenceEquals(before[i].Element, elements[i]))
                {
                    hasChanges = true;
                    break;
                }
            }

            if (!hasChanges)
            {
                return before;
            }
        }
        else if (elements.Count == 0)
        {
            return [];
        }

        IList<JRightPadded<J2>> after = new List<JRightPadded<J2>>(elements.Count);
        Dictionary<Guid, JRightPadded<J2>> beforeById =
            new Dictionary<Guid, JRightPadded<J2>>((int)Math.Ceiling(elements.Count / 0.75));
        foreach (var j in before)
        {
            if (!beforeById.TryAdd(j.Element.Id, j))
            {
                throw new ArgumentException("Duplicate key");
            }
        }

        foreach (var t in elements)
        {
            after.Add(beforeById.TryGetValue(t.Id, out var found)
                ? found.WithElement(t)
                : new JRightPadded<J2>(t, Space.EMPTY, Markers.EMPTY));
        }

        return after;
    }

    public JRightPadded<T> WithAfter(Space newAfter)
    {
        return newAfter == after ? this : new JRightPadded<T>(element, newAfter, markers);
    }

    public Markers Markers => markers;

    public JRightPadded<T> WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, markers) ? this : new JRightPadded<T>(element, after, newMarkers);
    }

    public static JRightPadded<T> Build(T t)
    {
        return new JRightPadded<T>(t, Space.EMPTY, Markers.EMPTY);
    }

    public JRightPadded<TNew> Cast<TNew>() where TNew : T
    {
        return new JRightPadded<TNew>((TNew)Element!, after, markers);
    }
}

public static class JRightPadded
{
    public record Location(Space.Location AfterLocation)
    {
        public static readonly Location ANY = new(Space.Location.ANY);
        public static readonly Location ANNOTATION_ARGUMENT = new(Space.Location.ANNOTATION_ARGUMENT_SUFFIX);
        public static readonly Location ARRAY_INDEX = new(Space.Location.ARRAY_INDEX_SUFFIX);
        public static readonly Location BLOCK_STATEMENT = new(Space.Location.BLOCK_STATEMENT_SUFFIX);
        public static readonly Location CASE = new(Space.Location.CASE_SUFFIX);
        public static readonly Location CASE_EXPRESSION = new(Space.Location.CASE_EXPRESSION);
        public static readonly Location CASE_BODY = new(Space.Location.CASE_BODY);
        public static readonly Location CATCH_ALTERNATIVE = new(Space.Location.CATCH_ALTERNATIVE_SUFFIX);
        public static readonly Location DIMENSION = new(Space.Location.DIMENSION_SUFFIX);
        public static readonly Location ENUM_VALUE = new(Space.Location.ENUM_VALUE_SUFFIX);
        public static readonly Location FOR_BODY = new(Space.Location.FOR_BODY_SUFFIX);
        public static readonly Location FOR_CONDITION = new(Space.Location.FOR_CONDITION_SUFFIX);
        public static readonly Location FOR_INIT = new(Space.Location.FOR_INIT_SUFFIX);
        public static readonly Location FOR_UPDATE = new(Space.Location.FOR_UPDATE_SUFFIX);
        public static readonly Location FOREACH_VARIABLE = new(Space.Location.FOREACH_VARIABLE_SUFFIX);
        public static readonly Location FOREACH_ITERABLE = new(Space.Location.FOREACH_ITERABLE_SUFFIX);
        public static readonly Location IF_ELSE = new(Space.Location.IF_ELSE_SUFFIX);
        public static readonly Location IF_THEN = new(Space.Location.IF_THEN_SUFFIX);
        public static readonly Location IMPLEMENTS = new(Space.Location.IMPLEMENTS_SUFFIX);
        public static readonly Location PERMITS = new(Space.Location.PERMITS_SUFFIX);
        public static readonly Location IMPORT = new(Space.Location.IMPORT_SUFFIX);
        public static readonly Location INSTANCEOF = new(Space.Location.INSTANCEOF_SUFFIX);
        public static readonly Location LABEL = new(Space.Location.LABEL_SUFFIX);
        public static readonly Location LAMBDA_PARAM = new(Space.Location.LAMBDA_PARAMETER);
        public static readonly Location LANGUAGE_EXTENSION = new(Space.Location.LANGUAGE_EXTENSION);
        public static readonly Location MEMBER_REFERENCE_CONTAINING = new(Space.Location.MEMBER_REFERENCE_CONTAINING);
        public static readonly Location METHOD_DECLARATION_PARAMETER = new(Space.Location.METHOD_DECLARATION_PARAMETER_SUFFIX);
        public static readonly Location METHOD_INVOCATION_ARGUMENT = new(Space.Location.METHOD_INVOCATION_ARGUMENT_SUFFIX);
        public static readonly Location METHOD_SELECT = new(Space.Location.METHOD_SELECT_SUFFIX);
        public static readonly Location NAMED_VARIABLE = new(Space.Location.NAMED_VARIABLE_SUFFIX);
        public static readonly Location NEW_ARRAY_INITIALIZER = new(Space.Location.NEW_ARRAY_INITIALIZER_SUFFIX);
        public static readonly Location NEW_CLASS_ARGUMENTS = new(Space.Location.NEW_CLASS_ARGUMENTS_SUFFIX);
        public static readonly Location NEW_CLASS_ENCLOSING = new(Space.Location.NEW_CLASS_ENCLOSING_SUFFIX);
        public static readonly Location NULLABLE = new(Space.Location.NULLABLE_TYPE_SUFFIX);
        public static readonly Location PACKAGE = new(Space.Location.PACKAGE_SUFFIX);
        public static readonly Location PARENTHESES = new(Space.Location.PARENTHESES_SUFFIX);
        public static readonly Location RECORD_STATE_VECTOR = new(Space.Location.RECORD_STATE_VECTOR_SUFFIX);
        public static readonly Location STATIC_INIT = new(Space.Location.STATIC_INIT_SUFFIX);
        public static readonly Location THROWS = new(Space.Location.THROWS_SUFFIX);
        public static readonly Location TRY_RESOURCE = new(Space.Location.TRY_RESOURCE_SUFFIX);
        public static readonly Location TYPE_PARAMETER = new(Space.Location.TYPE_PARAMETER_SUFFIX);
        public static readonly Location TYPE_BOUND = new(Space.Location.TYPE_BOUND_SUFFIX);
        public static readonly Location WHILE_BODY = new(Space.Location.WHILE_BODY_SUFFIX);
    }

    public static JRightPadded<T> WithElement<T>(JRightPadded<T> right, T element)
    {
        return right.WithElement(element);
    }
}
