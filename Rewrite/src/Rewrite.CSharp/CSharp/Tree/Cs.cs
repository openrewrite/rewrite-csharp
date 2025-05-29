using System.Runtime.CompilerServices;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public partial interface Cs
{
    public new partial class CompilationUnit : CSharpSourceFile
    {}

    Cursor? Core.Tree.Find(Predicate<Core.Tree> predicate)
    {
        var result = new object();
        var searchVisitor = new SearchVisitor(predicate);
        if (!IsAcceptable(searchVisitor, result))
            return null;
        searchVisitor.Visit(this, result);
        return searchVisitor.SearchResult;
    }
    // public new Cursor? Find<TNode>(Predicate<Core.Tree> predicate) where TNode : Core.Tree
    
    
    internal class SearchVisitor(Predicate<Core.Tree> shouldStop) : CSharpVisitor<object>
    {
        private bool _stop = false;
        public Cursor? SearchResult { get; private set; }
        public override J? Visit(Core.Tree? tree, object p, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(tree))] string callingArgumentExpression = "")
        {
            if (_stop)
            {
                return (J?)tree;
            }
            if (tree != null && shouldStop(tree))
            {
                SearchResult = new Cursor(Cursor, tree);
                _stop = true;
                return (J)tree;
            }

            return base.Visit(tree, p);
        }
    }
}
