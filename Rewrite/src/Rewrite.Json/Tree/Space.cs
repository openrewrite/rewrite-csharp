using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Space(
    IList<Comment> comments,
    string? whitespace
)
{
    public static readonly Space EMPTY = new([], "");
    public static readonly Space SINGLE_SPACE = new([], " ");

    public IList<Comment> Comments => comments;

    public string? Whitespace => whitespace;

    public Space WithComments(IList<Comment> newComments)
    {
        return newComments == Comments ? this : new Space(newComments, whitespace);
    }

    public Space WithWhitespace(string? newWhitespace)
    {
        return newWhitespace == whitespace ? this : new Space(Comments, newWhitespace);
    }
}