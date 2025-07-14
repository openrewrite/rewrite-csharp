using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteText;

public partial class PlainTextSourceFile : SourceFile
{
    public PlainTextSourceFile(Guid id, Markers markers, string sourcePath, string? charsetName, bool charsetBomMarked, Checksum? checksum, FileAttributes? fileAttributes)
    {
        Id = id;
        Markers = markers;
        SourcePath = sourcePath;
        CharsetName = charsetName;
        CharsetBomMarked = charsetBomMarked;
        Checksum = checksum;
        FileAttributes = fileAttributes;
    }

    public Guid Id { get; }
    public Markers Markers { get; }
    public string SourcePath { get; }
    public string? CharsetName { get; }
    public bool CharsetBomMarked { get; }
    public Checksum? Checksum { get; }
    public FileAttributes? FileAttributes { get; }

    public bool Equals(Tree? other)
    {
        throw new NotImplementedException();
    }

    public bool IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree
    {
        throw new NotImplementedException();
    }
}