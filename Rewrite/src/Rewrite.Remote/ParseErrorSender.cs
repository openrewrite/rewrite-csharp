using System.Runtime.CompilerServices;
using Rewrite.Core;

namespace Rewrite.Remote;

public class ParseErrorSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Core.Tree {
        var visitor = new Visitor();
        visitor.Visit(after, ctx.Fork(visitor, before));
    }

    private class Visitor : ParseErrorVisitor<SenderContext>
    {

        public override ParseError Visit(Tree? tree, SenderContext ctx, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
            ctx.SendNode(tree, x => x, ctx.SendTree);
            Cursor = Cursor.Parent!;

            return (ParseError) tree;
        }

        public override ParseError VisitParseError(ParseError parseError, SenderContext ctx)
        {
            ctx.SendValue(parseError, v => v.Id);
            ctx.SendNode(parseError, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(parseError, v => v.SourcePath);
            ctx.SendTypedValue(parseError, v => v.FileAttributes);
            ctx.SendValue(parseError, v => v.CharsetName);
            ctx.SendValue(parseError, v => v.CharsetBomMarked);
            ctx.SendTypedValue(parseError, v => v.Checksum);
            ctx.SendValue(parseError, v => v.Text);
            // ctx.SendNode(parseError, v => v.Erroneous, ctx.SendTree);
            return parseError;
        }
    }
}
