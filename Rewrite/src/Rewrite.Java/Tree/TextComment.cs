using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
[DebuggerStepThrough]
public sealed class TextComment(
    bool multiline,
    string text,
    string suffix,
    Markers markers
) : Comment
{
    public bool Multiline => multiline;

    public TextComment WithMultiline(bool newMultiline)
    {
        return newMultiline == multiline ? this : new TextComment(newMultiline, text, suffix, markers);
    }

    public string Text => text;

    public string Suffix => suffix;

    public Markers Markers => markers;

    private static readonly Func<string, string> MARKER_WRAPPER =
        o => "/*~~" + o + (o.Length == 0 ? "" : "~~") + ">*/";

    public void PrintComment<P>(Cursor cursor, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(cursor, this), MARKER_WRAPPER));
        }

        p.Append(multiline ? $"/*{text}*/" : $"//{text}");
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(cursor, this), MARKER_WRAPPER));
        }
    }

    public TextComment WithSuffix(string newSuffix)
    {
        return newSuffix == Suffix ? this : new TextComment(multiline, text, newSuffix, Markers);
    }

    Comment Comment.WithSuffix(string newSuffix) => WithSuffix(newSuffix);

    public TextComment WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, markers) ? this : new TextComment(multiline, text, suffix, newMarkers);
    }

    public TextComment WithText(string newText)
    {
        return newText == Text ? this : new TextComment(Multiline, newText, suffix, markers);
    }
}
