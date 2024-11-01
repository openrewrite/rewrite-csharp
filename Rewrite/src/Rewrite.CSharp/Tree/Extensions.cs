using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

internal static class Extensions
{



    public static JavaType? GetJavaType(Cs.DeclarationExpression expr)
    {
        // todo: not implemented properly
        return null;
    }
    public static Cs.DeclarationExpression WithJavaType(Cs.DeclarationExpression expr, JavaType newType)
    {
        // todo: not implemented properly
        return expr;
    }


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

    public static JavaType? GetJavaType(Cs.Argument expr)
    {
        return expr.Expression.Type;
    }

    public static Cs.Argument WithJavaType(Cs.Argument expr, JavaType newType)
    {
        return expr.WithExpression(expr.Expression.WithType<Expression>(newType));
    }

    public static JavaType? GetJavaType(Cs.SingleVariableDesignation expr)
    {
        return expr.Name.Type;
    }
    public static Cs.SingleVariableDesignation WithJavaType(Cs.SingleVariableDesignation expr, JavaType newType)
    {
        return expr.WithName(expr.Name.WithType(newType));
    }

    public static JavaType? GetJavaType(Cs.DiscardVariableDesignation expr)
    {
        return expr.Discard.Type;
    }
    public static Cs.DiscardVariableDesignation WithJavaType(Cs.DiscardVariableDesignation expr, JavaType newType)
    {
        return expr.WithDiscard(expr.Discard.WithType(newType));
    }

    public static JavaType? GetJavaType(Cs.TupleExpression expr)
    {
        return null;
    }

    public static Cs.TupleExpression WithJavaType(Cs.TupleExpression expr, JavaType newType)
    {
        return expr;
    }
}
