namespace Rewrite.Core;

public interface SourceFile : Tree
{
    string SourcePath { get; }
    string? CharsetName { get; }
    bool CharsetBomMarked { get; }
    Checksum? Checksum { get; }
    FileAttributes? FileAttributes { get; }

    string PrintAll()
    {
        return PrintAll(0);
    }

    string PrintAll<P>(P p)
    {
        return PrintAll(new PrintOutputCapture<P>(p));
    }

    string PrintAll<P>(PrintOutputCapture<P> capture)
    {
        return Print(new Cursor(null, "root"), capture);
    }

    new ITreeVisitor<Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
    {
        throw new NotImplementedException("SourceFile implementations should override this method");
    }
}