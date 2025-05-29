using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Remote;
using Rewrite.RewriteYaml;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Remote.Codec.Yaml;

using Rewrite.RewriteYaml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression")]
public record YamlReceiver : Receiver
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

    private class Visitor : YamlVisitor<ReceiverContext>
    {
        public override Yaml? Visit(Tree? tree, ReceiverContext ctx, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.ReceiveNode((Yaml?)tree, ctx.ReceiveTree);

            Cursor = Cursor.Parent!;
            return (Yaml?)tree;
        }

        public override Yaml VisitDocuments(Yaml.Documents documents, ReceiverContext ctx)
        {
            documents = documents.WithId(ctx.ReceiveValue(documents.Id)!);
            documents = documents.WithMarkers(ctx.ReceiveNode(documents.Markers, ctx.ReceiveMarkers)!);
            documents = documents.WithSourcePath(ctx.ReceiveValue(documents.SourcePath)!);
            documents = documents.WithFileAttributes(ctx.ReceiveValue(documents.FileAttributes));
            documents = documents.WithCharsetName(ctx.ReceiveValue(documents.CharsetName));
            documents = documents.WithCharsetBomMarked(ctx.ReceiveValue(documents.CharsetBomMarked));
            documents = documents.WithChecksum(ctx.ReceiveValue(documents.Checksum));
            documents = documents.WithDocs(ctx.ReceiveNodes(documents.Docs, ctx.ReceiveTree)!);
            return documents;
        }

        public override Yaml VisitDocument(Yaml.Document document, ReceiverContext ctx)
        {
            document = document.WithId(ctx.ReceiveValue(document.Id)!);
            document = document.WithPrefix(ctx.ReceiveValue(document.Prefix)!);
            document = document.WithMarkers(ctx.ReceiveNode(document.Markers, ctx.ReceiveMarkers)!);
            document = document.WithExplicit(ctx.ReceiveValue(document.Explicit));
            document = document.WithBlock(ctx.ReceiveNode(document.Block, ctx.ReceiveTree)!);
            document = document.WithEnding(ctx.ReceiveNode(document.Ending, ctx.ReceiveTree)!);
            return document;
        }

        public override Yaml VisitDocumentEnd(Yaml.Document.End end, ReceiverContext ctx)
        {
            end = end.WithId(ctx.ReceiveValue(end.Id)!);
            end = end.WithPrefix(ctx.ReceiveValue(end.Prefix)!);
            end = end.WithMarkers(ctx.ReceiveNode(end.Markers, ctx.ReceiveMarkers)!);
            end = end.WithExplicit(ctx.ReceiveValue(end.Explicit));
            return end;
        }

        public override Yaml VisitScalar(Yaml.Scalar scalar, ReceiverContext ctx)
        {
            scalar = scalar.WithId(ctx.ReceiveValue(scalar.Id)!);
            scalar = scalar.WithPrefix(ctx.ReceiveValue(scalar.Prefix)!);
            scalar = scalar.WithMarkers(ctx.ReceiveNode(scalar.Markers, ctx.ReceiveMarkers)!);
            scalar = scalar.WithScalarStyle(ctx.ReceiveValue(scalar.ScalarStyle)!);
            scalar = scalar.WithAnchor(ctx.ReceiveNode(scalar.Anchor, ctx.ReceiveTree));
            scalar = scalar.WithValue(ctx.ReceiveValue(scalar.Value)!);
            return scalar;
        }

        public override Yaml VisitMapping(Yaml.Mapping mapping, ReceiverContext ctx)
        {
            mapping = mapping.WithId(ctx.ReceiveValue(mapping.Id)!);
            mapping = mapping.WithMarkers(ctx.ReceiveNode(mapping.Markers, ctx.ReceiveMarkers)!);
            mapping = mapping.WithOpeningBracePrefix(ctx.ReceiveValue(mapping.OpeningBracePrefix));
            mapping = mapping.WithEntries(ctx.ReceiveNodes(mapping.Entries, ctx.ReceiveTree)!);
            mapping = mapping.WithClosingBracePrefix(ctx.ReceiveValue(mapping.ClosingBracePrefix));
            mapping = mapping.WithAnchor(ctx.ReceiveNode(mapping.Anchor, ctx.ReceiveTree));
            return mapping;
        }

        public override Yaml VisitMappingEntry(Yaml.Mapping.Entry entry, ReceiverContext ctx)
        {
            entry = entry.WithId(ctx.ReceiveValue(entry.Id)!);
            entry = entry.WithPrefix(ctx.ReceiveValue(entry.Prefix)!);
            entry = entry.WithMarkers(ctx.ReceiveNode(entry.Markers, ctx.ReceiveMarkers)!);
            entry = entry.WithKey(ctx.ReceiveNode(entry.Key, ctx.ReceiveTree)!);
            entry = entry.WithBeforeMappingValueIndicator(ctx.ReceiveValue(entry.BeforeMappingValueIndicator)!);
            entry = entry.WithValue(ctx.ReceiveNode(entry.Value, ctx.ReceiveTree)!);
            return entry;
        }

        public override Yaml VisitSequence(Yaml.Sequence sequence, ReceiverContext ctx)
        {
            sequence = sequence.WithId(ctx.ReceiveValue(sequence.Id)!);
            sequence = sequence.WithMarkers(ctx.ReceiveNode(sequence.Markers, ctx.ReceiveMarkers)!);
            sequence = sequence.WithOpeningBracketPrefix(ctx.ReceiveValue(sequence.OpeningBracketPrefix));
            sequence = sequence.WithEntries(ctx.ReceiveNodes(sequence.Entries, ctx.ReceiveTree)!);
            sequence = sequence.WithClosingBracketPrefix(ctx.ReceiveValue(sequence.ClosingBracketPrefix));
            sequence = sequence.WithAnchor(ctx.ReceiveNode(sequence.Anchor, ctx.ReceiveTree));
            return sequence;
        }

        public override Yaml VisitSequenceEntry(Yaml.Sequence.Entry entry, ReceiverContext ctx)
        {
            entry = entry.WithId(ctx.ReceiveValue(entry.Id)!);
            entry = entry.WithPrefix(ctx.ReceiveValue(entry.Prefix)!);
            entry = entry.WithMarkers(ctx.ReceiveNode(entry.Markers, ctx.ReceiveMarkers)!);
            entry = entry.WithBlock(ctx.ReceiveNode(entry.Block, ctx.ReceiveTree)!);
            entry = entry.WithDash(ctx.ReceiveValue(entry.Dash));
            entry = entry.WithTrailingCommaPrefix(ctx.ReceiveValue(entry.TrailingCommaPrefix));
            return entry;
        }

        public override Yaml VisitAlias(Yaml.Alias alias, ReceiverContext ctx)
        {
            alias = alias.WithId(ctx.ReceiveValue(alias.Id)!);
            alias = alias.WithPrefix(ctx.ReceiveValue(alias.Prefix)!);
            alias = alias.WithMarkers(ctx.ReceiveNode(alias.Markers, ctx.ReceiveMarkers)!);
            alias = alias.WithAnchor(ctx.ReceiveNode(alias.Anchor, ctx.ReceiveTree)!);
            return alias;
        }

        public override Yaml VisitAnchor(Yaml.Anchor anchor, ReceiverContext ctx)
        {
            anchor = anchor.WithId(ctx.ReceiveValue(anchor.Id)!);
            anchor = anchor.WithPrefix(ctx.ReceiveValue(anchor.Prefix)!);
            anchor = anchor.WithPostfix(ctx.ReceiveValue(anchor.Postfix)!);
            anchor = anchor.WithMarkers(ctx.ReceiveNode(anchor.Markers, ctx.ReceiveMarkers)!);
            anchor = anchor.WithKey(ctx.ReceiveValue(anchor.Key)!);
            return anchor;
        }

    }

    private class Factory : ReceiverFactory
    {
        public Rewrite.Core.Tree Create<T>(string type, ReceiverContext ctx) where T : Rewrite.Core.Tree
        {
            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Documents" or "org.openrewrite.yaml.tree.Yaml$Documents")
            {
                return new Yaml.Documents(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(FileAttributes)),
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(Checksum)),
                    ctx.ReceiveNodes(default(IList<Yaml.Document>), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Document" or "org.openrewrite.yaml.tree.Yaml$Document")
            {
                return new Yaml.Document(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveNode(default(Yaml.Block), ctx.ReceiveTree)!,
                    ctx.ReceiveNode(default(Yaml.Document.End), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Document.End" or "org.openrewrite.yaml.tree.Yaml$Document$End")
            {
                return new Yaml.Document.End(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(bool))
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Scalar" or "org.openrewrite.yaml.tree.Yaml$Scalar")
            {
                return new Yaml.Scalar(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(Yaml.Scalar.Style))!,
                    ctx.ReceiveNode(default(Yaml.Anchor), ctx.ReceiveTree),
                    ctx.ReceiveValue(default(string))!
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Mapping" or "org.openrewrite.yaml.tree.Yaml$Mapping")
            {
                return new Yaml.Mapping(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveNodes(default(IList<Yaml.Mapping.Entry>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveNode(default(Yaml.Anchor), ctx.ReceiveTree)
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Mapping.Entry" or "org.openrewrite.yaml.tree.Yaml$Mapping$Entry")
            {
                return new Yaml.Mapping.Entry(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(YamlKey), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Yaml.Block), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Sequence" or "org.openrewrite.yaml.tree.Yaml$Sequence")
            {
                return new Yaml.Sequence(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveNodes(default(IList<Yaml.Sequence.Entry>), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(string)),
                    ctx.ReceiveNode(default(Yaml.Anchor), ctx.ReceiveTree)
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Sequence.Entry" or "org.openrewrite.yaml.tree.Yaml$Sequence$Entry")
            {
                return new Yaml.Sequence.Entry(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(Yaml.Block), ctx.ReceiveTree)!,
                    ctx.ReceiveValue(default(bool)),
                    ctx.ReceiveValue(default(string))
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Alias" or "org.openrewrite.yaml.tree.Yaml$Alias")
            {
                return new Yaml.Alias(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveNode(default(Yaml.Anchor), ctx.ReceiveTree)!
                );
            }

            if (type is "Rewrite.RewriteYaml.Tree.Yaml.Anchor" or "org.openrewrite.yaml.tree.Yaml$Anchor")
            {
                return new Yaml.Anchor(
                    ctx.ReceiveValue(default(Guid))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveValue(default(string))!,
                    ctx.ReceiveNode(default(Markers), ctx.ReceiveMarkers)!,
                    ctx.ReceiveValue(default(string))!
                );
            }

            throw new NotImplementedException("No factory method for type: " + type);
        }
    }

}
