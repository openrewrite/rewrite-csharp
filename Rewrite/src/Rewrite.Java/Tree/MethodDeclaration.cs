namespace Rewrite.RewriteJava.Tree;

public partial interface J : Rewrite.Core.Tree
{
    public partial class MethodDeclaration
    {
        public Identifier Name => _name.Identifier;
    }
}
