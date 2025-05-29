using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteJava.Tree;

public interface VariableDeclarator : TypedTree 
{
    public List<J.Identifier> Names { get; }
}