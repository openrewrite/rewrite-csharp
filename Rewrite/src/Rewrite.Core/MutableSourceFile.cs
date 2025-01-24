namespace Rewrite.Core;

public interface MutableSourceFile<out T> : MutableSourceFile where T : MutableSourceFile<T>
{
    new T WithSourcePath(string sourcePath);
    MutableSourceFile MutableSourceFile.WithSourcePath(string sourcePath) => WithSourcePath(sourcePath);
    new T WithCharsetName(string? charsetName);
    MutableSourceFile MutableSourceFile.WithCharsetName(string? charsetName) => WithCharsetName(charsetName);
    new T WithCharsetBomMarked(bool charsetBomMarked);
    MutableSourceFile MutableSourceFile.WithCharsetBomMarked(bool charsetBomMarked) => WithCharsetBomMarked(charsetBomMarked);
    new T WithChecksum(Checksum? checksum);
    MutableSourceFile MutableSourceFile.WithChecksum(Checksum? checksum) => WithChecksum(checksum);
    new T WithFileAttributes(FileAttributes? fileAttributes);
    MutableSourceFile MutableSourceFile.WithFileAttributes(FileAttributes? fileAttributes) => WithFileAttributes(fileAttributes);
}

public interface MutableSourceFile : SourceFile
{
    MutableSourceFile WithSourcePath(string sourcePath);
    MutableSourceFile WithCharsetName(string? charsetName);
    MutableSourceFile WithCharsetBomMarked(bool charsetBomMarked);
    MutableSourceFile WithChecksum(Checksum? checksum);
    MutableSourceFile WithFileAttributes(FileAttributes? fileAttributes);
}
