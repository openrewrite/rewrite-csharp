using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteYaml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Yaml : Rewrite.Core.Tree
{
    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(YamlVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptYaml(v.Adapt<Yaml, YamlVisitor<P>>(), p);
    }

    Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

    public sealed partial class Documents(
        Guid id,
        Markers markers,
        string sourcePath,
        FileAttributes? fileAttributes,
        string? charsetName,
        bool charsetBomMarked,
        Checksum? checksum,
        IList<Document> docs
    ) : Yaml,MutableSourceFile<Documents>    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitDocuments(this, p);
        }

        public Guid Id => id;

        public Documents WithId(Guid newId)
        {
            return newId == id ? this : new Documents(newId, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, docs);
        }

        public Markers Markers => markers;

        public Documents WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Documents(id, newMarkers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, docs);
        }

        public string SourcePath => sourcePath;

        public Documents WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new Documents(id, markers, newSourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, docs);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public Documents WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new Documents(id, markers, sourcePath, newFileAttributes, charsetName, charsetBomMarked, checksum, docs);
        }

        public string? CharsetName => charsetName;

        public Documents WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new Documents(id, markers, sourcePath, fileAttributes, newCharsetName, charsetBomMarked, checksum, docs);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public Documents WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new Documents(id, markers, sourcePath, fileAttributes, charsetName, newCharsetBomMarked, checksum, docs);
        }

        public Checksum? Checksum => checksum;

        public Documents WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new Documents(id, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, newChecksum, docs);
        }

        public IList<Yaml.Document> Docs => docs;

        public Documents WithDocs(IList<Yaml.Document> newDocs)
        {
            return newDocs == docs ? this : new Documents(id, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, newDocs);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Documents && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public ITreeVisitor<Core.Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
        {
            return IPrinterFactory.Current()!.CreatePrinter<P>();
        }
    }

    public sealed partial class Document(
        Guid id,
        string prefix,
        Markers markers,
        bool @explicit,
        Block block,
        Document.End ending
    ) : Yaml    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitDocument(this, p);
        }

        public Guid Id => id;

        public Document WithId(Guid newId)
        {
            return newId == id ? this : new Document(newId, prefix, markers, @explicit, block, ending);
        }

        public string Prefix => prefix;

        public Document WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Document(id, newPrefix, markers, @explicit, block, ending);
        }

        public Markers Markers => markers;

        public Document WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Document(id, prefix, newMarkers, @explicit, block, ending);
        }

        public bool Explicit => @explicit;

        public Document WithExplicit(bool newExplicit)
        {
            return newExplicit == @explicit ? this : new Document(id, prefix, markers, newExplicit, block, ending);
        }

        public Yaml.Block Block => block;

        public Document WithBlock(Yaml.Block newBlock)
        {
            return ReferenceEquals(newBlock, block) ? this : new Document(id, prefix, markers, @explicit, newBlock, ending);
        }

        public End Ending => ending;

        public Document WithEnding(End newEnding)
        {
            return ReferenceEquals(newEnding, ending) ? this : new Document(id, prefix, markers, @explicit, block, newEnding);
        }

        public sealed partial class End(
            Guid id,
            string prefix,
            Markers markers,
            bool @explicit
        ) : Yaml
        {
            public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
            {
                return v.VisitDocumentEnd(this, p);
            }

            public Guid Id => id;

            public End WithId(Guid newId)
            {
                return newId == id ? this : new End(newId, prefix, markers, @explicit);
            }

            public string Prefix => prefix;

            public End WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new End(id, newPrefix, markers, @explicit);
            }

            public Markers Markers => markers;

            public End WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new End(id, prefix, newMarkers, @explicit);
            }

            public bool Explicit => @explicit;

            public End WithExplicit(bool newExplicit)
            {
                return newExplicit == @explicit ? this : new End(id, prefix, markers, newExplicit);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is End && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Document && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial interface Block : Yaml
    {
    }

    public sealed partial class Scalar(
        Guid id,
        string prefix,
        Markers markers,
        Scalar.Style scalarStyle,
        Anchor? anchor,
        string value
    ) : Yaml.Block,YamlKey    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitScalar(this, p);
        }

        public Guid Id => id;

        public Scalar WithId(Guid newId)
        {
            return newId == id ? this : new Scalar(newId, prefix, markers, scalarStyle, anchor, value);
        }

        public string Prefix => prefix;

        public Scalar WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Scalar(id, newPrefix, markers, scalarStyle, anchor, value);
        }

        public Markers Markers => markers;

        public Scalar WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Scalar(id, prefix, newMarkers, scalarStyle, anchor, value);
        }

        public Style ScalarStyle => scalarStyle;

        public Scalar WithScalarStyle(Style newScalarStyle)
        {
            return newScalarStyle == scalarStyle ? this : new Scalar(id, prefix, markers, newScalarStyle, anchor, value);
        }

        public Yaml.Anchor? Anchor => anchor;

        public Scalar WithAnchor(Yaml.Anchor? newAnchor)
        {
            return ReferenceEquals(newAnchor, anchor) ? this : new Scalar(id, prefix, markers, scalarStyle, newAnchor, value);
        }

        public string Value => value;

        public Scalar WithValue(string newValue)
        {
            return newValue == value ? this : new Scalar(id, prefix, markers, scalarStyle, anchor, newValue);
        }

        public enum Style
        {
            DOUBLE_QUOTED,
            SINGLE_QUOTED,
            LITERAL,
            FOLDED,
            PLAIN,

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Scalar && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial class Mapping(
        Guid id,
        Markers markers,
        string? openingBracePrefix,
        IList<Mapping.Entry> entries,
        string? closingBracePrefix,
        Anchor? anchor
    ) : Yaml.Block    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitMapping(this, p);
        }

        public Guid Id => id;

        public Mapping WithId(Guid newId)
        {
            return newId == id ? this : new Mapping(newId, markers, openingBracePrefix, entries, closingBracePrefix, anchor);
        }

        public Markers Markers => markers;

        public Mapping WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Mapping(id, newMarkers, openingBracePrefix, entries, closingBracePrefix, anchor);
        }

        public string? OpeningBracePrefix => openingBracePrefix;

        public Mapping WithOpeningBracePrefix(string? newOpeningBracePrefix)
        {
            return newOpeningBracePrefix == openingBracePrefix ? this : new Mapping(id, markers, newOpeningBracePrefix, entries, closingBracePrefix, anchor);
        }

        public IList<Entry> Entries => entries;

        public Mapping WithEntries(IList<Entry> newEntries)
        {
            return newEntries == entries ? this : new Mapping(id, markers, openingBracePrefix, newEntries, closingBracePrefix, anchor);
        }

        public string? ClosingBracePrefix => closingBracePrefix;

        public Mapping WithClosingBracePrefix(string? newClosingBracePrefix)
        {
            return newClosingBracePrefix == closingBracePrefix ? this : new Mapping(id, markers, openingBracePrefix, entries, newClosingBracePrefix, anchor);
        }

        public Yaml.Anchor? Anchor => anchor;

        public Mapping WithAnchor(Yaml.Anchor? newAnchor)
        {
            return ReferenceEquals(newAnchor, anchor) ? this : new Mapping(id, markers, openingBracePrefix, entries, closingBracePrefix, newAnchor);
        }

        public sealed partial class Entry(
            Guid id,
            string prefix,
            Markers markers,
            YamlKey key,
            string beforeMappingValueIndicator,
            Yaml.Block value
        ) : Yaml, MutableTree
        {
            public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
            {
                return v.VisitMappingEntry(this, p);
            }

            public Guid Id => id;

            public Entry WithId(Guid newId)
            {
                return newId == id ? this : new Entry(newId, prefix, markers, key, beforeMappingValueIndicator, value);
            }

            public string Prefix => prefix;

            public Entry WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new Entry(id, newPrefix, markers, key, beforeMappingValueIndicator, value);
            }

            public Markers Markers => markers;

            public Entry WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Entry(id, prefix, newMarkers, key, beforeMappingValueIndicator, value);
            }

            public YamlKey Key => key;

            public Entry WithKey(YamlKey newKey)
            {
                return ReferenceEquals(newKey, key) ? this : new Entry(id, prefix, markers, newKey, beforeMappingValueIndicator, value);
            }

            public string BeforeMappingValueIndicator => beforeMappingValueIndicator;

            public Entry WithBeforeMappingValueIndicator(string newBeforeMappingValueIndicator)
            {
                return newBeforeMappingValueIndicator == beforeMappingValueIndicator ? this : new Entry(id, prefix, markers, key, newBeforeMappingValueIndicator, value);
            }

            public Yaml.Block Value => value;

            public Entry WithValue(Yaml.Block newValue)
            {
                return ReferenceEquals(newValue, value) ? this : new Entry(id, prefix, markers, key, beforeMappingValueIndicator, newValue);
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Mapping && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial class Sequence(
        Guid id,
        Markers markers,
        string? openingBracketPrefix,
        IList<Sequence.Entry> entries,
        string? closingBracketPrefix,
        Anchor? anchor
    ) : Yaml.Block    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitSequence(this, p);
        }

        public Guid Id => id;

        public Sequence WithId(Guid newId)
        {
            return newId == id ? this : new Sequence(newId, markers, openingBracketPrefix, entries, closingBracketPrefix, anchor);
        }

        public Markers Markers => markers;

        public Sequence WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Sequence(id, newMarkers, openingBracketPrefix, entries, closingBracketPrefix, anchor);
        }

        public string? OpeningBracketPrefix => openingBracketPrefix;

        public Sequence WithOpeningBracketPrefix(string? newOpeningBracketPrefix)
        {
            return newOpeningBracketPrefix == openingBracketPrefix ? this : new Sequence(id, markers, newOpeningBracketPrefix, entries, closingBracketPrefix, anchor);
        }

        public IList<Entry> Entries => entries;

        public Sequence WithEntries(IList<Entry> newEntries)
        {
            return newEntries == entries ? this : new Sequence(id, markers, openingBracketPrefix, newEntries, closingBracketPrefix, anchor);
        }

        public string? ClosingBracketPrefix => closingBracketPrefix;

        public Sequence WithClosingBracketPrefix(string? newClosingBracketPrefix)
        {
            return newClosingBracketPrefix == closingBracketPrefix ? this : new Sequence(id, markers, openingBracketPrefix, entries, newClosingBracketPrefix, anchor);
        }

        public Yaml.Anchor? Anchor => anchor;

        public Sequence WithAnchor(Yaml.Anchor? newAnchor)
        {
            return ReferenceEquals(newAnchor, anchor) ? this : new Sequence(id, markers, openingBracketPrefix, entries, closingBracketPrefix, newAnchor);
        }

        public sealed partial class Entry(
            Guid id,
            string prefix,
            Markers markers,
            Yaml.Block block,
            bool dash,
            string? trailingCommaPrefix
        ) : Yaml, MutableTree
        {
            public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
            {
                return v.VisitSequenceEntry(this, p);
            }

            public Guid Id => id;

            public Entry WithId(Guid newId)
            {
                return newId == id ? this : new Entry(newId, prefix, markers, block, dash, trailingCommaPrefix);
            }

            public string Prefix => prefix;

            public Entry WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new Entry(id, newPrefix, markers, block, dash, trailingCommaPrefix);
            }

            public Markers Markers => markers;

            public Entry WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Entry(id, prefix, newMarkers, block, dash, trailingCommaPrefix);
            }

            public Yaml.Block Block => block;

            public Entry WithBlock(Yaml.Block newBlock)
            {
                return ReferenceEquals(newBlock, block) ? this : new Entry(id, prefix, markers, newBlock, dash, trailingCommaPrefix);
            }

            public bool Dash => dash;

            public Entry WithDash(bool newDash)
            {
                return newDash == dash ? this : new Entry(id, prefix, markers, block, newDash, trailingCommaPrefix);
            }

            public string? TrailingCommaPrefix => trailingCommaPrefix;

            public Entry WithTrailingCommaPrefix(string? newTrailingCommaPrefix)
            {
                return newTrailingCommaPrefix == trailingCommaPrefix ? this : new Entry(id, prefix, markers, block, dash, newTrailingCommaPrefix);
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Sequence && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Alias(
        Guid id,
        string prefix,
        Markers markers,
        Anchor anchor
    ) : Yaml.Block,YamlKey    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitAlias(this, p);
        }

        public Guid Id => id;

        public Alias WithId(Guid newId)
        {
            return newId == id ? this : new Alias(newId, prefix, markers, anchor);
        }

        public string Prefix => prefix;

        public Alias WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Alias(id, newPrefix, markers, anchor);
        }

        public Markers Markers => markers;

        public Alias WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Alias(id, prefix, newMarkers, anchor);
        }

        public Yaml.Anchor Anchor => anchor;

        public Alias WithAnchor(Yaml.Anchor newAnchor)
        {
            return ReferenceEquals(newAnchor, anchor) ? this : new Alias(id, prefix, markers, newAnchor);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Alias && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Anchor(
        Guid id,
        string prefix,
        string postfix,
        Markers markers,
        string key
    ) : Yaml    {
        public Yaml? AcceptYaml<P>(YamlVisitor<P> v, P p)
        {
            return v.VisitAnchor(this, p);
        }

        public Guid Id => id;

        public Anchor WithId(Guid newId)
        {
            return newId == id ? this : new Anchor(newId, prefix, postfix, markers, key);
        }

        public string Prefix => prefix;

        public Anchor WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Anchor(id, newPrefix, postfix, markers, key);
        }

        public string Postfix => postfix;

        public Anchor WithPostfix(string newPostfix)
        {
            return newPostfix == postfix ? this : new Anchor(id, prefix, newPostfix, markers, key);
        }

        public Markers Markers => markers;

        public Anchor WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Anchor(id, prefix, postfix, newMarkers, key);
        }

        public string Key => key;

        public Anchor WithKey(string newKey)
        {
            return newKey == key ? this : new Anchor(id, prefix, postfix, markers, newKey);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Anchor && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
