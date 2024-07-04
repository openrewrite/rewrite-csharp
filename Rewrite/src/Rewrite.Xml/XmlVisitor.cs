using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteXml.Tree;

namespace Rewrite.RewriteXml;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
public class XmlVisitor<P> : TreeVisitor<Xml, P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is Xml;
    }

    public virtual Xml? VisitDocument(Xml.Document document, P p)
    {
        document = document.WithMarkers(VisitMarkers(document.Markers, p));
        document = document.WithProlog(VisitAndCast<Xml.Prolog>(document.Prolog, p)!);
        document = document.WithRoot(VisitAndCast<Xml.Tag>(document.Root, p)!);
        return document;
    }

    public virtual Xml? VisitProlog(Xml.Prolog prolog, P p)
    {
        prolog = prolog.WithMarkers(VisitMarkers(prolog.Markers, p));
        prolog = prolog.WithXmlDecl(VisitAndCast<Xml.XmlDecl>(prolog.XmlDecl, p));
        prolog = prolog.WithMisc(ListUtils.Map(prolog.Misc, el => (Misc?)Visit(el, p)));
        prolog = prolog.WithJspDirectives(ListUtils.Map(prolog.JspDirectives, el => (Xml.JspDirective?)Visit(el, p)));
        return prolog;
    }

    public virtual Xml? VisitXmlDecl(Xml.XmlDecl xmlDecl, P p)
    {
        xmlDecl = xmlDecl.WithMarkers(VisitMarkers(xmlDecl.Markers, p));
        xmlDecl = xmlDecl.WithAttributes(ListUtils.Map(xmlDecl.Attributes, el => (Xml.Attribute?)Visit(el, p)));
        return xmlDecl;
    }

    public virtual Xml? VisitProcessingInstruction(Xml.ProcessingInstruction processingInstruction, P p)
    {
        processingInstruction = processingInstruction.WithMarkers(VisitMarkers(processingInstruction.Markers, p));
        processingInstruction = processingInstruction.WithProcessingInstructions(VisitAndCast<Xml.CharData>(processingInstruction.ProcessingInstructions, p)!);
        return processingInstruction;
    }

    public virtual Xml? VisitTag(Xml.Tag tag, P p)
    {
        tag = tag.WithMarkers(VisitMarkers(tag.Markers, p));
        tag = tag.WithAttributes(ListUtils.Map(tag.Attributes, el => (Xml.Attribute?)Visit(el, p)));
        tag = tag.WithContent(ListUtils.Map(tag.Content, el => (Content?)Visit(el, p)));
        tag = tag.WithClosingTag(VisitAndCast<Xml.Tag.Closing>(tag.ClosingTag, p));
        return tag;
    }

    public virtual Xml? VisitTagClosing(Xml.Tag.Closing closing, P p)
    {
        closing = closing.WithMarkers(VisitMarkers(closing.Markers, p));
        return closing;
    }

    public virtual Xml? VisitAttribute(Xml.Attribute attribute, P p)
    {
        attribute = attribute.WithMarkers(VisitMarkers(attribute.Markers, p));
        attribute = attribute.WithKey(VisitAndCast<Xml.Ident>(attribute.Key, p)!);
        attribute = attribute.WithVal(VisitAndCast<Xml.Attribute.Value>(attribute.Val, p)!);
        return attribute;
    }

    public virtual Xml? VisitAttributeValue(Xml.Attribute.Value value, P p)
    {
        value = value.WithMarkers(VisitMarkers(value.Markers, p));
        return value;
    }

    public virtual Xml? VisitCharData(Xml.CharData charData, P p)
    {
        charData = charData.WithMarkers(VisitMarkers(charData.Markers, p));
        return charData;
    }

    public virtual Xml? VisitComment(Xml.Comment comment, P p)
    {
        comment = comment.WithMarkers(VisitMarkers(comment.Markers, p));
        return comment;
    }

    public virtual Xml? VisitDocTypeDecl(Xml.DocTypeDecl docTypeDecl, P p)
    {
        docTypeDecl = docTypeDecl.WithMarkers(VisitMarkers(docTypeDecl.Markers, p));
        docTypeDecl = docTypeDecl.WithName(VisitAndCast<Xml.Ident>(docTypeDecl.Name, p)!);
        docTypeDecl = docTypeDecl.WithExternalId(VisitAndCast<Xml.Ident>(docTypeDecl.ExternalId, p));
        docTypeDecl = docTypeDecl.WithInternalSubset(ListUtils.Map(docTypeDecl.InternalSubset, el => (Xml.Ident?)Visit(el, p)));
        docTypeDecl = docTypeDecl.WithSubsets(VisitAndCast<Xml.DocTypeDecl.ExternalSubsets>(docTypeDecl.Subsets, p));
        return docTypeDecl;
    }

    public virtual Xml? VisitDocTypeDeclExternalSubsets(Xml.DocTypeDecl.ExternalSubsets externalSubsets, P p)
    {
        externalSubsets = externalSubsets.WithMarkers(VisitMarkers(externalSubsets.Markers, p));
        externalSubsets = externalSubsets.WithElements(ListUtils.Map(externalSubsets.Elements, el => (Xml.Element?)Visit(el, p)));
        return externalSubsets;
    }

    public virtual Xml? VisitElement(Xml.Element element, P p)
    {
        element = element.WithMarkers(VisitMarkers(element.Markers, p));
        element = element.WithSubset(ListUtils.Map(element.Subset, el => (Xml.Ident?)Visit(el, p)));
        return element;
    }

    public virtual Xml? VisitIdent(Xml.Ident ident, P p)
    {
        ident = ident.WithMarkers(VisitMarkers(ident.Markers, p));
        return ident;
    }

    public virtual Xml? VisitJspDirective(Xml.JspDirective jspDirective, P p)
    {
        jspDirective = jspDirective.WithMarkers(VisitMarkers(jspDirective.Markers, p));
        jspDirective = jspDirective.WithAttributes(ListUtils.Map(jspDirective.Attributes, el => (Xml.Attribute?)Visit(el, p)));
        return jspDirective;
    }

}
