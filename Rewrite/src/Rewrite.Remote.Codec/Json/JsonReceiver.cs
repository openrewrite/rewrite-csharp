using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteJson;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Remote.Codec.Json;

using Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression")]
public record JsonReceiver : Receiver
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

    private class Visitor : JsonVisitor<ReceiverContext>
    {
        public override Json? Visit(Tree? tree, ReceiverContext ctx)
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.ReceiveNode((Json?)tree, ctx.ReceiveTree);

            Cursor = Cursor.Parent!;
            return (Json?)tree;
        }

        public override Json VisitArray(Json.Array array, ReceiverContext ctx)
        {
            array = array.WithId(ctx.ReceiveValue(array.Id)!);
            array = array.WithPrefix(ctx.ReceiveNode(array.Prefix, ReceiveSpace)!);
            array = array.WithMarkers(ctx.ReceiveNode(array.Markers, ctx.ReceiveMarkers)!);
            array = array.Padding.WithValues(ctx.ReceiveNodes(array.Padding.Values, ReceiveRightPadded)!);
            return array;
        }

        public override Json VisitDocument(Json.Document document, ReceiverContext ctx)
        {
            document = document.WithId(ctx.ReceiveValue(document.Id)!);
            document = document.WithSourcePath(ctx.ReceiveValue(document.SourcePath)!);
            document = document.WithPrefix(ctx.ReceiveNode(document.Prefix, ReceiveSpace)!);
            document = document.WithMarkers(ctx.ReceiveNode(document.Markers, ctx.ReceiveMarkers)!);
            document = document.WithCharsetName(ctx.ReceiveValue(document.CharsetName));
            document = document.WithCharsetBomMarked(ctx.ReceiveValue(document.CharsetBomMarked));
            document = document.WithChecksum(ctx.ReceiveValue(document.Checksum));
            document = document.WithFileAttributes(ctx.ReceiveValue(document.FileAttributes));
            document = document.WithValue(ctx.ReceiveNode(document.Value, ctx.ReceiveTree)!);
            document = document.WithEof(ctx.ReceiveNode(document.Eof, ReceiveSpace)!);
            return document;
        }

        public override Json VisitEmpty(Json.Empty empty, ReceiverContext ctx)
        {
            empty = empty.WithId(ctx.ReceiveValue(empty.Id)!);
            empty = empty.WithPrefix(ctx.ReceiveNode(empty.Prefix, ReceiveSpace)!);
            empty = empty.WithMarkers(ctx.ReceiveNode(empty.Markers, ctx.ReceiveMarkers)!);
            return empty;
        }

        public override Json VisitIdentifier(Json.Identifier identifier, ReceiverContext ctx)
        {
            identifier = identifier.WithId(ctx.ReceiveValue(identifier.Id)!);
            identifier = identifier.WithPrefix(ctx.ReceiveNode(identifier.Prefix, ReceiveSpace)!);
            identifier = identifier.WithMarkers(ctx.ReceiveNode(identifier.Markers, ctx.ReceiveMarkers)!);
            identifier = identifier.WithName(ctx.ReceiveValue(identifier.Name)!);
            return identifier;
        }

        public override Json VisitLiteral(Json.Literal literal, ReceiverContext ctx)
        {
            literal = literal.WithId(ctx.ReceiveValue(literal.Id)!);
            literal = literal.WithPrefix(ctx.ReceiveNode(literal.Prefix, ReceiveSpace)!);
            literal = literal.WithMarkers(ctx.ReceiveNode(literal.Markers, ctx.ReceiveMarkers)!);
            literal = literal.WithSource(ctx.ReceiveValue(literal.Source)!);
            literal = literal.WithValue(ctx.ReceiveValue(literal.Value)!);
            return literal;
        }

        public override Json VisitMember(Json.Member member, ReceiverContext ctx)
        {
            member = member.WithId(ctx.ReceiveValue(member.Id)!);
            member = member.WithPrefix(ctx.ReceiveNode(member.Prefix, ReceiveSpace)!);
            member = member.WithMarkers(ctx.ReceiveNode(member.Markers, ctx.ReceiveMarkers)!);
            member = member.Padding.WithKey(ctx.ReceiveNode(member.Padding.Key, ReceiveRightPadded)!);
            member = member.WithValue(ctx.ReceiveNode(member.Value, ctx.ReceiveTree)!);
            return member;
        }

        public override Json VisitObject(Json.JsonObject jsonObject, ReceiverContext ctx)
        {
            jsonObject = jsonObject.WithId(ctx.ReceiveValue(jsonObject.Id)!);
            jsonObject = jsonObject.WithPrefix(ctx.ReceiveNode(jsonObject.Prefix, ReceiveSpace)!);
            jsonObject = jsonObject.WithMarkers(ctx.ReceiveNode(jsonObject.Markers, ctx.ReceiveMarkers)!);
            jsonObject = jsonObject.Padding.WithMembers(ctx.ReceiveNodes(jsonObject.Padding.Members, ReceiveRightPadded)!);
            return jsonObject;
        }

    }

    private class Factory : ReceiverFactory
    {
        public Rewrite.Core.Tree Create<T>(string type, ReceiverContext ctx) where T : Rewrite.Core.Tree
        {
            if (type is "Rewrite.RewriteJson.Tree.Json.Array" or "org.openrewrite.json.tree.Json$Array")
            {
                return new Json.Array(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNodes(default(IList<JsonRightPadded<JsonValue>>), ReceiveRightPadded)!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.Document" or "org.openrewrite.json.tree.Json$Document")
            {
                return new Json.Document(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(Checksum)),
                    ctx.ReceiveValue(default(FileAttributes)),
                    ctx.ReceiveNode(default(JsonValue), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.Empty" or "org.openrewrite.json.tree.Json$Empty")
            {
                return new Json.Empty(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.Identifier" or "org.openrewrite.json.tree.Json$Identifier")
            {
                return new Json.Identifier(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.Literal" or "org.openrewrite.json.tree.Json$Literal")
            {
                return new Json.Literal(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(object))!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.Member" or "org.openrewrite.json.tree.Json$Member")
            {
                return new Json.Member(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(JsonRightPadded<JsonKey>), ReceiveRightPadded)!,
                    ctx.ReceiveNode(default(JsonValue), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteJson.Tree.Json.JsonObject" or "org.openrewrite.json.tree.Json$JsonObject")
            {
                return new Json.JsonObject(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNodes(default(IList<JsonRightPadded<Json>>), ReceiveRightPadded)!
                );
            }

            throw new NotImplementedException("No factory method for type: " + type);
        }
    }

    private static Comment ReceiveComment(Comment? comment, string? type, ReceiverContext ctx)
    {
        if (comment != null) {
            comment = comment.WithMultiline(ctx.ReceiveValue(comment.Multiline));
            comment = comment.WithText(ctx.ReceiveValue(comment.Text)!);
            comment = comment.WithSuffix(ctx.ReceiveValue(comment.Suffix)!);
            comment = comment.WithMarkers(ctx.ReceiveNode(comment.Markers, ctx.ReceiveMarkers)!);
        } else {
            comment = new Comment(
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!
            );
        }
        return comment;
    }

    private static JsonRightPadded<T> ReceiveRightPadded<T>(JsonRightPadded<T>? right, string? type, ReceiverContext ctx)
    where T : class, Json
    {
        if (right != null) {
            right = right.WithElement(ctx.ReceiveNode(right.Element, ctx.ReceiveTree)!);
            right = right.WithAfter(ctx.ReceiveNode(right.After, ReceiveSpace)!);
            right = right.WithMarkers(ctx.ReceiveNode(right.Markers, ctx.ReceiveMarkers)!);
        } else {
            right = new JsonRightPadded<T>(
                    ctx.ReceiveNode(default(T), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Space), ReceiveSpace)!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!
            );
        }
        return right;
    }

    private static Space ReceiveSpace(Space? space, string? type, ReceiverContext ctx)
    {
        if (space != null) {
            space = space.WithComments(ctx.ReceiveNodes(space.Comments, ReceiveComment)!);
            space = space.WithWhitespace(ctx.ReceiveValue(space.Whitespace));
        } else {
            return new Space(
                    ctx.ReceiveNodes(default(IList<Comment>), ReceiveComment)!,
                    ctx.ReceiveValue(default(string))
            );
        }
        return space;
    }

}
