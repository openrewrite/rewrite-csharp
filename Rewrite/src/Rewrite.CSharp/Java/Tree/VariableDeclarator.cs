using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava.Tree;

public partial interface VariableDeclarator : TypedTree 
{
    public List<J.Identifier> Names { get; }
}