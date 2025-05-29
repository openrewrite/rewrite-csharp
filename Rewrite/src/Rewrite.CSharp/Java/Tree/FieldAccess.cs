namespace Rewrite.RewriteJava.Tree;

public partial interface J
{
    public partial class FieldAccess
    {
        public string SimpleName => this.Name.SimpleName;
    }
}