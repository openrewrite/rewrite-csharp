namespace Rewrite.RewriteCSharp.Tree;

public partial interface Cs<T> : Cs
{
}

public partial interface Cs
{
    public new partial class CompilationUnit : MutableSourceFile<CompilationUnit>
    {
        Core.Tree Core.Tree.WithId(Guid id) => WithId(id);
    }
}
