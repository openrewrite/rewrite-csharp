namespace Rewrite.RewriteCSharp.Tree;

public partial interface CSharpSourceFile : Cs, MutableSourceFile
{
    public IList<Statement> Members { get; }
    public IList<UsingDirective> Usings => Members.OfType<UsingDirective>().ToList();
}
