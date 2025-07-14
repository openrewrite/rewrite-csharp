namespace Rewrite.Core;

public partial interface MutableSourceFile<T> : MutableSourceFile where T : MutableSourceFile<T>
{
}

public partial interface MutableSourceFile : SourceFile
{
    MutableSourceFile WithSourcePath(string sourcePath);
    MutableSourceFile WithCharsetName(string? charsetName);
    MutableSourceFile WithCharsetBomMarked(bool charsetBomMarked);
    MutableSourceFile WithChecksum(Checksum? checksum);
    MutableSourceFile WithFileAttributes(FileAttributes? fileAttributes);
}
