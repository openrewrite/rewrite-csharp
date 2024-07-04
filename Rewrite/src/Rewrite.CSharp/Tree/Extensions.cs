using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

internal static class Extensions
{
    public static JavaType? GetJavaType(Cs.ArrayRankSpecifier expr)
    {
        return expr.Sizes.Count == 0 ? null : expr.Sizes[0].Type;
    }

    public static JavaType? GetJavaType(Cs.StatementExpression expr)
    {
        return null;
    }

    public static JavaType? GetJavaType(Cs.NullSafeExpression expr)
    {
        return expr.Expression.Type;
    }

    public static Cs.ArrayRankSpecifier WithJavaType(Cs.ArrayRankSpecifier expr, JavaType newType)
    {
        throw new NotImplementedException();
    }

    public static Cs.NullSafeExpression WithJavaType(Cs.NullSafeExpression expr, JavaType newType)
    {
        var newExpression = expr.Expression.WithType<Expression>(newType);
        if (newExpression == expr.Expression) return expr;

        return new Cs.NullSafeExpression(expr.Id, expr.Prefix, expr.Markers,
            expr.Padding.Expression.WithElement(newExpression));
    }

    public static Cs.StatementExpression WithJavaType(Cs.StatementExpression expr, JavaType newType)
    {
        return expr;
    }
}