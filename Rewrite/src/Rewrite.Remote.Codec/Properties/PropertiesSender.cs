using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteProperties;

namespace Rewrite.Remote.Codec.Properties;

using Rewrite.RewriteProperties.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record PropertiesSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Core.Tree {
        var visitor = new Visitor();
        visitor.Visit(after, ctx.Fork(visitor, before));
    }

    private class Visitor : PropertiesVisitor<SenderContext>
    {
        public override Properties Visit(Tree? tree, SenderContext ctx)
        {
            Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
            ctx.SendNode(tree, x => x, ctx.SendTree);
            Cursor = Cursor.Parent!;

            return (Properties) tree;
        }

        public override Properties VisitFile(Properties.File file, SenderContext ctx)
        {
            ctx.SendValue(file, v => v.Id);
            ctx.SendValue(file, v => v.Prefix);
            ctx.SendNode(file, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(file, v => v.SourcePath);
            ctx.SendNodes(file, v => v.Content, ctx.SendTree, t => t.Id);
            ctx.SendValue(file, v => v.Eof);
            ctx.SendValue(file, v => v.CharsetName);
            ctx.SendValue(file, v => v.CharsetBomMarked);
            ctx.SendTypedValue(file, v => v.FileAttributes);
            ctx.SendTypedValue(file, v => v.Checksum);
            return file;
        }

        public override Properties VisitEntry(Properties.Entry entry, SenderContext ctx)
        {
            ctx.SendValue(entry, v => v.Id);
            ctx.SendValue(entry, v => v.Prefix);
            ctx.SendNode(entry, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(entry, v => v.Key);
            ctx.SendValue(entry, v => v.BeforeEquals);
            ctx.SendValue(entry, v => v.Delim);
            ctx.SendTypedValue(entry, v => v.Value);
            return entry;
        }

        public override Properties.Value VisitValue(Properties.Value value, SenderContext ctx)
        {
            ctx.SendValue(value, v => v.Id);
            ctx.SendValue(value, v => v.Prefix);
            ctx.SendNode(value, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(value, v => v.Text);
            return value;
        }

        public override Properties VisitComment(Properties.Comment comment, SenderContext ctx)
        {
            ctx.SendValue(comment, v => v.Id);
            ctx.SendValue(comment, v => v.Prefix);
            ctx.SendNode(comment, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(comment, v => v.Delim);
            ctx.SendValue(comment, v => v.Message);
            return comment;
        }

    }
}
