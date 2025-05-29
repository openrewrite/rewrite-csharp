using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewrite.Core.Marker;

namespace Rewrite.Core;

public interface ITreeVisitor<out T, TState> where T : class, Tree
{
    public static ITreeVisitor<Tree, IExecutionContext> Noop()
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
    T? Visit(Tree? tree, TState p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "");
    T? Visit(Tree? tree, TState p, Cursor parent);
    Markers? VisitMarkers(Marker.Markers? markers, TState p);
    Core.Marker.Marker VisitMarker(Core.Marker.Marker marker, TState p);
}
internal class NoopVisitor<T> : TreeVisitor<T, IExecutionContext> where T: class, Tree
{
    public override T? Visit(Tree? tree, IExecutionContext p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
    {
        return tree as T;
    }
}
internal class NoopVisitor : TreeVisitor<Tree, IExecutionContext>
{
    public override Tree? Visit(Tree? tree, IExecutionContext p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
    {
        return tree;
    }
}
