using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using MyAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

public interface Expression<T> : Expression, TypedTree<T> where T : Expression
{
    TypedTree TypedTree.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
    Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
}
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface Expression :  TypedTree
{
    public new Expression WithType(JavaType? type);
    TypedTree TypedTree.WithType(JavaType? type) => WithType(type);


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
