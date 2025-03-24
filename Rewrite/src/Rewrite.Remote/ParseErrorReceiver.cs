using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Remote;

public class ParseErrorReceiver : Receiver
{
    public ReceiverContext Fork(ReceiverContext ctx)
    {
        return ctx.Fork(new Visitor(), new Factory());
    }

    public object Receive<T>(T? before, ReceiverContext ctx) where T : Tree
    {
        var forked = Fork(ctx);
        return forked.Visitor!.Visit(before, forked)!;
    }

    private class Visitor : ParseErrorVisitor<ReceiverContext>
    {

        public override ParseError? Visit(Tree? tree, ReceiverContext ctx)
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.ReceiveNode((ParseError?)tree, ctx.ReceiveTree);

            Cursor = Cursor.Parent!;
            return (ParseError?)tree;
        }

        public override ParseError VisitParseError(ParseError parseError, ReceiverContext ctx)
        {
            parseError = parseError.WithId(ctx.ReceiveValue(parseError.Id)!);
            parseError = parseError.WithMarkers(ctx.ReceiveNode(parseError.Markers, ctx.ReceiveMarkers)!);
            parseError = parseError.WithSourcePath(ctx.ReceiveValue(parseError.SourcePath)!);
            parseError = parseError.WithFileAttributes(ctx.ReceiveValue(parseError.FileAttributes));
            parseError = parseError.WithCharsetName(ctx.ReceiveValue(parseError.CharsetName));
            parseError = parseError.WithCharsetBomMarked(ctx.ReceiveValue(parseError.CharsetBomMarked));
            parseError = parseError.WithChecksum(ctx.ReceiveValue(parseError.Checksum));
            parseError = parseError.WithText(ctx.ReceiveValue(parseError.Text)!);
            // parseError = parseError.WithErroneous(ctx.ReceiveTree(parseError.Erroneous));
            return parseError;
        }
    }

    private class Factory : ReceiverFactory
    {
        public Tree Create<T>(string type, ReceiverContext ctx) where T : Tree
        {
            if (type is "Rewrite.Core.ParseError" or "org.openrewrite.tree.ParseError")
            {
                return new ParseError(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(FileAttributes)),
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(Checksum)),
                    ctx.ReceiveValue(default(string)) ?? "",
                    null // ctx.ReceiveTree(default(SourceFile))
                );
            }

            throw new NotImplementedException("No factory method for type: " + type);
        }
    }
}
