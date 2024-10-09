using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteXml;

namespace Rewrite.Remote.Codec.Xml;

using Rewrite.RewriteXml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record XmlSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Core.Tree {
        var visitor = new Visitor();
        visitor.Visit(after, ctx.Fork(visitor, before));
    }

    private class Visitor : XmlVisitor<SenderContext>
    {
        public override Xml Visit(Tree? tree, SenderContext ctx)
        {
            Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
            ctx.SendNode(tree, x => x, ctx.SendTree);
            Cursor = Cursor.Parent!;

            return (Xml) tree;
        }

        public override Xml VisitDocument(Xml.Document document, SenderContext ctx)
        {
            ctx.SendValue(document, v => v.Id);
            ctx.SendValue(document, v => v.SourcePath);
            ctx.SendValue(document, v => v.Prefix);
            ctx.SendNode(document, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(document, v => v.CharsetName);
            ctx.SendValue(document, v => v.CharsetBomMarked);
            ctx.SendTypedValue(document, v => v.Checksum);
            ctx.SendTypedValue(document, v => v.FileAttributes);
            ctx.SendNode(document, v => v.Prolog, ctx.SendTree);
            ctx.SendNode(document, v => v.Root, ctx.SendTree);
            ctx.SendValue(document, v => v.Eof);
            return document;
        }

        public override Xml VisitProlog(Xml.Prolog prolog, SenderContext ctx)
        {
            ctx.SendValue(prolog, v => v.Id);
            ctx.SendValue(prolog, v => v.Prefix);
            ctx.SendNode(prolog, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(prolog, v => v.XmlDecl, ctx.SendTree);
            ctx.SendNodes(prolog, v => v.Misc, ctx.SendTree, t => t.Id);
            ctx.SendNodes(prolog, v => v.JspDirectives, ctx.SendTree, t => t.Id);
            return prolog;
        }

        public override Xml VisitXmlDecl(Xml.XmlDecl xmlDecl, SenderContext ctx)
        {
            ctx.SendValue(xmlDecl, v => v.Id);
            ctx.SendValue(xmlDecl, v => v.Prefix);
            ctx.SendNode(xmlDecl, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(xmlDecl, v => v.Name);
            ctx.SendNodes(xmlDecl, v => v.Attributes, ctx.SendTree, t => t.Id);
            ctx.SendValue(xmlDecl, v => v.BeforeTagDelimiterPrefix);
            return xmlDecl;
        }

        public override Xml VisitProcessingInstruction(Xml.ProcessingInstruction processingInstruction, SenderContext ctx)
        {
            ctx.SendValue(processingInstruction, v => v.Id);
            ctx.SendValue(processingInstruction, v => v.Prefix);
            ctx.SendNode(processingInstruction, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(processingInstruction, v => v.Name);
            ctx.SendNode(processingInstruction, v => v.ProcessingInstructions, ctx.SendTree);
            ctx.SendValue(processingInstruction, v => v.BeforeTagDelimiterPrefix);
            return processingInstruction;
        }

        public override Xml VisitTag(Xml.Tag tag, SenderContext ctx)
        {
            ctx.SendValue(tag, v => v.Id);
            ctx.SendValue(tag, v => v.Prefix);
            ctx.SendNode(tag, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(tag, v => v.Name);
            ctx.SendNodes(tag, v => v.Attributes, ctx.SendTree, t => t.Id);
            ctx.SendNodes(tag, v => v.Content ?? [], ctx.SendTree, t => t.Id);
            ctx.SendNode(tag, v => v.ClosingTag, ctx.SendTree);
            ctx.SendValue(tag, v => v.BeforeTagDelimiterPrefix);
            return tag;
        }

        public override Xml VisitTagClosing(Xml.Tag.Closing closing, SenderContext ctx)
        {
            ctx.SendValue(closing, v => v.Id);
            ctx.SendValue(closing, v => v.Prefix);
            ctx.SendNode(closing, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(closing, v => v.Name);
            ctx.SendValue(closing, v => v.BeforeTagDelimiterPrefix);
            return closing;
        }

        public override Xml VisitAttribute(Xml.Attribute attribute, SenderContext ctx)
        {
            ctx.SendValue(attribute, v => v.Id);
            ctx.SendValue(attribute, v => v.Prefix);
            ctx.SendNode(attribute, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(attribute, v => v.Key, ctx.SendTree);
            ctx.SendValue(attribute, v => v.BeforeEquals);
            ctx.SendNode(attribute, v => v.Val, ctx.SendTree);
            return attribute;
        }

        public override Xml VisitAttributeValue(Xml.Attribute.Value value, SenderContext ctx)
        {
            ctx.SendValue(value, v => v.Id);
            ctx.SendValue(value, v => v.Prefix);
            ctx.SendNode(value, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(value, v => v.QuoteStyle);
            ctx.SendValue(value, v => v.Text);
            return value;
        }

        public override Xml VisitCharData(Xml.CharData charData, SenderContext ctx)
        {
            ctx.SendValue(charData, v => v.Id);
            ctx.SendValue(charData, v => v.Prefix);
            ctx.SendNode(charData, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(charData, v => v.Cdata);
            ctx.SendValue(charData, v => v.Text);
            ctx.SendValue(charData, v => v.AfterText);
            return charData;
        }

        public override Xml VisitComment(Xml.Comment comment, SenderContext ctx)
        {
            ctx.SendValue(comment, v => v.Id);
            ctx.SendValue(comment, v => v.Prefix);
            ctx.SendNode(comment, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(comment, v => v.Text);
            return comment;
        }

        public override Xml VisitDocTypeDecl(Xml.DocTypeDecl docTypeDecl, SenderContext ctx)
        {
            ctx.SendValue(docTypeDecl, v => v.Id);
            ctx.SendValue(docTypeDecl, v => v.Prefix);
            ctx.SendNode(docTypeDecl, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(docTypeDecl, v => v.Name, ctx.SendTree);
            ctx.SendNode(docTypeDecl, v => v.ExternalId, ctx.SendTree);
            ctx.SendNodes(docTypeDecl, v => v.InternalSubset, ctx.SendTree, t => t.Id);
            ctx.SendNode(docTypeDecl, v => v.Subsets, ctx.SendTree);
            ctx.SendValue(docTypeDecl, v => v.BeforeTagDelimiterPrefix);
            return docTypeDecl;
        }

        public override Xml VisitDocTypeDeclExternalSubsets(Xml.DocTypeDecl.ExternalSubsets externalSubsets, SenderContext ctx)
        {
            ctx.SendValue(externalSubsets, v => v.Id);
            ctx.SendValue(externalSubsets, v => v.Prefix);
            ctx.SendNode(externalSubsets, v => v.Markers, ctx.SendMarkers);
            ctx.SendNodes(externalSubsets, v => v.Elements, ctx.SendTree, t => t.Id);
            return externalSubsets;
        }

        public override Xml VisitElement(Xml.Element element, SenderContext ctx)
        {
            ctx.SendValue(element, v => v.Id);
            ctx.SendValue(element, v => v.Prefix);
            ctx.SendNode(element, v => v.Markers, ctx.SendMarkers);
            ctx.SendNodes(element, v => v.Subset, ctx.SendTree, t => t.Id);
            ctx.SendValue(element, v => v.BeforeTagDelimiterPrefix);
            return element;
        }

        public override Xml VisitIdent(Xml.Ident ident, SenderContext ctx)
        {
            ctx.SendValue(ident, v => v.Id);
            ctx.SendValue(ident, v => v.Prefix);
            ctx.SendNode(ident, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(ident, v => v.Name);
            return ident;
        }

        public override Xml VisitJspDirective(Xml.JspDirective jspDirective, SenderContext ctx)
        {
            ctx.SendValue(jspDirective, v => v.Id);
            ctx.SendValue(jspDirective, v => v.Prefix);
            ctx.SendNode(jspDirective, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(jspDirective, v => v.BeforeTypePrefix);
            ctx.SendValue(jspDirective, v => v.Type);
            ctx.SendNodes(jspDirective, v => v.Attributes, ctx.SendTree, t => t.Id);
            ctx.SendValue(jspDirective, v => v.BeforeDirectiveEndPrefix);
            return jspDirective;
        }

    }
}
