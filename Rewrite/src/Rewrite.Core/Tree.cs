using System.Diagnostics;
using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface Tree : IHasMarkers, IEquatable<Tree>
{
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    internal static string? ToString(Tree node)
    {
        var output = new PrintOutputCapture<object?>(null);
        IPrinterFactory.Default?.CreatePrinter<object?>().Visit(node, output);

        return output.ToString();
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public static Guid RandomId()
    {
        return Guid.NewGuid();
    }

    Guid Id { get; }

    bool IsAcceptable<R, P>(ITreeVisitor<R, P> v, P p) where R : class, Tree;

#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    TNodeType? Accept<TNodeType, TState>(ITreeVisitor<TNodeType, TState> v, TState p) where TNodeType : class, Tree
    {
        return v.DefaultValue(this, p);
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    string Print(Cursor cursor) => Print(cursor, new PrintOutputCapture<object>(0));
    string Print<P>(Cursor cursor, PrintOutputCapture<P> capture)
    {
        Printer<P>(cursor).Visit(this, capture, cursor);
        return capture.GetOut();
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    ITreeVisitor<Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
    {
        return cursor.FirstEnclosingOrThrow<SourceFile>().Printer<P>(cursor);
    }

    bool IsScope(Tree? tree) => tree?.Id.Equals(Id) ?? false;


    public Cursor? Find(Predicate<Core.Tree> predicate) 
    {
        throw new NotImplementedException();
    }
}

public static class TreeExtensions
{
    public static string PrintTrimmed(this Tree tree, Cursor cursor) => tree.Print(cursor).TrimIndent();
}