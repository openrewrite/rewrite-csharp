using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface Comment : IEquatable<Comment>
{
    public bool Multiline { get; }

    public string Suffix { get; }

    public Markers Markers { get; }

    void PrintComment<P>(Cursor cursor, PrintOutputCapture<P> p);

    Comment WithSuffix(string replace);
}
