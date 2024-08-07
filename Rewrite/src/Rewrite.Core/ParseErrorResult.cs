using Rewrite.Core.Internal;

namespace Rewrite.Core;

public class ParseExceptionResult(
    Guid id,
    string parserType,
    string exceptionType,
    string message,
    string? treeType) : Marker.Marker
{
    public Guid Id => id;

    public ParseExceptionResult WithId(Guid newId)
    {
        return newId == Id ? this : new ParseExceptionResult(newId, parserType, exceptionType, message, treeType);
    }

    public string ParserType => parserType;

    public ParseExceptionResult WithParserType(string newParserType)
    {
        return newParserType == ParserType
            ? this
            : new ParseExceptionResult(Id, newParserType, exceptionType, message, treeType);
    }

    public string ExceptionType => exceptionType;

    public ParseExceptionResult WithExceptionType(string newExceptionType)
    {
        return newExceptionType == exceptionType
            ? this
            : new ParseExceptionResult(id, parserType, newExceptionType, message, treeType);
    }

    public string Message => message;

    public ParseExceptionResult WithMessage(string newMessage)
    {
        return newMessage == message
            ? this
            : new ParseExceptionResult(id, parserType, exceptionType, newMessage, treeType);
    }

    public string? TreeType => treeType;

    public ParseExceptionResult WithTreeType(string? newTreeType)
    {
        return newTreeType == treeType
            ? this
            : new ParseExceptionResult(id, parserType, exceptionType, message, newTreeType);
    }

    /**
     * The type of tree element that was being parsed when the failure occurred.
     */
    public static ParseExceptionResult build(Type parserClass,
        Exception t,
        string message)
    {
        var simpleName = t.GetType().Name;
        return new ParseExceptionResult(
            Tree.RandomId(),
            parserClass.Name,
            !string.IsNullOrEmpty(simpleName) ? simpleName : t.GetType().Name,
            (message != null ? message : "") + ExceptionUtils.SanitizeStackTrace(t, parserClass),
            null
        );
    }

    public static ParseExceptionResult build(Parser parser, Exception t, string? message)
    {
        return build(parser.GetType(), t, message);
    }

    public static ParseExceptionResult build(Parser parser, Exception t)
    {
        return build(parser.GetType(), t, null);
    }

    public bool Equals(Marker.Marker? other)
    {
        return other is ParseExceptionResult && other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}