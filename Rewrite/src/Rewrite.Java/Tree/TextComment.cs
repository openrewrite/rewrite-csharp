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
public sealed class TextComment : Comment
{
    private readonly Lazy<int> _hashCode;
    private readonly bool _multiline;
    private readonly string _text;
    private readonly string _suffix;
    private readonly Markers _markers;

    public TextComment(bool multiline,
        string text,
        string suffix,
        Markers markers)
    {
        _multiline = multiline;
        _text = text;
        _suffix = suffix;
        _markers = markers;
        _hashCode = new(() => HashCode.Combine(_multiline, _text, _suffix, _markers));
    }

    public bool Multiline => _multiline;

    public TextComment WithMultiline(bool newMultiline)
    {
        return newMultiline == _multiline ? this : new TextComment(newMultiline, _text, _suffix, _markers);
    }

    public string Text => _text;

    public string Suffix => _suffix;

    public Markers Markers => _markers;

    private static readonly Func<string, string> MARKER_WRAPPER =
        o => "/*~~" + o + (o.Length == 0 ? "" : "~~") + ">*/";

    public void PrintComment<P>(Cursor cursor, PrintOutputCapture<P> p)
    {
        foreach (var marker in _markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(cursor, this), MARKER_WRAPPER));
        }

        p.Append(_multiline ? $"/*{_text}*/" : $"//{_text}");
        foreach (var marker in _markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(cursor, this), MARKER_WRAPPER));
        }
    }

    public TextComment WithSuffix(string newSuffix)
    {
        return newSuffix == Suffix ? this : new TextComment(_multiline, _text, newSuffix, Markers);
    }

    Comment Comment.WithSuffix(string newSuffix) => WithSuffix(newSuffix);

    public TextComment WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, _markers) ? this : new TextComment(_multiline, _text, _suffix, newMarkers);
    }

    public TextComment WithText(string newText)
    {
        return newText == Text ? this : new TextComment(Multiline, newText, _suffix, _markers);
    }

    public bool Equals(TextComment? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Markers.Id == other.Markers.Id && this.Multiline == other.Multiline && this.Suffix == other.Suffix && this.Text == other.Text;
    }

    bool IEquatable<Comment>.Equals(Comment? other)
    {
        return other is TextComment textComment && Equals(textComment);
    }
}
