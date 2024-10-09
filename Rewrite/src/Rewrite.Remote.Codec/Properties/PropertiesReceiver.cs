using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteProperties;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Remote.Codec.Properties;

using Rewrite.RewriteProperties.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression")]
public record PropertiesReceiver : Receiver
{
    public ReceiverContext Fork(ReceiverContext ctx)
    {
        return ctx.Fork(new Visitor(), new Factory());
    }

    public object Receive<T>(T? before, ReceiverContext ctx) where T : Core.Tree
    {
        var forked = Fork(ctx);
        return forked.Visitor!.Visit(before, forked)!;
    }

    private class Visitor : PropertiesVisitor<ReceiverContext>
    {
        public override Properties? Visit(Tree? tree, ReceiverContext ctx)
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.ReceiveNode((Properties?)tree, ctx.ReceiveTree);

            Cursor = Cursor.Parent!;
            return (Properties?)tree;
        }

        public override Properties VisitFile(Properties.File file, ReceiverContext ctx)
        {
            file = file.WithId(ctx.ReceiveValue(file.Id)!);
            file = file.WithPrefix(ctx.ReceiveValue(file.Prefix)!);
            file = file.WithMarkers(ctx.ReceiveNode(file.Markers, ctx.ReceiveMarkers)!);
            file = file.WithSourcePath(ctx.ReceiveValue(file.SourcePath)!);
            file = file.WithContent(ctx.ReceiveNodes(file.Content, ctx.ReceiveTree)!);
            file = file.WithEof(ctx.ReceiveValue(file.Eof)!);
            file = file.WithCharsetName(ctx.ReceiveValue(file.CharsetName));
            file = file.WithCharsetBomMarked(ctx.ReceiveValue(file.CharsetBomMarked));
            file = file.WithFileAttributes(ctx.ReceiveValue(file.FileAttributes));
            file = file.WithChecksum(ctx.ReceiveValue(file.Checksum));
            return file;
        }

        public override Properties VisitEntry(Properties.Entry entry, ReceiverContext ctx)
        {
            entry = entry.WithId(ctx.ReceiveValue(entry.Id)!);
            entry = entry.WithPrefix(ctx.ReceiveValue(entry.Prefix)!);
            entry = entry.WithMarkers(ctx.ReceiveNode(entry.Markers, ctx.ReceiveMarkers)!);
            entry = entry.WithKey(ctx.ReceiveValue(entry.Key)!);
            entry = entry.WithBeforeEquals(ctx.ReceiveValue(entry.BeforeEquals)!);
            entry = entry.WithDelim(ctx.ReceiveValue(entry.Delim));
            entry = entry.WithValue(ctx.ReceiveValue(entry.Value)!);
            return entry;
        }

        public override Properties.Value VisitValue(Properties.Value value, ReceiverContext ctx)
        {
            value = value.WithId(ctx.ReceiveValue(value.Id)!);
            value = value.WithPrefix(ctx.ReceiveValue(value.Prefix)!);
            value = value.WithMarkers(ctx.ReceiveNode(value.Markers, ctx.ReceiveMarkers)!);
            value = value.WithText(ctx.ReceiveValue(value.Text)!);
            return value;
        }

        public override Properties VisitComment(Properties.Comment comment, ReceiverContext ctx)
        {
            comment = comment.WithId(ctx.ReceiveValue(comment.Id)!);
            comment = comment.WithPrefix(ctx.ReceiveValue(comment.Prefix)!);
            comment = comment.WithMarkers(ctx.ReceiveNode(comment.Markers, ctx.ReceiveMarkers)!);
            comment = comment.WithDelim(ctx.ReceiveValue(comment.Delim)!);
            comment = comment.WithMessage(ctx.ReceiveValue(comment.Message)!);
            return comment;
        }

    }

    private class Factory : ReceiverFactory
    {
        public Rewrite.Core.Tree Create<T>(string type, ReceiverContext ctx) where T : Rewrite.Core.Tree
        {
            if (type is "Rewrite.RewriteProperties.Tree.Properties.File" or "org.openrewrite.properties.tree.Properties$File")
            {
                return new Properties.File(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNodes(default(IList<Properties.Content>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(FileAttributes)),
                    ctx.ReceiveValue(default(Checksum))
                );
            }

            if (type is "Rewrite.RewriteProperties.Tree.Properties.Entry" or "org.openrewrite.properties.tree.Properties$Entry")
            {
                return new Properties.Entry(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(Properties.Entry.Delimiter)),
                    ctx.ReceiveValue(default(Properties.Value))!
                );
            }

            if (type is "Rewrite.RewriteProperties.Tree.Properties.Comment" or "org.openrewrite.properties.tree.Properties$Comment")
            {
                return new Properties.Comment(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(Properties.Comment.Delimiter))!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            throw new NotImplementedException("No factory method for type: " + type);
        }
    }
}
