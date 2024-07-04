using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteXml.Tree;

namespace Rewrite.RewriteXml;

public class XmlPrinter<P> : XmlVisitor<PrintOutputCapture<P>>
{
    public override Tree.Xml VisitDocument(Tree.Xml.Document document, PrintOutputCapture<P> p)
    {
        BeforeSyntax(document, p);
        document = (Tree.Xml.Document)base.VisitDocument(document, p);
        AfterSyntax(document, p);
        p.Append(document.Eof);
        return document;
    }

    public override Tree.Xml VisitProlog(Tree.Xml.Prolog prolog, PrintOutputCapture<P> p)
    {
        BeforeSyntax(prolog, p);
        prolog = (Tree.Xml.Prolog)base.VisitProlog(prolog, p);
        AfterSyntax(prolog, p);
        return prolog;
    }

    public override Tree.Xml VisitXmlDecl(Tree.Xml.XmlDecl xmlDecl, PrintOutputCapture<P> p)
    {
        BeforeSyntax(xmlDecl, p);
        p.Append("<?")
            .Append(xmlDecl.Name);
        Visit(xmlDecl.Attributes, p);
        p.Append(xmlDecl.BeforeTagDelimiterPrefix)
            .Append("?>");
        AfterSyntax(xmlDecl, p);
        return xmlDecl;
    }

    public override Tree.Xml VisitTag(Tree.Xml.Tag tag, PrintOutputCapture<P> p)
    {
        BeforeSyntax(tag, p);
        p.Append('<');
        bool isJspDirective = Cursor.GetParentOrThrow().Value is Tree.Xml.Prolog;
        if (isJspDirective) {
            p.Append("%@");
        }
        p.Append(tag.Name);
        Visit(tag.Attributes, p);
        p.Append(tag.BeforeTagDelimiterPrefix);
        if (tag.ClosingTag == null) {
            p.Append("/>");
        } else {
            p.Append('>');
            Visit(tag.Content, p);
            Visit(tag.ClosingTag, p);
        }
        AfterSyntax(tag, p);
        return tag;
    }

    public override Tree.Xml VisitTagClosing(Tree.Xml.Tag.Closing closing, PrintOutputCapture<P> p)
    {
        BeforeSyntax(closing, p);
        p.Append("</")
            .Append(closing.Name)
            .Append(closing.BeforeTagDelimiterPrefix)
            .Append(">");
        AfterSyntax(closing, p);
        return closing;
    }

    public override Tree.Xml VisitAttribute(Tree.Xml.Attribute attribute, PrintOutputCapture<P> p)
    {
        BeforeSyntax(attribute, p);
        p.Append(attribute.Key.GetPrefix())
            .Append(attribute.Key.Name)
            .Append(attribute.BeforeEquals)
            .Append('=');
        Visit(attribute.Val, p);
        AfterSyntax(attribute, p);
        return attribute;
    }

    public override Tree.Xml VisitAttributeValue(Tree.Xml.Attribute.Value value, PrintOutputCapture<P> p)
    {
        BeforeSyntax(value, p);
        var valueDelim = Tree.Xml.Attribute.Value.Quote.Double == value.QuoteStyle ? '"' : '\'';
        p.Append(valueDelim)
            .Append(value.Text)
            .Append(valueDelim);
        AfterSyntax(value, p);
        return value;
    }

    public override Tree.Xml VisitComment(Tree.Xml.Comment comment, PrintOutputCapture<P> p)
    {
        BeforeSyntax(comment, p);
        p.Append("<!--")
            .Append(comment.Text)
            .Append("-->");
        AfterSyntax(comment, p);
        return comment;
    }

    public override Tree.Xml VisitProcessingInstruction(Tree.Xml.ProcessingInstruction processingInstruction, PrintOutputCapture<P> p)
    {
        BeforeSyntax(processingInstruction, p);
        p.Append("<?")
            .Append(processingInstruction.Name);
        Visit(processingInstruction.ProcessingInstructions, p);
        p.Append(processingInstruction.BeforeTagDelimiterPrefix)
            .Append("?>");
        AfterSyntax(processingInstruction, p);
        return processingInstruction;
    }

    public override Tree.Xml VisitCharData(Tree.Xml.CharData charData, PrintOutputCapture<P> p)
    {
        BeforeSyntax(charData, p);
        if (charData.Cdata) {
            p.Append("<![CDATA[")
                .Append(charData.Text)
                .Append("]]>");
        } else {
            p.Append(charData.Text);
        }
        p.Append(charData.AfterText);
        AfterSyntax(charData, p);
        return charData;
    }

    public override Tree.Xml VisitDocTypeDecl(Tree.Xml.DocTypeDecl docTypeDecl, PrintOutputCapture<P> p)
    {
        BeforeSyntax(docTypeDecl, p);
        p.Append("<!DOCTYPE");
        Visit(docTypeDecl.Name, p);
        Visit(docTypeDecl.ExternalId, p);
        Visit(docTypeDecl.InternalSubset, p);
        Visit(docTypeDecl.Subsets, p);
        p.Append(docTypeDecl.BeforeTagDelimiterPrefix);
        p.Append('>');
        AfterSyntax(docTypeDecl, p);
        return docTypeDecl;
    }

    public override Tree.Xml VisitDocTypeDeclExternalSubsets(Tree.Xml.DocTypeDecl.ExternalSubsets externalSubsets, PrintOutputCapture<P> p)
    {
        BeforeSyntax(externalSubsets, p);
        p.Append('[');
        Visit(externalSubsets.Elements, p);
        p.Append(']');
        AfterSyntax(externalSubsets, p);
        return externalSubsets;
    }

    public override Tree.Xml VisitElement(Tree.Xml.Element element, PrintOutputCapture<P> p)
    {
        BeforeSyntax(element, p);
        Visit(element.Subset, p);
        p.Append(element.BeforeTagDelimiterPrefix);
        AfterSyntax(element, p);
        return element;
    }

    public override Tree.Xml VisitIdent(Tree.Xml.Ident ident, PrintOutputCapture<P> p)
    {
        BeforeSyntax(ident, p);
        p.Append(ident.Name);
        AfterSyntax(ident, p);
        return ident;
    }

    public override Tree.Xml VisitJspDirective(Tree.Xml.JspDirective jspDirective, PrintOutputCapture<P> p)
    {
        BeforeSyntax(jspDirective, p);
        p.Append("<%@");
        p.Append(jspDirective.BeforeTypePrefix);
        p.Append(jspDirective.Type);
        Visit(jspDirective.Attributes, p);
        p.Append(jspDirective.BeforeDirectiveEndPrefix);
        p.Append("%>");
        AfterSyntax(jspDirective, p);
        return jspDirective;
    }

    private static readonly Func<string, string> XmlMarkerWrapper =
        o => "<!--~~" + o + (o.Length == 0 ? "" : "~~") + ">-->";

    private void BeforeSyntax(Tree.Xml x, PrintOutputCapture<P> p)
    {
        foreach (var marker in x.Markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), XmlMarkerWrapper));
        }

        p.Append(x.GetPrefix());
        VisitMarkers(x.Markers, p);
        foreach (Marker marker in x.Markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), XmlMarkerWrapper));
        }
    }

    private void AfterSyntax(Tree.Xml x, PrintOutputCapture<P> p)
    {
        foreach (Marker marker in x.Markers.MarkerList) {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), XmlMarkerWrapper));
        }
    }
}