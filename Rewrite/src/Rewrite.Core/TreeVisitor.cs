using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewrite.Core.Marker;

namespace Rewrite.Core;
#if DEBUG_VISITOR
[DebuggerStepThrough]
#endif
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

    public virtual T? PreVisit(Tree? tree, P p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
    {
        return DefaultValue(tree, p);
    }

    public virtual T? PostVisit(Tree tree, P p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
    {
        return (T?)tree;
    }
#if DEBUG_VISITOR
    [DebuggerStepThrough]
#endif
    public virtual T? Visit(Tree? tree, P p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
    {
        if (tree is null)
        {
            return DefaultValue(tree, p);
        }

        Cursor = new Cursor(Cursor, tree, callingMethodName, callingArgumentExpression);

        T? t = null;
        var isAcceptable = tree.IsAcceptable(this, p) && (tree is not SourceFile file || IsAcceptable(file, p));
        if (isAcceptable)
        {
            t = PreVisit((T?)tree, p, callingMethodName, callingArgumentExpression);
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

    public virtual Markers VisitMarkers(Markers? markers, P p)
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

    public virtual Core.Marker.Marker VisitMarker(Core.Marker.Marker marker, P p)
    {
        return marker;
    }

    public virtual T? Visit(Tree? tree, P p, Cursor parent)
    {
        Cursor = parent;
        return Visit(tree, p);
    }
}

// public abstract class TreeVisitorAsync<T,P> : ITreeVisitorAsync<T,P> where T : class, Tree
// {
//     
//     public Cursor Cursor { get; set; } = new (null, Cursor.ROOT_VALUE);
//     
//
//     public bool IsAdaptableTo(Type adaptTo)
//     {
//         // if (!adaptTo.Name.EndsWith("Async"))
//         // {
//         //     adaptTo = Type.GetType($"{adaptTo.FullName}Async, {adaptTo.Assembly.GetName().Name}")!;
//         // }
//         if (adaptTo.IsAssignableFrom(GetType())) {
//             return true;
//         }
//         var mine = VisitorTreeType(GetType());
//         var theirs = VisitorTreeType(adaptTo);
//         return mine.IsAssignableFrom(theirs);
//     }
//
//     private static Type VisitorTreeType(Type type)
//     {
//         // if(type.Name.EndsWith("Async"))
//         do
//         {
//             type = type.BaseType!;
//         } while (type is { BaseType: not null, IsGenericType: true } && type.GetGenericTypeDefinition() != typeof(TreeVisitorAsync<,>));
//         return type.GenericTypeArguments[0];
//     }
//
//     // public V Adapt<R, V>(Type adaptTo) where R : Tree where V : TreeVisitor<R, P>
//     // {
//     //     return (V)this;
//     // }
//
//     public virtual T? DefaultValue(Tree? tree, P p)
//     {
//         return (T?)tree;
//     }
//
//     public virtual bool IsAcceptable(SourceFile sourceFile, P p)
//     {
//         return true;
//     }
//
//     public virtual T? PreVisit(Tree? tree, P p)
//     {
//         return DefaultValue(tree, p);
//     }
//
//     public virtual T? PostVisit(Tree tree, P p)
//     {
//         return (T?)tree;
//     }
// #if DEBUG_VISITOR
//     [DebuggerStepThrough]
// #endif
//     T? ITreeVisitor<T, P>.Visit(Tree? tree, P p, string callingMethodName, string callingArgumentExpression) => Visit(tree, p, callingMethodName, callingArgumentExpression).Result;
//     public virtual async Task<T?> Visit(Tree? tree, P p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
//     {
//         if (tree is null)
//         {
//             return DefaultValue(tree, p);
//         }
//
//         Cursor = new Cursor(Cursor, tree, callingMethodName, callingArgumentExpression);
//
//         T? t = null;
//         var isAcceptable = tree.IsAcceptable(this, p) && (tree is not SourceFile file || IsAcceptable(file, p));
//         if (isAcceptable)
//         {
//             t = PreVisit((T?)tree, p);
//             if (t != null)
//             {
//                 t = t.Accept(this, p);
//             }
//
//             if (t != null)
//             {
//                 t = PostVisit(t, p);
//             }
//         }
//
//         Cursor = Cursor.Parent!;
//
//         return isAcceptable ? t :  tree as T; // todo: some serious issue here because in case of ParseError node, it will return null. JavaVisitor returns J for every visit, so it's impossible to properly return a ParseError
//     }
//
//     public async Task Visit<T2>(IList<T2>? nodes, P p) where T2 : T
//     {
//         if (nodes != null)
//         {
//             foreach (T node in nodes)
//             {
//                 await Visit(node, p);
//             }
//         }
//     }
//
//     public async Task<T2?> VisitAndCast<T2>(Tree? tree, P p) where T2 : T
//     {
//         return (T2?)await Visit(tree, p);
//     }
//
//     public virtual async Task<Markers> VisitMarkers(Markers? markers, P p)
//     {
//         if (markers == null || ReferenceEquals(markers, Marker.Markers.EMPTY))
//         {
//             return Marker.Markers.EMPTY;
//         }
//
//         if (markers.MarkerList.Count == 0)
//         {
//             return markers;
//         }
//
//         return markers with
//         {
//             MarkerList = ListUtils.Map(markers.MarkerList, m => VisitMarker(m, p))!
//         };
//     }
//
//     Markers? ITreeVisitor<T, P>.VisitMarkers(Markers? markers, P p)
//     {
//         return this.VisitMarkers(markers, p).Result; // todo: don't leave like this
//     }
//
//     public virtual Core.Marker.Marker VisitMarker(Core.Marker.Marker marker, P p)
//     {
//         return marker;
//     }
//     T? ITreeVisitor<T, P>.Visit(Tree? tree, P p, Cursor parent) => Visit(tree, p, parent).Result;
//
//     public virtual async Task<T?> Visit(Tree? tree, P p, Cursor parent)
//     {
//         Cursor = parent;
//         return await Visit(tree, p);
//     }
// }