using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed record Comment(
    bool Multiline,
    string Text,
    string Suffix,
    Markers Markers
)
{
    public Comment WithMultiline(bool multiline)
    {
        return multiline == Multiline ? this : this with { Multiline = multiline };
    }

    public Comment WithText(string text)
    {
        return text == Text ? this : this with { Text = text };
    }

    public Comment WithSuffix(string suffix)
    {
        return suffix == Suffix ? this : this with { Suffix = suffix };
    }

    public Comment WithMarkers(Markers markers)
    {
        return ReferenceEquals(markers, Markers) ? this : this with { Markers = markers };
    }

}
