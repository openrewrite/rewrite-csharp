using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
#if DEBUG_VISITOR
[DebuggerStepThrough]
#endif
public partial class Space : IEquatable<Space>
{
    private readonly Lazy<int> _hashCode;
    private readonly IList<Comment> _comments;

    private readonly string _whitespace;

    public bool Equals(Space? other)
    {
        return Equals(this, other);
    }

    public static bool Equals(Space? v1, Space? v2)
    {
        if (ReferenceEquals(v1, v2)) return true;
        if (v1 is null && v2 is null) return false;
        if(v1 is null || v2 is not null) return false;
        if(v1 is not null || v2 is null) return false;

        return v1!.Comments.SequenceEqual(v2.Comments) && v1.Whitespace == v2.Whitespace;
    }

    public static bool operator ==(Space? v1, Space? v2) => Equals(v1, v2);

    public static bool operator !=(Space? v1, Space? v2) => !(v1 == v2);


    public Space(IList<Comment> comments, string? whitespace)
    {
        _comments = comments;
        _whitespace = whitespace ?? "";
        _hashCode =  new(() =>
        {
            var hashCode = _whitespace.GetHashCode();
            foreach (var comment in _comments)
            {
                HashCode.Combine(hashCode, comment.GetHashCode());
            }

            return hashCode;
        });
    }


    public static readonly Space EMPTY = new([], "");

    public static readonly Space SINGLE_SPACE = new([], " ");

    public IList<Comment> Comments => _comments;

    public string Whitespace => _whitespace;

    public bool IsEmpty => this == EMPTY;

    public Space WithComments(IList<Comment>? newComments)
    {
        return newComments == Comments ? this : new Space(newComments!, Whitespace);
    }

    public Space WithWhitespace(string newWhitespace)
    {
        return newWhitespace == Whitespace ? this : new Space(Comments, newWhitespace);
    }

    public static Space FirstPrefix<T>(IList<T>? trees) where T : J
    {
        return (trees == null || trees.Count == 0) ? Space.EMPTY : trees[0].Prefix;
    }

    /// <summary>
    /// Formats the first prefix of the list of trees. If the list is not empty and the prefix
    /// of the first tree does not match the specified prefix, a new list is created with the
    /// first tree's prefix updated.
    /// </summary>
    /// <typeparam name="J2">A type that extends J.</typeparam>
    /// <param name="trees">The list of trees to format.</param>
    /// <param name="prefix">The prefix to apply to the first tree.</param>
    /// <returns>
    /// A new list with the first tree's prefix updated if necessary; otherwise, the original list.
    /// </returns>
    public static IList<J2> FormatFirstPrefix<J2>(IList<J2> trees, Space prefix) where J2 : J
    {
        if (trees.Count > 0 && !trees[0].Prefix.Equals(prefix))
        {
            // Create a new list to avoid mutating the original list
            List<J2> formattedTrees = new List<J2>(trees);

            // Update the first element with the new prefix
            formattedTrees[0] = (J2)formattedTrees[0].WithPrefix(prefix);

            return formattedTrees;
        }

        return trees;
    }

    public static Space Format(string formatting)
    {
        return Format(formatting, 0, formatting.Length);
    }

    public static Space Format(string formatting, int beginIndex, int toIndex)
    {
        if (beginIndex == toIndex)
        {
            return EMPTY;
        }
        else if (toIndex == beginIndex + 1 && ' ' == formatting[beginIndex])
        {
            return SINGLE_SPACE;
        }
        else
        {
            RangeCheck(formatting.Length, beginIndex, toIndex);
        }

        var prefix = new StringBuilder();
        var comment = new StringBuilder();
        var comments = new List<Comment>(1);

        var inSingleLineComment = false;
        var inMultiLineComment = false;

        var last = '\0';

        for (var i = beginIndex; i < toIndex; i++)
        {
            var c = formatting[i];
            switch (c)
            {
                case '/':
                    if (inSingleLineComment)
                    {
                        comment.Append(c);
                    }
                    else if (last == '/' && !inMultiLineComment)
                    {
                        inSingleLineComment = true;
                        comment.Length = 0; // equivalent to setLength in Java
                        prefix.Length -= 1;
                    }
                    else if (last == '*' && inMultiLineComment && comment.Length > 0)
                    {
                        inMultiLineComment = false;
                        comment.Length -= 1;
                        comments.Add(new TextComment(true, comment.ToString(), prefix.ToString(0, prefix.Length - 1), Markers.EMPTY));
                        prefix.Length = 0;
                        comment.Length = 0;
                        continue;
                    }
                    else if (inMultiLineComment)
                    {
                        comment.Append(c);
                    }
                    else
                    {
                        prefix.Append(c);
                    }

                    break;
                case '\r':
                case '\n':
                    if (inSingleLineComment)
                    {
                        inSingleLineComment = false;
                        comments.Add(new TextComment(false, comment.ToString(), prefix.ToString(), Markers.EMPTY));
                        prefix.Length = 0;
                        comment.Length = 0;
                        prefix.Append(c);
                    }
                    else if (!inMultiLineComment)
                    {
                        prefix.Append(c);
                    }
                    else
                    {
                        comment.Append(c);
                    }

                    break;
                case '*':
                    if (inSingleLineComment)
                    {
                        comment.Append(c);
                    }
                    else if (last == '/' && !inMultiLineComment)
                    {
                        inMultiLineComment = true;
                        comment.Length = 0;
                    }
                    else
                    {
                        comment.Append(c);
                    }

                    break;
                default:
                    if (inSingleLineComment || inMultiLineComment)
                    {
                        comment.Append(c);
                    }
                    else
                    {
                        prefix.Append(c);
                    }

                    break;
            }

            last = c;
        }

        if (comment.Length > 0)
        {
            comments.Add(new TextComment(false, comment.ToString(), prefix.ToString(), Markers.EMPTY));
            prefix.Length = 0;
        }

        var whitespace = prefix.ToString();
        if (comments.Count != 0)
        {
            for (var i = comments.Count - 1; i >= 0; i--)
            {
                var c = (TextComment)comments[i];
                var next = c.Suffix;
                comments[i] = c.WithSuffix(whitespace);
                whitespace = next;
            }
        }

        return Build(whitespace, comments);
    }

    public override string ToString()
    {
        StringBuilder p = new();
        p.Append(Whitespace);

        for (int i = 0; i < _comments.Count; ++i)
        {
            var comment = _comments[i];
            var text = ((TextComment)comment).Text;
            p.Append(comment.Multiline ? $"/*{text}*/" : $"//{text}");
            p.Append(comment.Suffix);
        }

        return p.ToString();
    }

    // public enum Location
    // {
    //     ANNOTATED_TYPE_PREFIX,
    //     ANNOTATIONS,
    //     ANNOTATION_ARGUMENTS,
    //     ANNOTATION_ARGUMENT_SUFFIX,
    //     ANNOTATION_PREFIX,
    //     ANY,
    //     ARRAY_ACCESS_PREFIX,
    //     ARRAY_INDEX_SUFFIX,
    //     ARRAY_TYPE_PREFIX,
    //     ASSERT_DETAIL,
    //     ASSERT_DETAIL_PREFIX,
    //     ASSERT_PREFIX,
    //     ASSIGNMENT,
    //     ASSIGNMENT_OPERATION_OPERATOR,
    //     ASSIGNMENT_OPERATION_PREFIX,
    //     ASSIGNMENT_PREFIX,
    //     BINARY_OPERATOR,
    //     BINARY_PREFIX,
    //     BLOCK_END,
    //     BLOCK_PREFIX,
    //     BLOCK_STATEMENT_SUFFIX,
    //     BREAK_PREFIX,
    //     CASE,
    //     CASE_BODY,
    //     CASE_CASE_LABELS,
    //     CASE_PREFIX,
    //     CASE_SUFFIX,
    //     CATCH_ALTERNATIVE_SUFFIX,
    //     CATCH_PREFIX,
    //     CLASS_DECLARATION_PREFIX,
    //     CLASS_KIND,
    //     COMPILATION_UNIT_EOF,
    //     COMPILATION_UNIT_PREFIX,
    //     CONTINUE_PREFIX,
    //     CONTROL_PARENTHESES_PREFIX,
    //     DECONSTRUCTION_PATTERN_NESTED,
    //     DECONSTRUCTION_PATTERN_PREFIX,
    //     DIMENSION,
    //     DIMENSION_PREFIX,
    //     DIMENSION_SUFFIX,
    //     DO_WHILE_PREFIX,
    //     ELSE_PREFIX,
    //     EMPTY_PREFIX,
    //     ENUM_VALUE_PREFIX,
    //     ENUM_VALUE_SET_PREFIX,
    //     ENUM_VALUE_SUFFIX,
    //     ERRONEOUS_PREFIX,
    //     EXPRESSION_PREFIX,
    //     EXTENDS,
    //     FIELD_ACCESS_NAME,
    //     FIELD_ACCESS_PREFIX,
    //     FOREACH_ITERABLE_SUFFIX,
    //     FOREACH_VARIABLE_SUFFIX,
    //     FOR_BODY_SUFFIX,
    //     FOR_CONDITION_SUFFIX,
    //     FOR_CONTROL_PREFIX,
    //     FOR_EACH_CONTROL_PREFIX,
    //     FOR_EACH_LOOP_PREFIX,
    //     FOR_INIT_SUFFIX,
    //     FOR_PREFIX,
    //     FOR_UPDATE_SUFFIX,
    //     IDENTIFIER_PREFIX,
    //     IF_ELSE_SUFFIX,
    //     IF_PREFIX,
    //     IF_THEN_SUFFIX,
    //     IMPLEMENTS,
    //     IMPLEMENTS_SUFFIX,
    //     IMPORT_ALIAS_PREFIX,
    //     IMPORT_PREFIX,
    //     IMPORT_SUFFIX,
    //     INSTANCEOF_PREFIX,
    //     INSTANCEOF_SUFFIX,
    //     INTERSECTION_TYPE_PREFIX,
    //     LABEL_PREFIX,
    //     LABEL_SUFFIX,
    //     LAMBDA_ARROW_PREFIX,
    //     LAMBDA_PARAMETER,
    //     LAMBDA_PARAMETERS_PREFIX,
    //     LAMBDA_PREFIX,
    //     LANGUAGE_EXTENSION,
    //     LITERAL_PREFIX,
    //     MEMBER_REFERENCE_CONTAINING,
    //     MEMBER_REFERENCE_NAME,
    //     MEMBER_REFERENCE_PREFIX,
    //     METHOD_DECLARATION_DEFAULT_VALUE,
    //     METHOD_DECLARATION_PARAMETERS,
    //     METHOD_DECLARATION_PARAMETER_SUFFIX,
    //     METHOD_DECLARATION_PREFIX,
    //     METHOD_INVOCATION_ARGUMENTS,
    //     METHOD_INVOCATION_ARGUMENT_SUFFIX,
    //     METHOD_INVOCATION_NAME,
    //     METHOD_INVOCATION_PREFIX,
    //     METHOD_SELECT_SUFFIX,
    //     MODIFIER_PREFIX,
    //     MULTI_CATCH_PREFIX,
    //     NAMED_VARIABLE_SUFFIX,
    //     NEW_ARRAY_INITIALIZER,
    //     NEW_ARRAY_INITIALIZER_SUFFIX,
    //     NEW_ARRAY_PREFIX,
    //     NEW_CLASS_ARGUMENTS,
    //     NEW_CLASS_ARGUMENTS_SUFFIX,
    //     NEW_CLASS_ENCLOSING_SUFFIX,
    //     NEW_CLASS_PREFIX,
    //     NEW_PREFIX,
    //     NULLABLE_TYPE_PREFIX,
    //     NULLABLE_TYPE_SUFFIX,
    //     PACKAGE_PREFIX,
    //     PACKAGE_SUFFIX,
    //     PARAMETERIZED_TYPE_PREFIX,
    //     PARENTHESES_PREFIX,
    //     PARENTHESES_SUFFIX,
    //     PERMITS,
    //     PERMITS_SUFFIX,
    //     PRIMITIVE_PREFIX,
    //     RECORD_STATE_VECTOR,
    //     RECORD_STATE_VECTOR_SUFFIX,
    //     RETURN_PREFIX,
    //     STATEMENT_PREFIX,
    //     STATIC_IMPORT,
    //     STATIC_INIT_SUFFIX,
    //     SWITCH_EXPRESSION_PREFIX,
    //     SWITCH_PREFIX,
    //     SYNCHRONIZED_PREFIX,
    //     TERNARY_FALSE,
    //     TERNARY_PREFIX,
    //     TERNARY_TRUE,
    //     THROWS,
    //     THROWS_SUFFIX,
    //     THROW_PREFIX,
    //     TRY_FINALLY,
    //     TRY_PREFIX,
    //     TRY_RESOURCE,
    //     TRY_RESOURCES,
    //     TRY_RESOURCE_SUFFIX,
    //     TYPE_BOUNDS,
    //     TYPE_BOUND_SUFFIX,
    //     TYPE_CAST_PREFIX,
    //     TYPE_PARAMETERS,
    //     TYPE_PARAMETERS_PREFIX,
    //     TYPE_PARAMETER_SUFFIX,
    //     UNARY_OPERATOR,
    //     UNARY_PREFIX,
    //     UNKNOWN_PREFIX,
    //     UNKNOWN_SOURCE_PREFIX,
    //     VARARGS,
    //     VARIABLE_DECLARATIONS_PREFIX,
    //     VARIABLE_INITIALIZER,
    //     VARIABLE_PREFIX,
    //     WHILE_BODY_SUFFIX,
    //     WHILE_CONDITION,
    //     WHILE_PREFIX,
    //     WILDCARD_BOUND,
    //     WILDCARD_PREFIX,
    //     YIELD_PREFIX,
    // }

    private static Space Build(string str, IList<Comment> comments)
    {
        return new Space(comments, str);
    }

    private static void RangeCheck(int arrayLength, int fromIndex, int toIndex)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(fromIndex, toIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(fromIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(toIndex, arrayLength);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Space)obj);
    }

    public override int GetHashCode() => _hashCode.Value;
}

internal enum JSpaceLocation
{

    ANNOTATED_TYPE_PREFIX,
    ANNOTATIONS,
    ANNOTATION_ARGUMENTS,
    ANNOTATION_ARGUMENT_SUFFIX,
    ANNOTATION_PREFIX,
    ANY,
    ARRAY_ACCESS_PREFIX,
    ARRAY_INDEX_SUFFIX,
    ARRAY_TYPE_PREFIX,
    ASSERT_DETAIL,
    ASSERT_DETAIL_PREFIX,
    ASSERT_PREFIX,
    ASSIGNMENT,
    ASSIGNMENT_OPERATION_OPERATOR,
    ASSIGNMENT_OPERATION_PREFIX,
    ASSIGNMENT_PREFIX,
    BINARY_OPERATOR,
    BINARY_PREFIX,
    BLOCK_END,
    BLOCK_PREFIX,
    BLOCK_STATEMENT_SUFFIX,
    BREAK_PREFIX,
    CASE,
    CASE_BODY,
    CASE_CASE_LABELS,
    CASE_PREFIX,
    CASE_SUFFIX,
    CATCH_ALTERNATIVE_SUFFIX,
    CATCH_PREFIX,
    CLASS_DECLARATION_PREFIX,
    CLASS_KIND,
    COMPILATION_UNIT_EOF,
    COMPILATION_UNIT_PREFIX,
    CONTINUE_PREFIX,
    CONTROL_PARENTHESES_PREFIX,
    DECONSTRUCTION_PATTERN_NESTED,
    DECONSTRUCTION_PATTERN_PREFIX,
    DIMENSION,
    DIMENSION_PREFIX,
    DIMENSION_SUFFIX,
    DO_WHILE_PREFIX,
    ELSE_PREFIX,
    EMPTY_PREFIX,
    ENUM_VALUE_PREFIX,
    ENUM_VALUE_SET_PREFIX,
    ENUM_VALUE_SUFFIX,
    ERRONEOUS_PREFIX,
    EXPRESSION_PREFIX,
    EXTENDS,
    FIELD_ACCESS_NAME,
    FIELD_ACCESS_PREFIX,
    FOREACH_ITERABLE_SUFFIX,
    FOREACH_VARIABLE_SUFFIX,
    FOR_BODY_SUFFIX,
    FOR_CONDITION_SUFFIX,
    FOR_CONTROL_PREFIX,
    FOR_EACH_CONTROL_PREFIX,
    FOR_EACH_LOOP_PREFIX,
    FOR_INIT_SUFFIX,
    FOR_PREFIX,
    FOR_UPDATE_SUFFIX,
    IDENTIFIER_PREFIX,
    IF_ELSE_SUFFIX,
    IF_PREFIX,
    IF_THEN_SUFFIX,
    IMPLEMENTS,
    IMPLEMENTS_SUFFIX,
    IMPORT_ALIAS_PREFIX,
    IMPORT_PREFIX,
    IMPORT_SUFFIX,
    INSTANCEOF_PREFIX,
    INSTANCEOF_SUFFIX,
    INTERSECTION_TYPE_PREFIX,
    LABEL_PREFIX,
    LABEL_SUFFIX,
    LAMBDA_ARROW_PREFIX,
    LAMBDA_PARAMETER,
    LAMBDA_PARAMETERS_PREFIX,
    LAMBDA_PREFIX,
    LANGUAGE_EXTENSION,
    LITERAL_PREFIX,
    MEMBER_REFERENCE_CONTAINING,
    MEMBER_REFERENCE_NAME,
    MEMBER_REFERENCE_PREFIX,
    METHOD_DECLARATION_DEFAULT_VALUE,
    METHOD_DECLARATION_PARAMETERS,
    METHOD_DECLARATION_PARAMETER_SUFFIX,
    METHOD_DECLARATION_PREFIX,
    METHOD_INVOCATION_ARGUMENTS,
    METHOD_INVOCATION_ARGUMENT_SUFFIX,
    METHOD_INVOCATION_NAME,
    METHOD_INVOCATION_PREFIX,
    METHOD_SELECT_SUFFIX,
    MODIFIER_PREFIX,
    MULTI_CATCH_PREFIX,
    NAMED_VARIABLE_SUFFIX,
    NEW_ARRAY_INITIALIZER,
    NEW_ARRAY_INITIALIZER_SUFFIX,
    NEW_ARRAY_PREFIX,
    NEW_CLASS_ARGUMENTS,
    NEW_CLASS_ARGUMENTS_SUFFIX,
    NEW_CLASS_ENCLOSING_SUFFIX,
    NEW_CLASS_PREFIX,
    NEW_PREFIX,
    NULLABLE_TYPE_PREFIX,
    NULLABLE_TYPE_SUFFIX,
    PACKAGE_PREFIX,
    PACKAGE_SUFFIX,
    PARAMETERIZED_TYPE_PREFIX,
    PARENTHESES_PREFIX,
    PARENTHESES_SUFFIX,
    PERMITS,
    PERMITS_SUFFIX,
    PRIMITIVE_PREFIX,
    RECORD_STATE_VECTOR,
    RECORD_STATE_VECTOR_SUFFIX,
    RETURN_PREFIX,
    STATEMENT_PREFIX,
    STATIC_IMPORT,
    STATIC_INIT_SUFFIX,
    SWITCH_EXPRESSION_PREFIX,
    SWITCH_PREFIX,
    SYNCHRONIZED_PREFIX,
    TERNARY_FALSE,
    TERNARY_PREFIX,
    TERNARY_TRUE,
    THROWS,
    THROWS_SUFFIX,
    THROW_PREFIX,
    TRY_FINALLY,
    TRY_PREFIX,
    TRY_RESOURCE,
    TRY_RESOURCES,
    TRY_RESOURCE_SUFFIX,
    TYPE_BOUNDS,
    TYPE_BOUND_SUFFIX,
    TYPE_CAST_PREFIX,
    TYPE_PARAMETERS,
    TYPE_PARAMETERS_PREFIX,
    TYPE_PARAMETER_SUFFIX,
    UNARY_OPERATOR,
    UNARY_PREFIX,
    UNKNOWN_PREFIX,
    UNKNOWN_SOURCE_PREFIX,
    VARARGS,
    VARIABLE_DECLARATIONS_PREFIX,
    VARIABLE_INITIALIZER,
    VARIABLE_PREFIX,
    WHILE_BODY_SUFFIX,
    WHILE_CONDITION,
    WHILE_PREFIX,
    WILDCARD_BOUND,
    WILDCARD_PREFIX,
    YIELD_PREFIX,

}
