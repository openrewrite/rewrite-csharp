using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface Comment
{
    public bool Multiline { get; }

    public string Suffix { get; }

    public Markers Markers { get; }
    
    void PrintComment<P>(Cursor cursor, PrintOutputCapture<P> p);

}
