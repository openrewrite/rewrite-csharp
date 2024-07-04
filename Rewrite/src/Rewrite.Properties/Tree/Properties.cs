using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteProperties.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public interface Properties : Rewrite.Core.Tree
{
    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(PropertiesVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptProperties(v.Adapt<Properties, PropertiesVisitor<P>>(), p);
    }

    Properties? AcceptProperties<P>(PropertiesVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

    public sealed class File(
        Guid id,
        string prefix,
        Markers markers,
        string sourcePath,
        IList<Content> content,
        string eof,
        string? charsetName,
        bool charsetBomMarked,
        FileAttributes? fileAttributes,
        Checksum? checksum
    ) : Properties, MutableSourceFile<File>, MutableTree<File>
    {
        public Properties? AcceptProperties<P>(PropertiesVisitor<P> v, P p)
        {
            return v.VisitFile(this, p);
        }

        public Guid Id => id;

        public File WithId(Guid newId)
        {
            return newId == id ? this : new File(newId, prefix, markers, sourcePath, content, eof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public string Prefix => prefix;

        public File WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new File(id, newPrefix, markers, sourcePath, content, eof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public Markers Markers => markers;

        public File WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new File(id, prefix, newMarkers, sourcePath, content, eof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public string SourcePath => sourcePath;

        public File WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new File(id, prefix, markers, newSourcePath, content, eof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public IList<Properties.Content> Content => content;

        public File WithContent(IList<Properties.Content> newContent)
        {
            return newContent == content ? this : new File(id, prefix, markers, sourcePath, newContent, eof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public string Eof => eof;

        public File WithEof(string newEof)
        {
            return newEof == eof ? this : new File(id, prefix, markers, sourcePath, content, newEof, charsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public string? CharsetName => charsetName;

        public File WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new File(id, prefix, markers, sourcePath, content, eof, newCharsetName, charsetBomMarked, fileAttributes, checksum);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public File WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new File(id, prefix, markers, sourcePath, content, eof, charsetName, newCharsetBomMarked, fileAttributes, checksum);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public File WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new File(id, prefix, markers, sourcePath, content, eof, charsetName, charsetBomMarked, newFileAttributes, checksum);
        }

        public Checksum? Checksum => checksum;

        public File WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new File(id, prefix, markers, sourcePath, content, eof, charsetName, charsetBomMarked, fileAttributes, newChecksum);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is File && other.Id == Id;
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

    public interface Content : Properties
    {
    }

    public sealed class Entry(
        Guid id,
        string prefix,
        Markers markers,
        string key,
        string beforeEquals,
        Entry.Delimiter? delim,
        Value value
    ) : Properties.Content, MutableTree<Entry>
    {
        public Properties? AcceptProperties<P>(PropertiesVisitor<P> v, P p)
        {
            return v.VisitEntry(this, p);
        }

        public Guid Id => id;

        public Entry WithId(Guid newId)
        {
            return newId == id ? this : new Entry(newId, prefix, markers, key, beforeEquals, delim, value);
        }

        public string Prefix => prefix;

        public Entry WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Entry(id, newPrefix, markers, key, beforeEquals, delim, value);
        }

        public Markers Markers => markers;

        public Entry WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Entry(id, prefix, newMarkers, key, beforeEquals, delim, value);
        }

        public string Key => key;

        public Entry WithKey(string newKey)
        {
            return newKey == key ? this : new Entry(id, prefix, markers, newKey, beforeEquals, delim, value);
        }

        public string BeforeEquals => beforeEquals;

        public Entry WithBeforeEquals(string newBeforeEquals)
        {
            return newBeforeEquals == beforeEquals ? this : new Entry(id, prefix, markers, key, newBeforeEquals, delim, value);
        }

        public Delimiter? Delim => delim;

        public Entry WithDelim(Delimiter? newDelim)
        {
            return newDelim == delim ? this : new Entry(id, prefix, markers, key, beforeEquals, newDelim, value);
        }

        public Properties.Value Value => value;

        public Entry WithValue(Properties.Value newValue)
        {
            return ReferenceEquals(newValue, value) ? this : new Entry(id, prefix, markers, key, beforeEquals, delim, newValue);
        }

        public enum Delimiter
        {
            COLON,
            EQUALS,
            NONE,

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Entry && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Value(
        Guid id,
        string prefix,
        Markers markers,
        string text
    )
    {
        public Properties.Value AcceptProperties<P>(PropertiesVisitor<P> v, P p)
        {
            return v.VisitValue(this, p);
        }

        public Guid Id => id;

        public Value WithId(Guid newId)
        {
            return newId == id ? this : new Value(newId, prefix, markers, text);
        }

        public string Prefix => prefix;

        public Value WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Value(id, newPrefix, markers, text);
        }

        public Markers Markers => markers;

        public Value WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Value(id, prefix, newMarkers, text);
        }

        public string Text => text;

        public Value WithText(string newText)
        {
            return newText == text ? this : new Value(id, prefix, markers, newText);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Value && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed class Comment(
        Guid id,
        string prefix,
        Markers markers,
        Comment.Delimiter delim,
        string message
    ) : Properties.Content, MutableTree<Comment>
    {
        public Properties? AcceptProperties<P>(PropertiesVisitor<P> v, P p)
        {
            return v.VisitComment(this, p);
        }

        public Guid Id => id;

        public Comment WithId(Guid newId)
        {
            return newId == id ? this : new Comment(newId, prefix, markers, delim, message);
        }

        public string Prefix => prefix;

        public Comment WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Comment(id, newPrefix, markers, delim, message);
        }

        public Markers Markers => markers;

        public Comment WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Comment(id, prefix, newMarkers, delim, message);
        }

        public Delimiter Delim => delim;

        public Comment WithDelim(Delimiter newDelim)
        {
            return newDelim == delim ? this : new Comment(id, prefix, markers, newDelim, message);
        }

        public string Message => message;

        public Comment WithMessage(string newMessage)
        {
            return newMessage == message ? this : new Comment(id, prefix, markers, delim, newMessage);
        }

        public enum Delimiter
        {
            HASH_TAG,
            EXCLAMATION_MARK,

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Comment && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed record Continuation(

    )
    {
    }

}
