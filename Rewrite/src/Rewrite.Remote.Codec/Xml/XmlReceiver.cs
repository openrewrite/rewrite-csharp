using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteXml;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Remote.Codec.Xml;

using Rewrite.RewriteXml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression")]
public record XmlReceiver : Receiver
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

    private class Visitor : XmlVisitor<ReceiverContext>
    {
        public override Xml? Visit(Tree? tree, ReceiverContext ctx, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.ReceiveNode((Xml?)tree, ctx.ReceiveTree);

            Cursor = Cursor.Parent!;
            return (Xml?)tree;
        }

        public override Xml VisitDocument(Xml.Document document, ReceiverContext ctx)
        {
            document = document.WithId(ctx.ReceiveValue(document.Id)!);
            document = document.WithSourcePath(ctx.ReceiveValue(document.SourcePath)!);
            document = document.WithPrefix(ctx.ReceiveValue(document.Prefix)!);
            document = document.WithMarkers(ctx.ReceiveNode(document.Markers, ctx.ReceiveMarkers)!);
            document = document.WithCharsetName(ctx.ReceiveValue(document.CharsetName));
            document = document.WithCharsetBomMarked(ctx.ReceiveValue(document.CharsetBomMarked));
            document = document.WithChecksum(ctx.ReceiveValue(document.Checksum));
            document = document.WithFileAttributes(ctx.ReceiveValue(document.FileAttributes));
            document = document.WithProlog(ctx.ReceiveNode(document.Prolog, ctx.ReceiveTree)!);
            document = document.WithRoot(ctx.ReceiveNode(document.Root, ctx.ReceiveTree)!);
            document = document.WithEof(ctx.ReceiveValue(document.Eof)!);
            return document;
        }

        public override Xml VisitProlog(Xml.Prolog prolog, ReceiverContext ctx)
        {
            prolog = prolog.WithId(ctx.ReceiveValue(prolog.Id)!);
            prolog = prolog.WithPrefix(ctx.ReceiveValue(prolog.Prefix)!);
            prolog = prolog.WithMarkers(ctx.ReceiveNode(prolog.Markers, ctx.ReceiveMarkers)!);
            prolog = prolog.WithXmlDecl(ctx.ReceiveNode(prolog.XmlDecl, ctx.ReceiveTree));
            prolog = prolog.WithMisc(ctx.ReceiveNodes(prolog.Misc, ctx.ReceiveTree)!);
            prolog = prolog.WithJspDirectives(ctx.ReceiveNodes(prolog.JspDirectives, ctx.ReceiveTree)!);
            return prolog;
        }

        public override Xml VisitXmlDecl(Xml.XmlDecl xmlDecl, ReceiverContext ctx)
        {
            xmlDecl = xmlDecl.WithId(ctx.ReceiveValue(xmlDecl.Id)!);
            xmlDecl = xmlDecl.WithPrefix(ctx.ReceiveValue(xmlDecl.Prefix)!);
            xmlDecl = xmlDecl.WithMarkers(ctx.ReceiveNode(xmlDecl.Markers, ctx.ReceiveMarkers)!);
            xmlDecl = xmlDecl.WithName(ctx.ReceiveValue(xmlDecl.Name)!);
            xmlDecl = xmlDecl.WithAttributes(ctx.ReceiveNodes(xmlDecl.Attributes, ctx.ReceiveTree)!);
            xmlDecl = xmlDecl.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(xmlDecl.BeforeTagDelimiterPrefix)!);
            return xmlDecl;
        }

        public override Xml VisitProcessingInstruction(Xml.ProcessingInstruction processingInstruction, ReceiverContext ctx)
        {
            processingInstruction = processingInstruction.WithId(ctx.ReceiveValue(processingInstruction.Id)!);
            processingInstruction = processingInstruction.WithPrefix(ctx.ReceiveValue(processingInstruction.Prefix)!);
            processingInstruction = processingInstruction.WithMarkers(ctx.ReceiveNode(processingInstruction.Markers, ctx.ReceiveMarkers)!);
            processingInstruction = processingInstruction.WithName(ctx.ReceiveValue(processingInstruction.Name)!);
            processingInstruction = processingInstruction.WithProcessingInstructions(ctx.ReceiveNode(processingInstruction.ProcessingInstructions, ctx.ReceiveTree)!);
            processingInstruction = processingInstruction.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(processingInstruction.BeforeTagDelimiterPrefix)!);
            return processingInstruction;
        }

        public override Xml VisitTag(Xml.Tag tag, ReceiverContext ctx)
        {
            tag = tag.WithId(ctx.ReceiveValue(tag.Id)!);
            tag = tag.WithPrefix(ctx.ReceiveValue(tag.Prefix)!);
            tag = tag.WithMarkers(ctx.ReceiveNode(tag.Markers, ctx.ReceiveMarkers)!);
            tag = tag.WithName(ctx.ReceiveValue(tag.Name)!);
            tag = tag.WithAttributes(ctx.ReceiveNodes(tag.Attributes, ctx.ReceiveTree)!);
            tag = tag.WithContent(ctx.ReceiveNodes(tag.Content, ctx.ReceiveTree)!);
            tag = tag.WithClosingTag(ctx.ReceiveNode(tag.ClosingTag, ctx.ReceiveTree));
            tag = tag.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(tag.BeforeTagDelimiterPrefix)!);
            return tag;
        }

        public override Xml VisitTagClosing(Xml.Tag.Closing closing, ReceiverContext ctx)
        {
            closing = closing.WithId(ctx.ReceiveValue(closing.Id)!);
            closing = closing.WithPrefix(ctx.ReceiveValue(closing.Prefix)!);
            closing = closing.WithMarkers(ctx.ReceiveNode(closing.Markers, ctx.ReceiveMarkers)!);
            closing = closing.WithName(ctx.ReceiveValue(closing.Name)!);
            closing = closing.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(closing.BeforeTagDelimiterPrefix)!);
            return closing;
        }

        public override Xml VisitAttribute(Xml.Attribute attribute, ReceiverContext ctx)
        {
            attribute = attribute.WithId(ctx.ReceiveValue(attribute.Id)!);
            attribute = attribute.WithPrefix(ctx.ReceiveValue(attribute.Prefix)!);
            attribute = attribute.WithMarkers(ctx.ReceiveNode(attribute.Markers, ctx.ReceiveMarkers)!);
            attribute = attribute.WithKey(ctx.ReceiveNode(attribute.Key, ctx.ReceiveTree)!);
            attribute = attribute.WithBeforeEquals(ctx.ReceiveValue(attribute.BeforeEquals)!);
            attribute = attribute.WithVal(ctx.ReceiveNode(attribute.Val, ctx.ReceiveTree)!);
            return attribute;
        }

        public override Xml VisitAttributeValue(Xml.Attribute.Value value, ReceiverContext ctx)
        {
            value = value.WithId(ctx.ReceiveValue(value.Id)!);
            value = value.WithPrefix(ctx.ReceiveValue(value.Prefix)!);
            value = value.WithMarkers(ctx.ReceiveNode(value.Markers, ctx.ReceiveMarkers)!);
            value = value.WithQuoteStyle(ctx.ReceiveValue(value.QuoteStyle)!);
            value = value.WithText(ctx.ReceiveValue(value.Text)!);
            return value;
        }

        public override Xml VisitCharData(Xml.CharData charData, ReceiverContext ctx)
        {
            charData = charData.WithId(ctx.ReceiveValue(charData.Id)!);
            charData = charData.WithPrefix(ctx.ReceiveValue(charData.Prefix)!);
            charData = charData.WithMarkers(ctx.ReceiveNode(charData.Markers, ctx.ReceiveMarkers)!);
            charData = charData.WithCdata(ctx.ReceiveValue(charData.Cdata));
            charData = charData.WithText(ctx.ReceiveValue(charData.Text)!);
            charData = charData.WithAfterText(ctx.ReceiveValue(charData.AfterText)!);
            return charData;
        }

        public override Xml VisitComment(Xml.Comment comment, ReceiverContext ctx)
        {
            comment = comment.WithId(ctx.ReceiveValue(comment.Id)!);
            comment = comment.WithPrefix(ctx.ReceiveValue(comment.Prefix)!);
            comment = comment.WithMarkers(ctx.ReceiveNode(comment.Markers, ctx.ReceiveMarkers)!);
            comment = comment.WithText(ctx.ReceiveValue(comment.Text)!);
            return comment;
        }

        public override Xml VisitDocTypeDecl(Xml.DocTypeDecl docTypeDecl, ReceiverContext ctx)
        {
            docTypeDecl = docTypeDecl.WithId(ctx.ReceiveValue(docTypeDecl.Id)!);
            docTypeDecl = docTypeDecl.WithPrefix(ctx.ReceiveValue(docTypeDecl.Prefix)!);
            docTypeDecl = docTypeDecl.WithMarkers(ctx.ReceiveNode(docTypeDecl.Markers, ctx.ReceiveMarkers)!);
            docTypeDecl = docTypeDecl.WithName(ctx.ReceiveNode(docTypeDecl.Name, ctx.ReceiveTree)!);
            docTypeDecl = docTypeDecl.WithExternalId(ctx.ReceiveNode(docTypeDecl.ExternalId, ctx.ReceiveTree));
            docTypeDecl = docTypeDecl.WithInternalSubset(ctx.ReceiveNodes(docTypeDecl.InternalSubset, ctx.ReceiveTree)!);
            docTypeDecl = docTypeDecl.WithSubsets(ctx.ReceiveNode(docTypeDecl.Subsets, ctx.ReceiveTree));
            docTypeDecl = docTypeDecl.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(docTypeDecl.BeforeTagDelimiterPrefix)!);
            return docTypeDecl;
        }

        public override Xml VisitDocTypeDeclExternalSubsets(Xml.DocTypeDecl.ExternalSubsets externalSubsets, ReceiverContext ctx)
        {
            externalSubsets = externalSubsets.WithId(ctx.ReceiveValue(externalSubsets.Id)!);
            externalSubsets = externalSubsets.WithPrefix(ctx.ReceiveValue(externalSubsets.Prefix)!);
            externalSubsets = externalSubsets.WithMarkers(ctx.ReceiveNode(externalSubsets.Markers, ctx.ReceiveMarkers)!);
            externalSubsets = externalSubsets.WithElements(ctx.ReceiveNodes(externalSubsets.Elements, ctx.ReceiveTree)!);
            return externalSubsets;
        }

        public override Xml VisitElement(Xml.Element element, ReceiverContext ctx)
        {
            element = element.WithId(ctx.ReceiveValue(element.Id)!);
            element = element.WithPrefix(ctx.ReceiveValue(element.Prefix)!);
            element = element.WithMarkers(ctx.ReceiveNode(element.Markers, ctx.ReceiveMarkers)!);
            element = element.WithSubset(ctx.ReceiveNodes(element.Subset, ctx.ReceiveTree)!);
            element = element.WithBeforeTagDelimiterPrefix(ctx.ReceiveValue(element.BeforeTagDelimiterPrefix)!);
            return element;
        }

        public override Xml VisitIdent(Xml.Ident ident, ReceiverContext ctx)
        {
            ident = ident.WithId(ctx.ReceiveValue(ident.Id)!);
            ident = ident.WithPrefix(ctx.ReceiveValue(ident.Prefix)!);
            ident = ident.WithMarkers(ctx.ReceiveNode(ident.Markers, ctx.ReceiveMarkers)!);
            ident = ident.WithName(ctx.ReceiveValue(ident.Name)!);
            return ident;
        }

        public override Xml VisitJspDirective(Xml.JspDirective jspDirective, ReceiverContext ctx)
        {
            jspDirective = jspDirective.WithId(ctx.ReceiveValue(jspDirective.Id)!);
            jspDirective = jspDirective.WithPrefix(ctx.ReceiveValue(jspDirective.Prefix)!);
            jspDirective = jspDirective.WithMarkers(ctx.ReceiveNode(jspDirective.Markers, ctx.ReceiveMarkers)!);
            jspDirective = jspDirective.WithBeforeTypePrefix(ctx.ReceiveValue(jspDirective.BeforeTypePrefix)!);
            jspDirective = jspDirective.WithType(ctx.ReceiveValue(jspDirective.Type)!);
            jspDirective = jspDirective.WithAttributes(ctx.ReceiveNodes(jspDirective.Attributes, ctx.ReceiveTree)!);
            jspDirective = jspDirective.WithBeforeDirectiveEndPrefix(ctx.ReceiveValue(jspDirective.BeforeDirectiveEndPrefix)!);
            return jspDirective;
        }

    }

    private class Factory : ReceiverFactory
    {
        public Rewrite.Core.Tree Create<T>(string type, ReceiverContext ctx) where T : Rewrite.Core.Tree
        {
            if (type is "Rewrite.RewriteXml.Tree.Xml.Document" or "org.openrewrite.xml.tree.Xml$Document")
            {
                return new Xml.Document(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(Checksum)),
                    ctx.ReceiveValue(default(FileAttributes)),
                    ctx.ReceiveNode(default(Xml.Prolog), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Xml.Tag), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Prolog" or "org.openrewrite.xml.tree.Xml$Prolog")
            {
                return new Xml.Prolog(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(Xml.XmlDecl), ctx.ReceiveTree),
                    ctx.ReceiveNodes(default(IList<Misc>), ctx.ReceiveTree)!,
                    ctx.ReceiveNodes(default(IList<Xml.JspDirective>), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.XmlDecl" or "org.openrewrite.xml.tree.Xml$XmlDecl")
            {
                return new Xml.XmlDecl(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNodes(default(IList<Xml.Attribute>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.ProcessingInstruction" or "org.openrewrite.xml.tree.Xml$ProcessingInstruction")
            {
                return new Xml.ProcessingInstruction(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Xml.CharData), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Tag" or "org.openrewrite.xml.tree.Xml$Tag")
            {
                return new Xml.Tag(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNodes(default(IList<Xml.Attribute>), ctx.ReceiveTree)!,
                    ctx.ReceiveNodes(default(IList<Content>), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Xml.Tag.Closing), ctx.ReceiveTree),
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Tag.Closing" or "org.openrewrite.xml.tree.Xml$Tag$Closing")
            {
                return new Xml.Tag.Closing(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Attribute" or "org.openrewrite.xml.tree.Xml$Attribute")
            {
                return new Xml.Attribute(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(Xml.Ident), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Xml.Attribute.Value), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Attribute.Value" or "org.openrewrite.xml.tree.Xml$Attribute$Value")
            {
                return new Xml.Attribute.Value(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(Xml.Attribute.Value.Quote))!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.CharData" or "org.openrewrite.xml.tree.Xml$CharData")
            {
                return new Xml.CharData(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Comment" or "org.openrewrite.xml.tree.Xml$Comment")
            {
                return new Xml.Comment(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.DocTypeDecl" or "org.openrewrite.xml.tree.Xml$DocTypeDecl")
            {
                return new Xml.DocTypeDecl(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(Xml.Ident), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Xml.Ident), ctx.ReceiveTree),
                    ctx.ReceiveNodes(default(IList<Xml.Ident>), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Xml.DocTypeDecl.ExternalSubsets), ctx.ReceiveTree),
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.DocTypeDecl.ExternalSubsets" or "org.openrewrite.xml.tree.Xml$DocTypeDecl$ExternalSubsets")
            {
                return new Xml.DocTypeDecl.ExternalSubsets(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNodes(default(IList<Xml.Element>), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Element" or "org.openrewrite.xml.tree.Xml$Element")
            {
                return new Xml.Element(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNodes(default(IList<Xml.Ident>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.Ident" or "org.openrewrite.xml.tree.Xml$Ident")
            {
                return new Xml.Ident(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteXml.Tree.Xml.JspDirective" or "org.openrewrite.xml.tree.Xml$JspDirective")
            {
                return new Xml.JspDirective(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNodes(default(IList<Xml.Attribute>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            throw new NotImplementedException("No factory method for type: " + type);
        }
    }

}
