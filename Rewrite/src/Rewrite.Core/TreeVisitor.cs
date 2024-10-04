using System.Diagnostics;
using Rewrite.Core.Marker;

namespace Rewrite.Core;

public abstract class TreeVisitor<T, P> : ITreeVisitor<T, P> where T : class, Tree
{

    public Cursor Cursor { get; set; } = new (null, Cursor.ROOT_VALUE);

    public bool IsAdaptableTo(Type adaptTo)
    {
        if (adaptTo.IsAssignableFrom(GetType())) {
            return true;
        }
        var mine = VisitorTreeType(GetType());
        var theirs = VisitorTreeType(adaptTo);
        return mine.IsAssignableFrom(theirs);
    }

    private static Type VisitorTreeType(Type type)
    {
        do
        {
            type = type.BaseType!;
        } while (type is { BaseType: not null, IsGenericType: true } && type.GetGenericTypeDefinition() != typeof(TreeVisitor<,>));
        return type.GenericTypeArguments[0];
    }

    // public V Adapt<R, V>(Type adaptTo) where R : Tree where V : TreeVisitor<R, P>
    // {
    //     return (V)this;
    // }

    public virtual T? DefaultValue(Tree? tree, P p)
    {
        return (T?)tree;
    }

    public virtual bool IsAcceptable(SourceFile sourceFile, P p)
    {
        return true;
    }

    public virtual T? PreVisit(Tree? tree, P p)
    {
        return DefaultValue(tree, p);
    }

    public virtual T? PostVisit(Tree tree, P p)
    {
        return (T?)tree;
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public virtual T? Visit(Tree? tree, P p)
    {
        if (tree is null)
        {
            return DefaultValue(tree, p);
        }

        Cursor = new Cursor(Cursor, tree);

        T? t = null;
        var isAcceptable = tree.IsAcceptable(this, p) && (tree is not SourceFile file || IsAcceptable(file, p));
        if (isAcceptable)
        {
            t = PreVisit((T?)tree, p);
            if (t != null)
            {
                t = t.Accept(this, p);
            }

            if (t != null)
            {
                t = PostVisit(t, p);
            }
        }

        Cursor = Cursor.Parent!;

        return isAcceptable ? t :  tree as T; // todo: some serious issue here because in case of ParseError node, it will return null. JavaVisitor returns J for every visit, so it's impossible to properly return a ParseError
    }

    public void Visit<T2>(IList<T2>? nodes, P p) where T2 : T
    {
        if (nodes != null)
        {
            foreach (T node in nodes)
            {
                Visit(node, p);
            }
        }
    }

    public T2? VisitAndCast<T2>(Tree? tree, P p) where T2 : T
    {
        return (T2?)Visit(tree, p);
    }

    public virtual Markers VisitMarkers(Marker.Markers? markers, P p)
    {
        if (markers == null || ReferenceEquals(markers, Marker.Markers.EMPTY))
        {
            return Marker.Markers.EMPTY;
        }

        if (markers.MarkerList.Count == 0)
        {
            return markers;
        }

        return markers with
        {
            MarkerList = ListUtils.Map(markers.MarkerList, m => VisitMarker(m, p))!
        };
    }

    public virtual M VisitMarker<M>(M marker, P p) where M : Marker.Marker
    {
        return marker;
    }

    public virtual T? Visit(Tree? tree, P p, Cursor parent)
    {
        Cursor = parent;
        return Visit(tree, p);
    }
}
