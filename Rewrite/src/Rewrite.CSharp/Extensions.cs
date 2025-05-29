using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public static class Extensions
{
    public static bool IsPresent(this SyntaxToken token) => !token.IsKind(SyntaxKind.None);

    public static bool TryFind<TNode, TResult>(this TNode source, Predicate<Core.Tree> predicate, out Cursor<TResult>? result) where TNode : Core.Tree where TResult : Core.Tree 
    {
        result = source.Find(predicate)?.As<TResult>();
        return result != null;
    }

    public static Cursor? Find(this Cs source, Predicate<J> predicate)
    {
        return source.Find(tree => predicate((J)tree));
    }
    public static Cursor<TResult>? Find<TResult>(this J source, Predicate<TResult>? predicate = null) where TResult : J
    {
        Predicate<Core.Tree> untypedPredicate = predicate is null ? (x => x is TResult) :  (x => x is TResult node && predicate(node));
        return source.Find(untypedPredicate)?.As<TResult>();
    }
    // public static Cursor? Find(this Cs source, Predicate<Core.Tree> predicate)
    // {
    //     
    //     // var result = new object();
    //     // var searchVisitor = new SearchVisitor(predicate);
    //     // if (!source.IsAcceptable(searchVisitor, result))
    //     //     return null;
    //     // searchVisitor.Visit(source, result);
    //     // return searchVisitor.SearchResult;
    // }
    public static IEnumerable<Core.Tree> Descendents<TNode>(this TNode source, Predicate<Core.Tree>? shouldStop = null) where TNode : Core.Tree
    {
        var result = new List<Core.Tree>();
        shouldStop ??= _ => false;
        var searchVisitor = new DescendentsVisitor(shouldStop);
        if (!source.IsAcceptable(searchVisitor, result))
            return new Core.Tree[] { source };
        searchVisitor.Visit(source, result);
        return result;
    }

    public static TRoot ReplaceNode<TRoot>(this TRoot root, J oldNode, J newNode)
        where TRoot : Core.Tree
    {
        var newRoot = new ReplaceNodeVisitor(oldNode, newNode).Visit(root, null);
        return (TRoot)newRoot!;
    }


    private class ReplaceContainerVisitor(J oldNode, J newNode) : CSharpVisitor<object?>
    {
        public override J? PreVisit(Core.Tree? tree, object? p)
        {
            if (oldNode.Equals(tree))
            {
                return newNode;
            }
            return base.PreVisit(tree, p);
        }
    }


    private class ReplaceNodeVisitor(J oldNode, J newNode) : CSharpVisitor<object?>
    {
        bool _stop = false;
        public override J? PreVisit(Core.Tree? tree, object? p)
        {
            if (oldNode.Equals(tree))
            {
                _stop = true;
                return newNode;
            }
            return base.PreVisit(tree, p);
        }

        public override J? Visit(Core.Tree? tree, object? p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            if (_stop)
                return (J?)tree;
            return base.Visit(tree, p);
        }
        
    }

    
    private class DescendentsVisitor(Predicate<Core.Tree> shouldStop) : CSharpVisitor<List<Core.Tree>>
    {
        // public override J? PreVisit(Core.Tree? tree, List<Core.Tree> p)
        // {
        //
        //     
        //     return base.PreVisit(tree!, p);
        // }

        public override J? Visit(Core.Tree? tree, List<Core.Tree> p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            p.Add(tree!);
            if (tree != null && shouldStop(tree))
                return (J)tree;
            return base.Visit(tree, p);
        }
    }
}
