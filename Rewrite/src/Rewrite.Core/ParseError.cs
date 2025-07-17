using System.Text;
using Rewrite.Core.Marker;

namespace Rewrite.Core;

public partial class ParseError(
    Guid id,
    Markers markers,
    string sourcePath,
    FileAttributes? fileAttributes,
    string? charsetName,
    bool charsetBomMarked,
    Checksum? checksum,
    string text,
    SourceFile? erroneous) : SourceFile
{
    public Guid Id => id;

    // public ParseError WithId(Guid newId)
    // {
    //     return newId == id
    //         ? this
    //         : new ParseError(newId, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, text,
    //             erroneous);
    // }

    public Markers Markers => markers;
    public ParseError WithId(Guid id) => throw new NotImplementedException();
    public ParseError WithMarkers(Markers newMarkers)
    {
        return ReferenceEquals(newMarkers, markers)
            ? this
            : new ParseError(id, newMarkers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, text,
                erroneous);
    }


    public string SourcePath => sourcePath;

    public ParseError WithSourcePath(string newSourcePath)
    {
        return newSourcePath == sourcePath
            ? this
            : new ParseError(id, markers, newSourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, text,
                erroneous);
    }

    public FileAttributes? FileAttributes => fileAttributes;

    public ParseError WithFileAttributes(FileAttributes? newFileAttributes)
    {
        return newFileAttributes == fileAttributes
            ? this
            : new ParseError(id, markers, sourcePath, newFileAttributes, charsetName, charsetBomMarked, checksum, text,
                erroneous);
    }

    public string? CharsetName => charsetName;

    public ParseError WithCharsetName(string? newCharsetName)
    {
        return newCharsetName == charsetName
            ? this
            : new ParseError(id, markers, sourcePath, fileAttributes, newCharsetName, charsetBomMarked, checksum, text,
                erroneous);
    }

    public bool CharsetBomMarked => charsetBomMarked;

    public ParseError WithCharsetBomMarked(bool newCharsetBomMarked)
    {
        return newCharsetBomMarked == charsetBomMarked
            ? this
            : new ParseError(id, markers, sourcePath, fileAttributes, charsetName, newCharsetBomMarked, checksum, text,
                erroneous);
    }

    public Checksum? Checksum => checksum;

    public ParseError WithChecksum(Checksum? newChecksum)
    {
        return newChecksum == checksum
            ? this
            : new ParseError(id, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, newChecksum, text,
                erroneous);
    }

    public string Text => text;

    public ParseError WithText(string newText)
    {
        return newText == text
            ? this
            : new ParseError(id, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, newText,
                erroneous);
    }

    public SourceFile? Erroneous => erroneous;

    public ParseError WithErroneous(SourceFile? newErroneous)
    {
        return ReferenceEquals(newErroneous, erroneous)
            ? this
            : new ParseError(id, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, text,
                newErroneous);
    }

    public bool Equals(Tree? other)
    {
        return other is ParseError && other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree
    {
        return v.IsAdaptableTo(typeof(ParseErrorVisitor<P>));
    }

    R? Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)Accept(v.Adapt<Tree, ParseErrorVisitor<P>>(), p);
    }

    Tree Accept<P>(ParseErrorVisitor<P> v, P p)
    {
        return v.VisitParseError(this, p);
    }


    public ITreeVisitor<Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
    {
        return IPrinterFactory.Current()!.CreatePrinter<P>();
    }

    public static ParseError Build(IParser parser,
        IParser.Input input,
        string? relativeTo,
        IExecutionContext ctx,
        Exception t)
    {
        var stream = input.GetSource(ctx);
        using var readableStream = new StreamReader(stream, Encoding.UTF8);
        return new ParseError(
            Tree.RandomId(),
            new Markers(Tree.RandomId(), [ParseExceptionResult.Build(t)]),
            input.GetRelativePath(relativeTo),
            null, // FIXME: need real file attr
            parser.GetCharset(ctx),
            false, // stream.isCharsetBomMarked(),
            null,
            readableStream.ReadToEnd(),
            null
        );
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Parse error in {SourcePath}");
        var parseError = Markers.OfType<ParseExceptionResult>().FirstOrDefault();
        sb.AppendLine(parseError?.ToString());
        return sb.ToString();
    }
}
