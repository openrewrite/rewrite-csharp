using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.RewriteProperties.Tree;

namespace Rewrite.RewriteProperties;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
public class PropertiesVisitor<P> : TreeVisitor<Properties, P>
{
    public override bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return sourceFile is Properties;
    }

    public virtual Properties? VisitFile(Properties.File file, P p)
    {
        file = file.WithMarkers(VisitMarkers(file.Markers, p));
        file = file.WithContent(ListUtils.Map(file.Content, el => (Properties.Content?)Visit(el, p)));
        return file;
    }

    public virtual Properties? VisitEntry(Properties.Entry entry, P p)
    {
        entry = entry.WithMarkers(VisitMarkers(entry.Markers, p));
        entry = entry.WithValue(VisitValue(entry.Value, p)!);
        return entry;
    }

    public virtual Properties.Value? VisitValue(Properties.Value value, P p)
    {
        value = value.WithMarkers(VisitMarkers(value.Markers, p));
        return value;
    }

    public virtual Properties? VisitComment(Properties.Comment comment, P p)
    {
        comment = comment.WithMarkers(VisitMarkers(comment.Markers, p));
        return comment;
    }

}