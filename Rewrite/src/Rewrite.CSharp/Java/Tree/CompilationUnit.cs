using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

public partial interface J
{
    public partial class CompilationUnit<T>(
        Guid id,
        Space prefix,
        Markers markers,
        string sourcePath,
        FileAttributes? fileAttributes,
        string? charsetName,
        bool charsetBomMarked,
        Checksum? checksum,
        JRightPadded<Package>? packageDeclaration,
        IList<JRightPadded<Import>> imports,
        IList<ClassDeclaration> classes,
        Space eof
    ) : J.CompilationUnit(id,prefix, markers, sourcePath,
         fileAttributes,
         charsetName, charsetBomMarked,
         checksum,
         packageDeclaration,
         imports,
         classes, eof
    )
    {
        
    }
}