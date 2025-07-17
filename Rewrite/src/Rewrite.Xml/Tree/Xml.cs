using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteXml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Xml : Rewrite.Core.Tree
{
    bool Core.Tree.IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p)
    {
        return v.IsAdaptableTo(typeof(XmlVisitor<>));
    }

    R? Core.Tree.Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class
    {
        return (R?)AcceptXml(v.Adapt<Xml, XmlVisitor<P>>(), p);
    }

    Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
    {
        return v.DefaultValue(this, p);
    }

    public partial class Document(
        Guid id,
        string sourcePath,
        string prefix,
        Markers markers,
        string? charsetName,
        bool charsetBomMarked,
        Checksum? checksum,
        FileAttributes? fileAttributes,
        Prolog prolog,
        Tag root,
        string eof
    ) : Xml,MutableSourceFile<Document>    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitDocument(this, p);
        }

        public Guid Id => id;

        public Document WithId(Guid newId)
        {
            return newId == id ? this : new Document(newId, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public string SourcePath => sourcePath;

        public Document WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new Document(id, newSourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public string Prefix => prefix;

        public Document WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Document(id, sourcePath, newPrefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public Markers Markers => markers;

        public Document WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Document(id, sourcePath, prefix, newMarkers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public string? CharsetName => charsetName;

        public Document WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new Document(id, sourcePath, prefix, markers, newCharsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public bool CharsetBomMarked => charsetBomMarked;

        public Document WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new Document(id, sourcePath, prefix, markers, charsetName, newCharsetBomMarked, checksum, fileAttributes, prolog, root, eof);
        }

        public Checksum? Checksum => checksum;

        public Document WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, newChecksum, fileAttributes, prolog, root, eof);
        }

        public FileAttributes? FileAttributes => fileAttributes;

        public Document WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, newFileAttributes, prolog, root, eof);
        }

        public Xml.Prolog Prolog => prolog;

        public Document WithProlog(Xml.Prolog newProlog)
        {
            return ReferenceEquals(newProlog, prolog) ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, newProlog, root, eof);
        }

        public Xml.Tag Root => root;

        public Document WithRoot(Xml.Tag newRoot)
        {
            return ReferenceEquals(newRoot, root) ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, newRoot, eof);
        }

        public string Eof => eof;

        public Document WithEof(string newEof)
        {
            return newEof == eof ? this : new Document(id, sourcePath, prefix, markers, charsetName, charsetBomMarked, checksum, fileAttributes, prolog, root, newEof);
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
            return IPrinterFactory.Current()!.CreatePrinter<P>();
        }
    }

    public sealed partial class Prolog(
        Guid id,
        string prefix,
        Markers markers,
        XmlDecl? xmlDecl,
        IList<Misc> misc,
        IList<JspDirective> jspDirectives
    ) : Xml    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitProlog(this, p);
        }

        public Guid Id => id;

        public Prolog WithId(Guid newId)
        {
            return newId == id ? this : new Prolog(newId, prefix, markers, xmlDecl, misc, jspDirectives);
        }

        public string Prefix => prefix;

        public Prolog WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Prolog(id, newPrefix, markers, xmlDecl, misc, jspDirectives);
        }

        public Markers Markers => markers;

        public Prolog WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Prolog(id, prefix, newMarkers, xmlDecl, misc, jspDirectives);
        }

        public Xml.XmlDecl? XmlDecl => xmlDecl;

        public Prolog WithXmlDecl(Xml.XmlDecl? newXmlDecl)
        {
            return ReferenceEquals(newXmlDecl, xmlDecl) ? this : new Prolog(id, prefix, markers, newXmlDecl, misc, jspDirectives);
        }

        public IList<Misc> Misc => misc;

        public Prolog WithMisc(IList<Misc> newMisc)
        {
            return newMisc == misc ? this : new Prolog(id, prefix, markers, xmlDecl, newMisc, jspDirectives);
        }

        public IList<Xml.JspDirective> JspDirectives => jspDirectives;

        public Prolog WithJspDirectives(IList<Xml.JspDirective> newJspDirectives)
        {
            return newJspDirectives == jspDirectives ? this : new Prolog(id, prefix, markers, xmlDecl, misc, newJspDirectives);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Prolog && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class XmlDecl(
        Guid id,
        string prefix,
        Markers markers,
        string name,
        IList<Attribute> attributes,
        string beforeTagDelimiterPrefix
    ) : Xml,Misc    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitXmlDecl(this, p);
        }

        public Guid Id => id;

        public XmlDecl WithId(Guid newId)
        {
            return newId == id ? this : new XmlDecl(newId, prefix, markers, name, attributes, beforeTagDelimiterPrefix);
        }

        public string Prefix => prefix;

        public XmlDecl WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new XmlDecl(id, newPrefix, markers, name, attributes, beforeTagDelimiterPrefix);
        }

        public Markers Markers => markers;

        public XmlDecl WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new XmlDecl(id, prefix, newMarkers, name, attributes, beforeTagDelimiterPrefix);
        }

        public string Name => name;

        public XmlDecl WithName(string newName)
        {
            return newName == name ? this : new XmlDecl(id, prefix, markers, newName, attributes, beforeTagDelimiterPrefix);
        }

        public IList<Xml.Attribute> Attributes => attributes;

        public XmlDecl WithAttributes(IList<Xml.Attribute> newAttributes)
        {
            return newAttributes == attributes ? this : new XmlDecl(id, prefix, markers, name, newAttributes, beforeTagDelimiterPrefix);
        }

        public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

        public XmlDecl WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
        {
            return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new XmlDecl(id, prefix, markers, name, attributes, newBeforeTagDelimiterPrefix);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is XmlDecl && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class ProcessingInstruction(
        Guid id,
        string prefix,
        Markers markers,
        string name,
        CharData processingInstructions,
        string beforeTagDelimiterPrefix
    ) : Xml,Content,Misc    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitProcessingInstruction(this, p);
        }

        public Guid Id => id;

        public ProcessingInstruction WithId(Guid newId)
        {
            return newId == id ? this : new ProcessingInstruction(newId, prefix, markers, name, processingInstructions, beforeTagDelimiterPrefix);
        }

        public string Prefix => prefix;

        public ProcessingInstruction WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new ProcessingInstruction(id, newPrefix, markers, name, processingInstructions, beforeTagDelimiterPrefix);
        }

        public Markers Markers => markers;

        public ProcessingInstruction WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ProcessingInstruction(id, prefix, newMarkers, name, processingInstructions, beforeTagDelimiterPrefix);
        }

        public string Name => name;

        public ProcessingInstruction WithName(string newName)
        {
            return newName == name ? this : new ProcessingInstruction(id, prefix, markers, newName, processingInstructions, beforeTagDelimiterPrefix);
        }

        public Xml.CharData ProcessingInstructions => processingInstructions;

        public ProcessingInstruction WithProcessingInstructions(Xml.CharData newProcessingInstructions)
        {
            return ReferenceEquals(newProcessingInstructions, processingInstructions) ? this : new ProcessingInstruction(id, prefix, markers, name, newProcessingInstructions, beforeTagDelimiterPrefix);
        }

        public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

        public ProcessingInstruction WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
        {
            return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new ProcessingInstruction(id, prefix, markers, name, processingInstructions, newBeforeTagDelimiterPrefix);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ProcessingInstruction && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Tag(
        Guid id,
        string prefix,
        Markers markers,
        string name,
        IList<Attribute> attributes,
        IList<Content> content,
        Tag.Closing? closingTag,
        string beforeTagDelimiterPrefix
    ) : Xml,Content    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitTag(this, p);
        }

        public Guid Id => id;

        public Tag WithId(Guid newId)
        {
            return newId == id ? this : new Tag(newId, prefix, markers, name, attributes, content, closingTag, beforeTagDelimiterPrefix);
        }

        public string Prefix => prefix;

        public Tag WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Tag(id, newPrefix, markers, name, attributes, content, closingTag, beforeTagDelimiterPrefix);
        }

        public Markers Markers => markers;

        public Tag WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Tag(id, prefix, newMarkers, name, attributes, content, closingTag, beforeTagDelimiterPrefix);
        }

        public string Name => name;

        public Tag WithName(string newName)
        {
            return newName == name ? this : new Tag(id, prefix, markers, newName, attributes, content, closingTag, beforeTagDelimiterPrefix);
        }

        public IList<Xml.Attribute> Attributes => attributes;

        public Tag WithAttributes(IList<Xml.Attribute> newAttributes)
        {
            return newAttributes == attributes ? this : new Tag(id, prefix, markers, name, newAttributes, content, closingTag, beforeTagDelimiterPrefix);
        }

        public IList<Content> Content => content;

        public Tag WithContent(IList<Content> newContent)
        {
            return newContent == content ? this : new Tag(id, prefix, markers, name, attributes, newContent, closingTag, beforeTagDelimiterPrefix);
        }

        public Closing? ClosingTag => closingTag;

        public Tag WithClosingTag(Closing? newClosingTag)
        {
            return ReferenceEquals(newClosingTag, closingTag) ? this : new Tag(id, prefix, markers, name, attributes, content, newClosingTag, beforeTagDelimiterPrefix);
        }

        public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

        public Tag WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
        {
            return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new Tag(id, prefix, markers, name, attributes, content, closingTag, newBeforeTagDelimiterPrefix);
        }

        public sealed partial class Closing(
            Guid id,
            string prefix,
            Markers markers,
            string name,
            string beforeTagDelimiterPrefix
        ) : Xml
        {
            public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
            {
                return v.VisitTagClosing(this, p);
            }

            public Guid Id => id;

            public Closing WithId(Guid newId)
            {
                return newId == id ? this : new Closing(newId, prefix, markers, name, beforeTagDelimiterPrefix);
            }

            public string Prefix => prefix;

            public Closing WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new Closing(id, newPrefix, markers, name, beforeTagDelimiterPrefix);
            }

            public Markers Markers => markers;

            public Closing WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Closing(id, prefix, newMarkers, name, beforeTagDelimiterPrefix);
            }

            public string Name => name;

            public Closing WithName(string newName)
            {
                return newName == name ? this : new Closing(id, prefix, markers, newName, beforeTagDelimiterPrefix);
            }

            public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

            public Closing WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
            {
                return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new Closing(id, prefix, markers, name, newBeforeTagDelimiterPrefix);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Closing && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Tag && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Attribute(
        Guid id,
        string prefix,
        Markers markers,
        Ident key,
        string beforeEquals,
        Attribute.Value val
    ) : Xml    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitAttribute(this, p);
        }

        public Guid Id => id;

        public Attribute WithId(Guid newId)
        {
            return newId == id ? this : new Attribute(newId, prefix, markers, key, beforeEquals, val);
        }

        public string Prefix => prefix;

        public Attribute WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Attribute(id, newPrefix, markers, key, beforeEquals, val);
        }

        public Markers Markers => markers;

        public Attribute WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Attribute(id, prefix, newMarkers, key, beforeEquals, val);
        }

        public Xml.Ident Key => key;

        public Attribute WithKey(Xml.Ident newKey)
        {
            return ReferenceEquals(newKey, key) ? this : new Attribute(id, prefix, markers, newKey, beforeEquals, val);
        }

        public string BeforeEquals => beforeEquals;

        public Attribute WithBeforeEquals(string newBeforeEquals)
        {
            return newBeforeEquals == beforeEquals ? this : new Attribute(id, prefix, markers, key, newBeforeEquals, val);
        }

        public Value Val => val;

        public Attribute WithVal(Value newVal)
        {
            return ReferenceEquals(newVal, val) ? this : new Attribute(id, prefix, markers, key, beforeEquals, newVal);
        }

        public sealed partial class Value(
            Guid id,
            string prefix,
            Markers markers,
            Value.Quote quoteStyle,
            string text
        ) : Xml
        {
            public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
            {
                return v.VisitAttributeValue(this, p);
            }

            public enum Quote
            {
                Double,
                Single,

            }

            public Guid Id => id;

            public Value WithId(Guid newId)
            {
                return newId == id ? this : new Value(newId, prefix, markers, quoteStyle, text);
            }

            public string Prefix => prefix;

            public Value WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new Value(id, newPrefix, markers, quoteStyle, text);
            }

            public Markers Markers => markers;

            public Value WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Value(id, prefix, newMarkers, quoteStyle, text);
            }

            public Quote QuoteStyle => quoteStyle;

            public Value WithQuoteStyle(Quote newQuoteStyle)
            {
                return newQuoteStyle == quoteStyle ? this : new Value(id, prefix, markers, newQuoteStyle, text);
            }

            public string Text => text;

            public Value WithText(string newText)
            {
                return newText == text ? this : new Value(id, prefix, markers, quoteStyle, newText);
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

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Attribute && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class CharData(
        Guid id,
        string prefix,
        Markers markers,
        bool cdata,
        string text,
        string afterText
    ) : Xml,Content    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitCharData(this, p);
        }

        public Guid Id => id;

        public CharData WithId(Guid newId)
        {
            return newId == id ? this : new CharData(newId, prefix, markers, cdata, text, afterText);
        }

        public string Prefix => prefix;

        public CharData WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new CharData(id, newPrefix, markers, cdata, text, afterText);
        }

        public Markers Markers => markers;

        public CharData WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CharData(id, prefix, newMarkers, cdata, text, afterText);
        }

        public bool Cdata => cdata;

        public CharData WithCdata(bool newCdata)
        {
            return newCdata == cdata ? this : new CharData(id, prefix, markers, newCdata, text, afterText);
        }

        public string Text => text;

        public CharData WithText(string newText)
        {
            return newText == text ? this : new CharData(id, prefix, markers, cdata, newText, afterText);
        }

        public string AfterText => afterText;

        public CharData WithAfterText(string newAfterText)
        {
            return newAfterText == afterText ? this : new CharData(id, prefix, markers, cdata, text, newAfterText);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CharData && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Comment(
        Guid id,
        string prefix,
        Markers markers,
        string text
    ) : Xml,Content,Misc    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitComment(this, p);
        }

        public Guid Id => id;

        public Comment WithId(Guid newId)
        {
            return newId == id ? this : new Comment(newId, prefix, markers, text);
        }

        public string Prefix => prefix;

        public Comment WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Comment(id, newPrefix, markers, text);
        }

        public Markers Markers => markers;

        public Comment WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Comment(id, prefix, newMarkers, text);
        }

        public string Text => text;

        public Comment WithText(string newText)
        {
            return newText == text ? this : new Comment(id, prefix, markers, newText);
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

    public sealed partial class DocTypeDecl(
        Guid id,
        string prefix,
        Markers markers,
        Ident name,
        Ident? externalId,
        IList<Ident> internalSubset,
        DocTypeDecl.ExternalSubsets? subsets,
        string beforeTagDelimiterPrefix
    ) : Xml,Misc    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitDocTypeDecl(this, p);
        }

        public Guid Id => id;

        public DocTypeDecl WithId(Guid newId)
        {
            return newId == id ? this : new DocTypeDecl(newId, prefix, markers, name, externalId, internalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public string Prefix => prefix;

        public DocTypeDecl WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new DocTypeDecl(id, newPrefix, markers, name, externalId, internalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public Markers Markers => markers;

        public DocTypeDecl WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new DocTypeDecl(id, prefix, newMarkers, name, externalId, internalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public Xml.Ident Name => name;

        public DocTypeDecl WithName(Xml.Ident newName)
        {
            return ReferenceEquals(newName, name) ? this : new DocTypeDecl(id, prefix, markers, newName, externalId, internalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public Xml.Ident? ExternalId => externalId;

        public DocTypeDecl WithExternalId(Xml.Ident? newExternalId)
        {
            return ReferenceEquals(newExternalId, externalId) ? this : new DocTypeDecl(id, prefix, markers, name, newExternalId, internalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public IList<Xml.Ident> InternalSubset => internalSubset;

        public DocTypeDecl WithInternalSubset(IList<Xml.Ident> newInternalSubset)
        {
            return newInternalSubset == internalSubset ? this : new DocTypeDecl(id, prefix, markers, name, externalId, newInternalSubset, subsets, beforeTagDelimiterPrefix);
        }

        public ExternalSubsets? Subsets => subsets;

        public DocTypeDecl WithSubsets(ExternalSubsets? newSubsets)
        {
            return ReferenceEquals(newSubsets, subsets) ? this : new DocTypeDecl(id, prefix, markers, name, externalId, internalSubset, newSubsets, beforeTagDelimiterPrefix);
        }

        public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

        public DocTypeDecl WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
        {
            return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new DocTypeDecl(id, prefix, markers, name, externalId, internalSubset, subsets, newBeforeTagDelimiterPrefix);
        }

        public sealed partial class ExternalSubsets(
            Guid id,
            string prefix,
            Markers markers,
            IList<Xml.Element> elements
        ) : Xml
        {
            public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
            {
                return v.VisitDocTypeDeclExternalSubsets(this, p);
            }

            public Guid Id => id;

            public ExternalSubsets WithId(Guid newId)
            {
                return newId == id ? this : new ExternalSubsets(newId, prefix, markers, elements);
            }

            public string Prefix => prefix;

            public ExternalSubsets WithPrefix(string newPrefix)
            {
                return newPrefix == prefix ? this : new ExternalSubsets(id, newPrefix, markers, elements);
            }

            public Markers Markers => markers;

            public ExternalSubsets WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new ExternalSubsets(id, prefix, newMarkers, elements);
            }

            public IList<Xml.Element> Elements => elements;

            public ExternalSubsets WithElements(IList<Xml.Element> newElements)
            {
                return newElements == elements ? this : new ExternalSubsets(id, prefix, markers, newElements);
            }

            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is ExternalSubsets && other.Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DocTypeDecl && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Element(
        Guid id,
        string prefix,
        Markers markers,
        IList<Ident> subset,
        string beforeTagDelimiterPrefix
    ) : Xml    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitElement(this, p);
        }

        public Guid Id => id;

        public Element WithId(Guid newId)
        {
            return newId == id ? this : new Element(newId, prefix, markers, subset, beforeTagDelimiterPrefix);
        }

        public string Prefix => prefix;

        public Element WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Element(id, newPrefix, markers, subset, beforeTagDelimiterPrefix);
        }

        public Markers Markers => markers;

        public Element WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Element(id, prefix, newMarkers, subset, beforeTagDelimiterPrefix);
        }

        public IList<Xml.Ident> Subset => subset;

        public Element WithSubset(IList<Xml.Ident> newSubset)
        {
            return newSubset == subset ? this : new Element(id, prefix, markers, newSubset, beforeTagDelimiterPrefix);
        }

        public string BeforeTagDelimiterPrefix => beforeTagDelimiterPrefix;

        public Element WithBeforeTagDelimiterPrefix(string newBeforeTagDelimiterPrefix)
        {
            return newBeforeTagDelimiterPrefix == beforeTagDelimiterPrefix ? this : new Element(id, prefix, markers, subset, newBeforeTagDelimiterPrefix);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Element && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class Ident(
        Guid id,
        string prefix,
        Markers markers,
        string name
    ) : Xml    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitIdent(this, p);
        }

        public Guid Id => id;

        public Ident WithId(Guid newId)
        {
            return newId == id ? this : new Ident(newId, prefix, markers, name);
        }

        public string Prefix => prefix;

        public Ident WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new Ident(id, newPrefix, markers, name);
        }

        public Markers Markers => markers;

        public Ident WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Ident(id, prefix, newMarkers, name);
        }

        public string Name => name;

        public Ident WithName(string newName)
        {
            return newName == name ? this : new Ident(id, prefix, markers, newName);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Ident && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public sealed partial class JspDirective(
        Guid id,
        string prefix,
        Markers markers,
        string beforeTypePrefix,
        string type,
        IList<Attribute> attributes,
        string beforeDirectiveEndPrefix
    ) : Xml,Content    {
        public Xml? AcceptXml<P>(XmlVisitor<P> v, P p)
        {
            return v.VisitJspDirective(this, p);
        }

        public Guid Id => id;

        public JspDirective WithId(Guid newId)
        {
            return newId == id ? this : new JspDirective(newId, prefix, markers, beforeTypePrefix, type, attributes, beforeDirectiveEndPrefix);
        }

        public string Prefix => prefix;

        public JspDirective WithPrefix(string newPrefix)
        {
            return newPrefix == prefix ? this : new JspDirective(id, newPrefix, markers, beforeTypePrefix, type, attributes, beforeDirectiveEndPrefix);
        }

        public Markers Markers => markers;

        public JspDirective WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new JspDirective(id, prefix, newMarkers, beforeTypePrefix, type, attributes, beforeDirectiveEndPrefix);
        }

        public string BeforeTypePrefix => beforeTypePrefix;

        public JspDirective WithBeforeTypePrefix(string newBeforeTypePrefix)
        {
            return newBeforeTypePrefix == beforeTypePrefix ? this : new JspDirective(id, prefix, markers, newBeforeTypePrefix, type, attributes, beforeDirectiveEndPrefix);
        }

        public string Type => type;

        public JspDirective WithType(string newType)
        {
            return newType == type ? this : new JspDirective(id, prefix, markers, beforeTypePrefix, newType, attributes, beforeDirectiveEndPrefix);
        }

        public IList<Xml.Attribute> Attributes => attributes;

        public JspDirective WithAttributes(IList<Xml.Attribute> newAttributes)
        {
            return newAttributes == attributes ? this : new JspDirective(id, prefix, markers, beforeTypePrefix, type, newAttributes, beforeDirectiveEndPrefix);
        }

        public string BeforeDirectiveEndPrefix => beforeDirectiveEndPrefix;

        public JspDirective WithBeforeDirectiveEndPrefix(string newBeforeDirectiveEndPrefix)
        {
            return newBeforeDirectiveEndPrefix == beforeDirectiveEndPrefix ? this : new JspDirective(id, prefix, markers, beforeTypePrefix, type, attributes, newBeforeDirectiveEndPrefix);
        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is JspDirective && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
