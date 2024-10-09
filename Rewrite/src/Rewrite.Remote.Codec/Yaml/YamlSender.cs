using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteYaml;

namespace Rewrite.Remote.Codec.Yaml;

using Rewrite.RewriteYaml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record YamlSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Core.Tree {
        var visitor = new Visitor();
        visitor.Visit(after, ctx.Fork(visitor, before));
    }

    private class Visitor : YamlVisitor<SenderContext>
    {
        public override Yaml Visit(Tree? tree, SenderContext ctx)
        {
            Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
            ctx.SendNode(tree, x => x, ctx.SendTree);
            Cursor = Cursor.Parent!;

            return (Yaml) tree;
        }

        public override Yaml VisitDocuments(Yaml.Documents documents, SenderContext ctx)
        {
            ctx.SendValue(documents, v => v.Id);
            ctx.SendNode(documents, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(documents, v => v.SourcePath);
            ctx.SendTypedValue(documents, v => v.FileAttributes);
            ctx.SendValue(documents, v => v.CharsetName);
            ctx.SendValue(documents, v => v.CharsetBomMarked);
            ctx.SendTypedValue(documents, v => v.Checksum);
            ctx.SendNodes(documents, v => v.Docs, ctx.SendTree, t => t.Id);
            return documents;
        }

        public override Yaml VisitDocument(Yaml.Document document, SenderContext ctx)
        {
            ctx.SendValue(document, v => v.Id);
            ctx.SendValue(document, v => v.Prefix);
            ctx.SendNode(document, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(document, v => v.Explicit);
            ctx.SendNode(document, v => v.Block, ctx.SendTree);
            ctx.SendNode(document, v => v.Ending, ctx.SendTree);
            return document;
        }

        public override Yaml VisitDocumentEnd(Yaml.Document.End end, SenderContext ctx)
        {
            ctx.SendValue(end, v => v.Id);
            ctx.SendValue(end, v => v.Prefix);
            ctx.SendNode(end, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(end, v => v.Explicit);
            return end;
        }

        public override Yaml VisitScalar(Yaml.Scalar scalar, SenderContext ctx)
        {
            ctx.SendValue(scalar, v => v.Id);
            ctx.SendValue(scalar, v => v.Prefix);
            ctx.SendNode(scalar, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(scalar, v => v.ScalarStyle);
            ctx.SendNode(scalar, v => v.Anchor, ctx.SendTree);
            ctx.SendValue(scalar, v => v.Value);
            return scalar;
        }

        public override Yaml VisitMapping(Yaml.Mapping mapping, SenderContext ctx)
        {
            ctx.SendValue(mapping, v => v.Id);
            ctx.SendNode(mapping, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(mapping, v => v.OpeningBracePrefix);
            ctx.SendNodes(mapping, v => v.Entries, ctx.SendTree, t => t.Id);
            ctx.SendValue(mapping, v => v.ClosingBracePrefix);
            ctx.SendNode(mapping, v => v.Anchor, ctx.SendTree);
            return mapping;
        }

        public override Yaml VisitMappingEntry(Yaml.Mapping.Entry entry, SenderContext ctx)
        {
            ctx.SendValue(entry, v => v.Id);
            ctx.SendValue(entry, v => v.Prefix);
            ctx.SendNode(entry, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(entry, v => v.Key, ctx.SendTree);
            ctx.SendValue(entry, v => v.BeforeMappingValueIndicator);
            ctx.SendNode(entry, v => v.Value, ctx.SendTree);
            return entry;
        }

        public override Yaml VisitSequence(Yaml.Sequence sequence, SenderContext ctx)
        {
            ctx.SendValue(sequence, v => v.Id);
            ctx.SendNode(sequence, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(sequence, v => v.OpeningBracketPrefix);
            ctx.SendNodes(sequence, v => v.Entries, ctx.SendTree, t => t.Id);
            ctx.SendValue(sequence, v => v.ClosingBracketPrefix);
            ctx.SendNode(sequence, v => v.Anchor, ctx.SendTree);
            return sequence;
        }

        public override Yaml VisitSequenceEntry(Yaml.Sequence.Entry entry, SenderContext ctx)
        {
            ctx.SendValue(entry, v => v.Id);
            ctx.SendValue(entry, v => v.Prefix);
            ctx.SendNode(entry, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(entry, v => v.Block, ctx.SendTree);
            ctx.SendValue(entry, v => v.Dash);
            ctx.SendValue(entry, v => v.TrailingCommaPrefix);
            return entry;
        }

        public override Yaml VisitAlias(Yaml.Alias alias, SenderContext ctx)
        {
            ctx.SendValue(alias, v => v.Id);
            ctx.SendValue(alias, v => v.Prefix);
            ctx.SendNode(alias, v => v.Markers, ctx.SendMarkers);
            ctx.SendNode(alias, v => v.Anchor, ctx.SendTree);
            return alias;
        }

        public override Yaml VisitAnchor(Yaml.Anchor anchor, SenderContext ctx)
        {
            ctx.SendValue(anchor, v => v.Id);
            ctx.SendValue(anchor, v => v.Prefix);
            ctx.SendValue(anchor, v => v.Postfix);
            ctx.SendNode(anchor, v => v.Markers, ctx.SendMarkers);
            ctx.SendValue(anchor, v => v.Key);
            return anchor;
        }

    }
}
