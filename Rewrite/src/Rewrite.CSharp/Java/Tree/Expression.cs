using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using MyAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

// public interface Expression<T> : Expression, TypedTree<T> where T : Expression
// {
//     J J.WithPrefix(Space space) => ((J<T>)this).WithPrefix(space);
//     Expression Expression.WithPrefix(Space space) => ((J<T>)this).WithPrefix(space);
//     TypedTree TypedTree.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
//     Expression Expression.WithType(JavaType? type) => ((TypedTree<T>)this).WithType(type);
// }
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public partial interface Expression :  TypedTree
{
    // public new Expression WithType(JavaType? type);
    // TypedTree TypedTree.WithType(JavaType? type) => WithType(type);

    // public new Expression WithPrefix(Space padding) => (Expression)((J)this).WithPrefix(padding);
    // J J.WithPrefix(Space padding) => WithPrefix(padding);

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
