using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteJson.Tree;

namespace Rewrite.RewriteJson;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
public class JsonVisitor<P> : TreeVisitor<Json, P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is Json;
    }

    public virtual Json? VisitArray(Json.Array array, P p)
    {
        array = array.WithPrefix(VisitSpace(array.Prefix, p)!);
        array = array.WithMarkers(VisitMarkers(array.Markers, p));
        array = array.Padding.WithValues(ListUtils.Map(array.Padding.Values, el => VisitRightPadded(el, p)));
        return array;
    }

    public virtual Json? VisitDocument(Json.Document document, P p)
    {
        document = document.WithPrefix(VisitSpace(document.Prefix, p)!);
        document = document.WithMarkers(VisitMarkers(document.Markers, p));
        document = document.WithValue(VisitAndCast<JsonValue>(document.Value, p)!);
        document = document.WithEof(VisitSpace(document.Eof, p)!);
        return document;
    }

    public virtual Json? VisitEmpty(Json.Empty empty, P p)
    {
        empty = empty.WithPrefix(VisitSpace(empty.Prefix, p)!);
        empty = empty.WithMarkers(VisitMarkers(empty.Markers, p));
        return empty;
    }

    public virtual Json? VisitIdentifier(Json.Identifier identifier, P p)
    {
        identifier = identifier.WithPrefix(VisitSpace(identifier.Prefix, p)!);
        identifier = identifier.WithMarkers(VisitMarkers(identifier.Markers, p));
        return identifier;
    }

    public virtual Json? VisitLiteral(Json.Literal literal, P p)
    {
        literal = literal.WithPrefix(VisitSpace(literal.Prefix, p)!);
        literal = literal.WithMarkers(VisitMarkers(literal.Markers, p));
        return literal;
    }

    public virtual Json? VisitMember(Json.Member member, P p)
    {
        member = member.WithPrefix(VisitSpace(member.Prefix, p)!);
        member = member.WithMarkers(VisitMarkers(member.Markers, p));
        member = member.Padding.WithKey(VisitRightPadded(member.Padding.Key, p)!);
        member = member.WithValue(VisitAndCast<JsonValue>(member.Value, p)!);
        return member;
    }

    public virtual Json? VisitObject(Json.JsonObject jsonObject, P p)
    {
        jsonObject = jsonObject.WithPrefix(VisitSpace(jsonObject.Prefix, p)!);
        jsonObject = jsonObject.WithMarkers(VisitMarkers(jsonObject.Markers, p));
        jsonObject = jsonObject.Padding.WithMembers(ListUtils.Map(jsonObject.Padding.Members, el => VisitRightPadded(el, p)));
        return jsonObject;
    }

    public virtual JsonRightPadded<T>? VisitRightPadded<T>(JsonRightPadded<T>? right, P p)
    where T : Json
    {
        if (right == null)
        {
            return null;
        }

        var t = right.Element;
        Cursor = new Cursor(Cursor, right);
        t = VisitAndCast<T>(t, p);
        Cursor = Cursor.Parent!;

        if (t == null)
        {
            return null;
        }

        right = right.WithElement(t);
        right = right.WithAfter(VisitSpace(right.After, p)!);
        right = right.WithMarkers(VisitMarkers(right.Markers, p));
        return right;
    }

    public virtual Space? VisitSpace(Space space, P p)
    {
        return space;
    }

}
