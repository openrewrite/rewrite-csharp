using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public static class Extensions
{
    public static bool IsPresent(this SyntaxToken token) => !token.IsKind(SyntaxKind.None);
    public static IEnumerable<Core.Tree> Descendents<TNode>(this TNode source) where TNode : Core.Tree
    {
        var result = new List<Core.Tree>();
        var searchVisitor = new SearchVisitor();
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
        public override J? PreVisit(Core.Tree? tree, object? p)
        {
            if (oldNode.Equals(tree))
            {
                return newNode;
            }
            return base.PreVisit(tree, p);
        }
    }

    private class SearchVisitor : CSharpVisitor<List<Core.Tree>>
    {
        public override J? PreVisit(Core.Tree? tree, List<Core.Tree> p)
        {

            p.Add(tree!);
            return base.PreVisit(tree!, p);
        }
    }
}
