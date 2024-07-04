using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface Tree : IEquatable<Tree>
{
    public static Guid RandomId()
    {
        return Guid.NewGuid();
    }

    Guid Id { get; }
    Markers Markers { get; }

    bool IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree;

    R? Accept<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree
    {
        return v.DefaultValue(this, p);
    }

    string Print<P>(Cursor cursor, PrintOutputCapture<P> capture)
    {
        Printer<P>(cursor).Visit(this, capture, cursor);
        return capture.GetOut();
    }

    ITreeVisitor<Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
    {
        return cursor.FirstEnclosingOrThrow<SourceFile>().Printer<P>(cursor);
    }
}