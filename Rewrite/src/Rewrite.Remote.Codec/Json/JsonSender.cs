using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteJson;

namespace Rewrite.Remote.Codec.Json;

using Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record JsonSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Core.Tree {
        var visitor = new Visitor();
        visitor.Visit(after, ctx.Fork(visitor, before));
    }

    private class Visitor : JsonVisitor<SenderContext>
    {
        public override Json Visit(Tree? tree, SenderContext ctx)
        {
            Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
            ctx.SendNode(tree, x => x, ctx.SendTree);
            Cursor = Cursor.Parent!;

            return (Json) tree;
        }

        public override Json VisitArray(Json.Array array, SenderContext ctx)
        {
            ctx.SendValue(array, v => v.Id);
            ctx.SendNode(array, v => v.Prefix, SendSpace);
            ctx.SendNode(array, v => v.Markers, ctx.SendMarkers);
            ctx.SendNodes(array, v => v.Padding.Values, SendRightPadded, t => t.Element.Id);
            return array;
        }

        public override Json VisitDocument(Json.Document document, SenderContext ctx)
        {
            ctx.SendValue(document, v => v.Id);
            ctx.SendValue(document, v => v.SourcePath);
            ctx.SendNode(document, v => v.Prefix, SendSpace);
            ctx.SendNode(document, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(document, v => v.CharsetName);
            ctx.SendValue(document, v => v.CharsetBomMarked);
            ctx.SendTypedValue(document, v => v.Checksum);
            ctx.SendTypedValue(document, v => v.FileAttributes);
            ctx.SendNode(document, v => v.Value, ctx.SendTree);
            ctx.SendNode(document, v => v.Eof, SendSpace);
            return document;
        }

        public override Json VisitEmpty(Json.Empty empty, SenderContext ctx)
        {
            ctx.SendValue(empty, v => v.Id);
            ctx.SendNode(empty, v => v.Prefix, SendSpace);
            ctx.SendNode(empty, v => v.Markers, ctx.SendMarkers);
            return empty;
        }

        public override Json VisitIdentifier(Json.Identifier identifier, SenderContext ctx)
        {
            ctx.SendValue(identifier, v => v.Id);
            ctx.SendNode(identifier, v => v.Prefix, SendSpace);
            ctx.SendNode(identifier, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(identifier, v => v.Name);
            return identifier;
        }

        public override Json VisitLiteral(Json.Literal literal, SenderContext ctx)
        {
            ctx.SendValue(literal, v => v.Id);
            ctx.SendNode(literal, v => v.Prefix, SendSpace);
            ctx.SendNode(literal, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(literal, v => v.Source);
            ctx.SendTypedValue(literal, v => v.Value);
            return literal;
        }

        public override Json VisitMember(Json.Member member, SenderContext ctx)
        {
            ctx.SendValue(member, v => v.Id);
            ctx.SendNode(member, v => v.Prefix, SendSpace);
            ctx.SendNode(member, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(member, v => v.Padding.Key, SendRightPadded);
            ctx.SendNode(member, v => v.Value, ctx.SendTree);
            return member;
        }

        public override Json VisitObject(Json.JsonObject jsonObject, SenderContext ctx)
        {
            ctx.SendValue(jsonObject, v => v.Id);
            ctx.SendNode(jsonObject, v => v.Prefix, SendSpace);
            ctx.SendNode(jsonObject, v => v.Markers, ctx.SendMarkers);
            ctx.SendNodes(jsonObject, v => v.Padding.Members, SendRightPadded, t => t.Element.Id);
            return jsonObject;
        }

        private static void SendComment(Comment comment, SenderContext ctx)
        {
            ctx.SendValue(comment, v => v.Multiline);
            ctx.SendValue(comment, v => v.Text);
            ctx.SendValue(comment, v => v.Suffix);
            ctx.SendNode(comment, v => v.Markers, ctx.SendMarkers);
        }

        private static void SendRightPadded<T>(JsonRightPadded<T> rightPadded, SenderContext ctx) where T : Json
        {
            ctx.SendNode(rightPadded, v => v.Element, ctx.SendTree);
            ctx.SendNode(rightPadded, v => v.After, SendSpace);
            ctx.SendNode(rightPadded, v => v.Markers, ctx.SendMarkers);
        }

        private static void SendSpace(Space space, SenderContext ctx)
        {
            ctx.SendNodes(space, v => v.Comments, SendComment, x => x);
            ctx.SendValue(space, v => v.Whitespace);
        }

    }
}
