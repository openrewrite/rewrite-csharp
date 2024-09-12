using Rewrite.Core.Marker;

namespace Rewrite.Core.Quark;

// FIXME implement
public abstract class Quark : SourceFile
{
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

    public string SourcePath { get; } = null!;
    public string? CharsetName { get; }
    public bool CharsetBomMarked { get; }
    public Checksum? Checksum { get; }
    public FileAttributes? FileAttributes { get; }
}