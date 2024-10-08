using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

internal static class Extensions
{
    public static JavaType? GetJavaType(Cs.Lambda expr)
    {
        return expr.LambdaExpression.Type;
    }
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

    public static Cs.Lambda WithJavaType(Cs.Lambda expr, JavaType newType)
    {
        return new Cs.Lambda(
            expr.Id,
            expr.Prefix,
            expr.Markers,
            expr.LambdaExpression.WithType(newType),
            expr.Modifiers
        );
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

    public static JavaType? GetJavaType(Cs.InterpolatedString expr)
    {
        return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.String);
    }

    public static Cs.InterpolatedString WithJavaType(Cs.InterpolatedString expr, JavaType newType)
    {
        return expr;
    }

    public static JavaType? GetJavaType(Cs.Interpolation expr)
    {
        return expr.Type;
    }

    public static Cs.Interpolation WithJavaType(Cs.Interpolation expr, JavaType newType)
    {
        return expr.WithExpression(expr.Expression.WithType<Expression>(newType));
    }

    public static JavaType? GetJavaType(Cs.NamedArgument expr)
    {
        return expr.Type;
    }

    public static Cs.NamedArgument WithJavaType(Cs.NamedArgument expr, JavaType newType)
    {
        return expr.WithExpression(expr.Expression.WithType<Expression>(newType));
    }
}
