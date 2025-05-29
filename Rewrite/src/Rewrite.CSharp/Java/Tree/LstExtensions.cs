using Rewrite.RewriteJava.Tree;

namespace Rewrite.Java.Tree;

public static class LstExtensions
{
    public static bool IsModifying(this J.Unary.Types type) => type switch
    {
        J.Unary.Types.PreIncrement => true,
        J.Unary.Types.PreDecrement => true,
        J.Unary.Types.PostIncrement => true,
        J.Unary.Types.PostDecrement => true,
        _ => false
    };
}