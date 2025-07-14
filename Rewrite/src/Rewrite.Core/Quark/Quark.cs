using Rewrite.Core.Marker;

namespace Rewrite.Core.Quark;

// FIXME implement
public partial class Quark : SourceFile
{
    global::Rewrite.Core.Tree global::Rewrite.Core.Tree.WithMarkers(global::Rewrite.Core.Marker.Markers markers) => WithMarkers(markers);
    public Quark(Guid id, string sourcePath, string? charsetName, bool charsetBomMarked, Checksum? checksum, FileAttributes? fileAttributes)
    {
        Id = id;
        SourcePath = sourcePath;
        CharsetName = charsetName;
        CharsetBomMarked = charsetBomMarked;
        Checksum = checksum;
        FileAttributes = fileAttributes;
    }

    public bool Equals(Tree? other)
    {
        throw new NotImplementedException();
    }

    public Guid Id { get; }
    public Markers Markers { get; } = Markers.EMPTY;
    public bool IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree
    {
        throw new NotImplementedException();
    }

    // public Quark WithId(Guid id) => throw new NotImplementedException();

    public string SourcePath { get; } = null!;
    public string? CharsetName { get; }
    public bool CharsetBomMarked { get; }
    public Checksum? Checksum { get; }
    public FileAttributes? FileAttributes { get; }
}