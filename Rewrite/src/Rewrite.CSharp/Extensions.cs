using System.Collections;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public static class Extensions
{
    public static IEnumerable<Core.Tree> Descendents<TNode>(this TNode source) where TNode : Core.Tree
    {
        var searchVisitor = new SearchVisitor();
        searchVisitor.Visit(source, null);
        return searchVisitor.Visited;
    }

    public static TRoot ReplaceNode<TRoot>(this TRoot root, J oldNode, J newNode)
        where TRoot : Core.Tree
    {
        var newRoot = new ReplaceVisitor(oldNode, newNode).Visit(root, null);
        return (TRoot)newRoot!;
    }

    private class ReplaceVisitor(J oldNode, J newNode) : CSharpVisitor<object?>
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

    private class SearchVisitor : CSharpVisitor<object?>
    {
        public List<Core.Tree> Visited { get; } = new ();
        public override J? PostVisit(Core.Tree tree, object? p)
        {

            Visited.Add(tree);
            return base.PostVisit(tree, p);
        }
    }
}
