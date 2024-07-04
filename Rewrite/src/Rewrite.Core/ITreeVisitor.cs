namespace Rewrite.Core;

public interface ITreeVisitor<out T, P> where T : class, Tree
{
    public static ITreeVisitor<Tree, ExecutionContext> Noop()
    {
        return new NoopVisitor();
    }

    T? DefaultValue(Tree? tree, P p);
    bool IsAcceptable(SourceFile sourceFile, P p);
    bool IsAdaptableTo(Type type);
    V Adapt<R, V>() where R : class, Tree where V : class, ITreeVisitor<R, P>
    {
        if (typeof(V).IsAssignableFrom(GetType()))
            return (V)this;
        if (!IsAdaptableTo(typeof(V)))
            throw new ArgumentException(GetType() + " is not adaptable to " + typeof(V));
        return TreeVisitorAdapter.Adapt<V,R,P>(this);
    }
    T? PreVisit(Tree? tree, P p);
    T? Visit(Tree? tree, P p);
    T? Visit(Tree? tree, P p, Cursor parent);
    T? PostVisit(Tree tree, P p);
    Marker.Markers VisitMarkers(Marker.Markers? markers, P p);
    M VisitMarker<M>(M marker, P p) where M : Marker.Marker;
}

internal class NoopVisitor : TreeVisitor<Tree, ExecutionContext>
{
    public override Tree? Visit(Tree? tree, ExecutionContext p)
    {
        return tree;
    }
}
