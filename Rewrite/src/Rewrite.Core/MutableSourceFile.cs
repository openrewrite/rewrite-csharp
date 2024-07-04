namespace Rewrite.Core;

public interface MutableSourceFile<out T> : SourceFile where T : MutableSourceFile<T>
{
    T WithSourcePath(string sourcePath);
    T WithCharsetName(string? charsetName);
    T WithCharsetBomMarked(bool charsetBomMarked);
    T WithChecksum(Checksum? checksum);
    T WithFileAttributes(FileAttributes? fileAttributes);
}