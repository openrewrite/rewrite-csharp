using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface Expression : J
{
    public JavaType? Type => Extensions.GetJavaType(this);

    public T WithType<T>(JavaType? type) where T : J
    {
        return Extensions.WithType(this as dynamic, type);
    }

    IList<J> SideEffects => (Enumerable.Empty<J>() as IList<J>)!;

    // Expression Unwrap()
    // {
    //     return Unwrap(this)!;
    // }
    //
    // static Expression? Unwrap(Expression expr)
    // {
    //     return expr is J.Parentheses<J> { Tree: Expression tree } ? tree.Unwrap() : expr;
    // }

    // CoordinateBuilder.Expression getCoordinates();
}