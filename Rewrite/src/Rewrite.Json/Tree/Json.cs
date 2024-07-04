using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public interface Json : Rewrite.Core.Tree
{
    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(JsonVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptJson(v.Adapt<Json, JsonVisitor<P>>(), p);
    }

    Json? AcceptJson<P>(JsonVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

    public class Array(
        Guid id,
        Space prefix,
        Markers markers,
        IList<JsonRightPadded<JsonValue>> values
    ) : JsonValue, MutableTree<Array>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitArray(this, p);
        }

        public Guid Id => id;

        public Array WithId(Guid newId)
        {
            return newId == id ? this : new Array(newId, prefix, markers, _values);
        }

        public Space Prefix => prefix;

        public Array WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Array(id, newPrefix, markers, _values);
        }

        public Markers Markers => markers;

        public Array WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Array(id, prefix, newMarkers, _values);
        }

        private readonly IList<JsonRightPadded<JsonValue>> _values = values;
        public IList<JsonValue> Values => _values.Elements();

        public Array WithValues(IList<JsonValue> newValues)
        {
            return Padding.WithValues(_values.WithElements(newValues));
        }

        public sealed record PaddingHelper(Json.Array T)
        {
            public IList<JsonRightPadded<JsonValue>> Values => T._values;

            public Json.Array WithValues(IList<JsonRightPadded<JsonValue>> newValues)
            {
                return T._values == newValues ? T : new Json.Array(T.Id, T.Prefix, T.Markers, newValues);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Array && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Document(
        Guid id,
        string sourcePath,
        Space prefix,
        Markers markers,
        string? charsetName,
        bool charsetBomMarked,
        Checksum? checksum,
        FileAttributes? fileAttributes,
        JsonValue value,
        Space eof
    ) : Json, MutableSourceFile<Document>, MutableTree<Document>
    {
        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitDocument(this, p);
        }

        public Guid Id => id;

        public Document WithId(Guid newId)
        {
            return newId == id ? this : new Document(newId, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public string SourcePath => sourcePath;

        public Document WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new Document(id, newSourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public Space Prefix => prefix;

        public Document WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Document(id, sourcePath, newPrefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public Markers Markers => markers;

        public Document WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Document(id, sourcePath, prefix, newMarkers, charsetName, charsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public string? CharsetName => charsetName;

        public Document WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new Document(id, sourcePath, prefix, markers, newCharsetName, charsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public Document WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new Document(id, sourcePath, prefix, markers, charsetName, newCharsetBomMarked, checksum, fileAttributes, value, eof);
        }

        public Checksum? Checksum => checksum;

        public Document WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, newChecksum, fileAttributes, value, eof);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public Document WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, newFileAttributes, value, eof);
        }

        public JsonValue Value => value;

        public Document WithValue(JsonValue newValue)
        {
            return ReferenceEquals(newValue, value) ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, newValue, eof);
        }

        public Space Eof => eof;

        public Document WithEof(Space newEof)
        {
            return newEof == eof ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, value, newEof);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Document && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public ITreeVisitor<Core.Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
        {
            return new RemotePrinter<P>();
        }
    }

    public sealed class Empty(
        Guid id,
        Space prefix,
        Markers markers
    ) : JsonValue, MutableTree<Empty>
    {
        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitEmpty(this, p);
        }

        public Guid Id => id;

        public Empty WithId(Guid newId)
        {
            return newId == id ? this : new Empty(newId, prefix, markers);
        }

        public Space Prefix => prefix;

        public Empty WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Empty(id, newPrefix, markers);
        }

        public Markers Markers => markers;

        public Empty WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Empty(id, prefix, newMarkers);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Empty && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Identifier(
        Guid id,
        Space prefix,
        Markers markers,
        string name
    ) : JsonKey, MutableTree<Identifier>
    {
        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitIdentifier(this, p);
        }

        public Guid Id => id;

        public Identifier WithId(Guid newId)
        {
            return newId == id ? this : new Identifier(newId, prefix, markers, name);
        }

        public Space Prefix => prefix;

        public Identifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Identifier(id, newPrefix, markers, name);
        }

        public Markers Markers => markers;

        public Identifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Identifier(id, prefix, newMarkers, name);
        }

        public string Name => name;

        public Identifier WithName(string newName)
        {
            return newName == name ? this : new Identifier(id, prefix, markers, newName);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Identifier && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Literal(
        Guid id,
        Space prefix,
        Markers markers,
        string source,
        object value
    ) : JsonValue, JsonKey, MutableTree<Literal>
    {
        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitLiteral(this, p);
        }

        public Guid Id => id;

        public Literal WithId(Guid newId)
        {
            return newId == id ? this : new Literal(newId, prefix, markers, source, value);
        }

        public Space Prefix => prefix;

        public Literal WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Literal(id, newPrefix, markers, source, value);
        }

        public Markers Markers => markers;

        public Literal WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Literal(id, prefix, newMarkers, source, value);
        }

        public string Source => source;

        public Literal WithSource(string newSource)
        {
            return newSource == source ? this : new Literal(id, prefix, markers, newSource, value);
        }

        public object Value => value;

        public Literal WithValue(object newValue)
        {
            return newValue == value ? this : new Literal(id, prefix, markers, source, newValue);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Literal && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Member(
        Guid id,
        Space prefix,
        Markers markers,
        JsonRightPadded<JsonKey> key,
        JsonValue value
    ) : Json, MutableTree<Member>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitMember(this, p);
        }

        public Guid Id => id;

        public Member WithId(Guid newId)
        {
            return newId == id ? this : new Member(newId, prefix, markers, _key, value);
        }

        public Space Prefix => prefix;

        public Member WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Member(id, newPrefix, markers, _key, value);
        }

        public Markers Markers => markers;

        public Member WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Member(id, prefix, newMarkers, _key, value);
        }

        private readonly JsonRightPadded<JsonKey> _key = key;
        public JsonKey Key => _key.Element;

        public Member WithKey(JsonKey newKey)
        {
            return Padding.WithKey(_key.WithElement(newKey));
        }

        public JsonValue Value => value;

        public Member WithValue(JsonValue newValue)
        {
            return ReferenceEquals(newValue, value) ? this : new Member(id, prefix, markers, _key, newValue);
        }

        public sealed record PaddingHelper(Json.Member T)
        {
            public JsonRightPadded<JsonKey> Key => T._key;

            public Json.Member WithKey(JsonRightPadded<JsonKey> newKey)
            {
                return T._key == newKey ? T : new Json.Member(T.Id, T.Prefix, T.Markers, newKey, T.Value);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Member && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class JsonObject(
        Guid id,
        Space prefix,
        Markers markers,
        IList<JsonRightPadded<Json>> members
    ) : JsonValue, MutableTree<JsonObject>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public Json? AcceptJson<P>(JsonVisitor<P> v, P p)
        {
            return v.VisitObject(this, p);
        }

        public Guid Id => id;

        public JsonObject WithId(Guid newId)
        {
            return newId == id ? this : new JsonObject(newId, prefix, markers, _members);
        }

        public Space Prefix => prefix;

        public JsonObject WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new JsonObject(id, newPrefix, markers, _members);
        }

        public Markers Markers => markers;

        public JsonObject WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new JsonObject(id, prefix, newMarkers, _members);
        }

        private readonly IList<JsonRightPadded<Json>> _members = members;
        public IList<Json> Members => _members.Elements();

        public JsonObject WithMembers(IList<Json> newMembers)
        {
            return Padding.WithMembers(_members.WithElements(newMembers));
        }

        public sealed record PaddingHelper(Json.JsonObject T)
        {
            public IList<JsonRightPadded<Json>> Members => T._members;

            public Json.JsonObject WithMembers(IList<JsonRightPadded<Json>> newMembers)
            {
                return T._members == newMembers ? T : new Json.JsonObject(T.Id, T.Prefix, T.Markers, newMembers);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is JsonObject && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
