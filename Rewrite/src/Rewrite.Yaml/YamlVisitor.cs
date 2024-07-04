using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteYaml.Tree;

namespace Rewrite.RewriteYaml;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
public class YamlVisitor<P> : TreeVisitor<Yaml, P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is Yaml;
    }

    public virtual Yaml? VisitDocuments(Yaml.Documents documents, P p)
    {
        documents = documents.WithMarkers(VisitMarkers(documents.Markers, p));
        documents = documents.WithDocs(ListUtils.Map(documents.Docs, el => (Yaml.Document?)Visit(el, p)));
        return documents;
    }

    public virtual Yaml? VisitDocument(Yaml.Document document, P p)
    {
        document = document.WithMarkers(VisitMarkers(document.Markers, p));
        document = document.WithBlock(VisitAndCast<Yaml.Block>(document.Block, p)!);
        document = document.WithEnding(VisitAndCast<Yaml.Document.End>(document.Ending, p)!);
        return document;
    }

    public virtual Yaml? VisitDocumentEnd(Yaml.Document.End end, P p)
    {
        end = end.WithMarkers(VisitMarkers(end.Markers, p));
        return end;
    }

    public virtual Yaml? VisitScalar(Yaml.Scalar scalar, P p)
    {
        scalar = scalar.WithMarkers(VisitMarkers(scalar.Markers, p));
        scalar = scalar.WithAnchor(VisitAndCast<Yaml.Anchor>(scalar.Anchor, p));
        return scalar;
    }

    public virtual Yaml? VisitMapping(Yaml.Mapping mapping, P p)
    {
        mapping = mapping.WithMarkers(VisitMarkers(mapping.Markers, p));
        mapping = mapping.WithEntries(ListUtils.Map(mapping.Entries, el => (Yaml.Mapping.Entry?)Visit(el, p)));
        mapping = mapping.WithAnchor(VisitAndCast<Yaml.Anchor>(mapping.Anchor, p));
        return mapping;
    }

    public virtual Yaml? VisitMappingEntry(Yaml.Mapping.Entry entry, P p)
    {
        entry = entry.WithMarkers(VisitMarkers(entry.Markers, p));
        entry = entry.WithKey(VisitAndCast<YamlKey>(entry.Key, p)!);
        entry = entry.WithValue(VisitAndCast<Yaml.Block>(entry.Value, p)!);
        return entry;
    }

    public virtual Yaml? VisitSequence(Yaml.Sequence sequence, P p)
    {
        sequence = sequence.WithMarkers(VisitMarkers(sequence.Markers, p));
        sequence = sequence.WithEntries(ListUtils.Map(sequence.Entries, el => (Yaml.Sequence.Entry?)Visit(el, p)));
        sequence = sequence.WithAnchor(VisitAndCast<Yaml.Anchor>(sequence.Anchor, p));
        return sequence;
    }

    public virtual Yaml? VisitSequenceEntry(Yaml.Sequence.Entry entry, P p)
    {
        entry = entry.WithMarkers(VisitMarkers(entry.Markers, p));
        entry = entry.WithBlock(VisitAndCast<Yaml.Block>(entry.Block, p)!);
        return entry;
    }

    public virtual Yaml? VisitAlias(Yaml.Alias alias, P p)
    {
        alias = alias.WithMarkers(VisitMarkers(alias.Markers, p));
        alias = alias.WithAnchor(VisitAndCast<Yaml.Anchor>(alias.Anchor, p)!);
        return alias;
    }

    public virtual Yaml? VisitAnchor(Yaml.Anchor anchor, P p)
    {
        anchor = anchor.WithMarkers(VisitMarkers(anchor.Markers, p));
        return anchor;
    }

}
