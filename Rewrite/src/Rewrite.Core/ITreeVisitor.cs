using System.Diagnostics;
using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface ITreeVisitor<out T, TState> where T : class, Tree
{
    public static ITreeVisitor<Tree, ExecutionContext> Noop()
    {
        return new NoopVisitor();
    }

    T? DefaultValue(Tree? tree, TState p);
    bool IsAcceptable(SourceFile sourceFile, TState p);
    [DebuggerHidden]
    bool IsAdaptableTo(Type type);
    [DebuggerStepThrough]
    TTargetVisitor Adapt<TNodeType, TTargetVisitor>() where TNodeType : class, Tree where TTargetVisitor : class, ITreeVisitor<TNodeType, TState>
    {
        if (typeof(TTargetVisitor).IsAssignableFrom(GetType()))
            return (TTargetVisitor)this;
        if (!IsAdaptableTo(typeof(TTargetVisitor)))
            throw new ArgumentException(GetType() + " is not adaptable to " + typeof(TTargetVisitor));
        return TreeVisitorAdapter.Adapt<TTargetVisitor,TNodeType,TState>(this);
    }
    T? PreVisit(Tree? tree, TState p);
    T? Visit(Tree? tree, TState p);
    T? Visit(Tree? tree, TState p, Cursor parent);
    T? PostVisit(Tree tree, TState p);
    Markers? VisitMarkers(Marker.Markers? markers, TState p);
    TMarker VisitMarker<TMarker>(TMarker marker, TState p) where TMarker : Marker.Marker;
}

internal class NoopVisitor : TreeVisitor<Tree, ExecutionContext>
{
    public override Tree? Visit(Tree? tree, ExecutionContext p)
    {
        return tree;
    }
}
