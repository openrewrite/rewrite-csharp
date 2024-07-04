using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;

public static class Extensions
{
    public static JavaType? GetJavaType(J.AnnotatedType expr)
    {
        return expr.TypeExpression.Type;
    }

    public static JavaType? GetJavaType(J.Annotation expr)
    {
        return expr.AnnotationType.Type;
    }

    public static JavaType? GetJavaType(J.Empty expr)
    {
        return null;
    }

    public static JavaType? GetJavaType(J.ParenthesizedTypeTree expr)
    {
        return GetTypeFromParentheses(expr.ParenthesizedType);
    }

    public static JavaType? GetJavaType(J.IntersectionType expr)
    {
        return new JavaType.Intersection(expr.Bounds
            .Where(b => b != null)
            .Select(b => b.Type)
            .ToList());
    }

    public static JavaType? GetJavaType(J.MethodDeclaration expr)
    {
        return expr.MethodType?.ReturnType;
    }

    public static JavaType? GetJavaType(J.MethodInvocation expr)
    {
        return expr.MethodType?.ReturnType;
    }

    public static JavaType? GetJavaType(J.MultiCatch expr)
    {
        return new JavaType.MultiCatch(expr.Alternatives
            .Where(b => b != null)
            .Select(alt => alt.Type)
            .ToList());
    }

    public static JavaType? GetJavaType(J.NewClass expr)
    {
        return expr.ConstructorType?.ReturnType;
    }

    public static JavaType? GetJavaType(J.NullableType expr)
    {
        return expr.TypeTree.Type;
    }

    public static JavaType? GetJavaType<J2>(J.Parentheses<J2> expr) where J2 : J
    {
        return expr.Tree is Expression ? ((Expression)expr.Tree).Type :
            expr.Tree is NameTree ? ((NameTree)expr.Tree).Type :
            null;
    }

    public static JavaType? GetJavaType<J2>(J.ControlParentheses<J2> expr) where J2 : J
    {
        return expr.Tree is Expression ? ((Expression)expr.Tree).Type :
            expr.Tree is NameTree ? ((NameTree)expr.Tree).Type :
            null;
    }

    public static JavaType? GetJavaType(J.SwitchExpression expr)
    {
        var javaTypes = new JavaType?[1];
        new SwitchExpressionJavaVisitor().Visit(expr, javaTypes);
        return javaTypes[0];
    }

    public static JavaType? GetJavaType(J.TypeCast expr)
    {
        return GetTypeFromControlParentheses(expr.Clazz);
    }

    public static JavaType? GetJavaType(J.VariableDeclarations expr)
    {
        return expr.TypeExpression?.Type;
    }

    public static JavaType? GetJavaType(J.VariableDeclarations.NamedVariable expr)
    {
        return expr.VariableType?.Type;
    }

    public static JavaType? GetJavaType(J.Wildcard expr)
    {
        return null;
    }

    public static JavaType? GetJavaType(J.Unknown expr)
    {
        return null;
    }

    public static JavaType? GetJavaType(J? owner)
    {
        switch (owner)
        {
            case J.AnnotatedType at:
                return GetJavaType(at);
            case J.Annotation a:
                return GetJavaType(a);
            case J.Binary b:
                return b.JavaType;
            case J.ClassDeclaration classDeclaration:
                return classDeclaration.Type;
            case J.ParenthesizedTypeTree ptt:
                return GetJavaType(ptt);
            case J.IntersectionType it:
                return GetJavaType(it);
            case J.MethodDeclaration methodDeclaration:
                return GetJavaType(methodDeclaration);
            case J.MethodInvocation methodInvocation:
                return GetJavaType(methodInvocation);
            case J.MultiCatch multiCatch:
                return GetJavaType(multiCatch);
            case J.NewClass newClass:
                return GetJavaType(newClass);
            case J.NullableType nullableType:
                return GetJavaType(nullableType);
            case J.SwitchExpression switchExpression:
                return GetJavaType(switchExpression);
            case J.TypeCast typeCast:
                return GetJavaType(typeCast);
            case J.Unary unary:
                return unary.JavaType;
            case J.VariableDeclarations variableDeclarations:
                return GetJavaType(variableDeclarations);
            case J.VariableDeclarations.NamedVariable namedVariable:
                return GetJavaType(namedVariable);
        }

        if (IsParentheses(owner as dynamic))
        {
            return GetTypeFromParentheses(owner as dynamic);
        }

        if (IsControlParentheses(owner as dynamic))
        {
            return GetTypeFromControlParentheses(owner as dynamic);
        }

        return null;
    }


    private static bool IsParentheses<TJ2>(J.Parentheses<TJ2> parentheses) where TJ2 : J
    {
        return true;
    }

    private static JavaType? GetTypeFromParentheses<TJ2>(J.Parentheses<TJ2> parentheses) where TJ2 : J
    {
        return parentheses.Tree switch
        {
            Expression e => e.Type,
            NameTree nt => nt.Type,
            _ => null
        };
    }

    private static bool IsParentheses(object any)
    {
        return false;
    }

    private static JavaType? GetTypeFromParentheses(object parentheses)
    {
        return null;
    }

    private static bool IsControlParentheses<TJ2>(J.ControlParentheses<TJ2> parentheses) where TJ2 : J
    {
        return true;
    }

    private static JavaType? GetTypeFromControlParentheses<TJ2>(J.ControlParentheses<TJ2> parentheses) where TJ2 : J
    {
        return parentheses.Tree switch
        {
            Expression expression => expression.Type,
            NameTree nameTree => nameTree.Type,
            J.VariableDeclarations declarations => GetJavaType(declarations),
            _ => null
        };
    }

    private static bool IsControlParentheses(object any)
    {
        return false;
    }

    private static JavaType? GetTypeFromControlParentheses(object parentheses)
    {
        return null;
    }

    public static IList<T> Elements<T>(this IList<JRightPadded<T>> ls)
        where T : class
    {
        return ls.Select(e => e.Element).ToList();
    }

    public static IList<JRightPadded<T>> WithElements<T>(this IList<JRightPadded<T>> before, IList<T> elements)
        where T : J
    {
        // a cheaper check for the most common case when there are no changes
        if (elements.Count == before.Count)
        {
            var hasChanges = false;
            for (var i = 0; i < before.Count; i++)
            {
                if (!ReferenceEquals(before[i].Element, elements[i]))
                {
                    hasChanges = true;
                    break;
                }
            }

            if (!hasChanges)
            {
                return before;
            }
        }

        var after = new List<JRightPadded<T>>(elements.Count);
        var beforeById = before.ToDictionary(j => j.Element.Id, e => e);

        foreach (var t in elements)
        {
            if (beforeById.TryGetValue(t.Id, out var found))
            {
                after.Add(found.WithElement(t));
            }
            else
            {
                after.Add(new JRightPadded<T>(t, Space.EMPTY, Markers.EMPTY));
            }
        }

        return after;
    }

    public static J.ClassDeclaration.Kind.Type GetKind(this J.ClassDeclaration @class)
    {
        return @class.Padding.DeclarationKind.KindType;
    }

    public static J.ClassDeclaration WithKind(this J.ClassDeclaration @class, J.ClassDeclaration.Kind.Type kind)
    {
        return @class.GetKind() == kind
            ? @class
            : @class.Padding.WithDeclarationKind(@class.Padding.DeclarationKind.WithKindType(kind));
    }

    public static J.ClassDeclaration WithType(this J.ClassDeclaration @class, JavaType? type)
    {
        // FIXME type attribution
        return @class;
    }

    public static J.Literal WithType(this J.Literal literal, JavaType? javaType)
    {
        if (javaType == literal.Type)
        {
            return literal;
        }

        if (javaType is JavaType.Primitive primitive)
        {
            return new J.Literal(literal.Id, literal.Prefix, literal.Markers, literal.Value, literal.ValueSource,
                literal.UnicodeEscapes, primitive);
        }

        return literal;
    }

    public static J.Primitive WithType(this J.Primitive primitive, JavaType? newType)
    {
        if (newType == primitive.Type)
        {
            return primitive;
        }

        if (newType is not JavaType.Primitive type)
        {
            throw new ArgumentException("Cannot apply a non-primitive type to Primitive");
        }

        return new J.Primitive(primitive.Id, primitive.Prefix, primitive.Markers, type);
    }

    public static J.VariableDeclarations WithType(this J.VariableDeclarations variableDeclarations, JavaType? newType)
    {
        return variableDeclarations.TypeExpression == null
            ? variableDeclarations
            : variableDeclarations.WithTypeExpression(variableDeclarations.TypeExpression.WithType<TypeTree>(newType));
    }

    public static J.VariableDeclarations.NamedVariable WithType(this J.VariableDeclarations.NamedVariable namedVariable,
        JavaType? type)
    {
        return namedVariable.VariableType != null
            ? namedVariable.WithVariableType(namedVariable.VariableType.WithType(type))
            : namedVariable;
    }

    public static J WithType(object any, JavaType? javaType)
    {
        throw new NotSupportedException();
    }

    public static J.MethodDeclaration WithMethodType(this J.MethodDeclaration method, JavaType.Method? type)
    {
        // FIXME type attribution
        return method;
    }

    public static J.MethodInvocation WithMethodType(this J.MethodInvocation method, JavaType.Method? type)
    {
        // FIXME type attribution
        return method;
    }

    public static J.MethodInvocation WithName(this J.MethodInvocation method, J.Identifier name)
    {
        if (method.Name == name) return method;
        // FIXME add type attribution logic
        return new J.MethodInvocation(method.Id, method.Prefix, method.Markers, method.Padding.Select,
            method.Padding.TypeParameters, name, method.Padding.Arguments, method.MethodType);
    }

    public static J.MethodDeclaration WithTypeParameters(this J.MethodDeclaration method,
        IList<J.TypeParameter>? typeParameters)
    {
        var annotations = method.Annotations;
        if (typeParameters == null)
        {
            if (annotations.TypeParameters == null)
            {
                return method;
            }
            else
            {
                return annotations.WithTypeParameters(null);
            }
        }
        else
        {
            var currentTypeParameters = annotations.TypeParameters;
            if (currentTypeParameters == null)
            {
                return annotations.WithTypeParameters(new J.TypeParameters(Core.Tree.RandomId(), Space.EMPTY,
                    Markers.EMPTY,
                    [], typeParameters.Select(JRightPadded<J.TypeParameter>.Build).ToList()));
            }
            else
            {
                return annotations.WithTypeParameters(currentTypeParameters.WithParameters(typeParameters));
            }
        }
    }

    class SwitchExpressionJavaVisitor : JavaVisitor<JavaType?[]>
    {
        public override J VisitBlock(J.Block block, JavaType?[] javaType)
        {
            if (block.Statements.Count != 0)
            {
                var caze = (J.Case)block.Statements[0];
                javaType[0] = caze.Expressions[0].Type;
            }

            return block;
        }
    }

    public static J.AnnotatedType WithJavaType(J.AnnotatedType expr, JavaType newType)
    {
        return expr.WithTypeExpression(expr.TypeExpression.WithType<TypeTree>(newType));
    }

    public static J.Annotation WithJavaType(J.Annotation expr, JavaType newType)
    {
        return expr.WithAnnotationType(expr.AnnotationType.WithType<NameTree>(newType));
    }

    public static J.Empty WithJavaType(J.Empty expr, JavaType newType)
    {
        return expr;
    }

    public static J.ParenthesizedTypeTree WithJavaType(J.ParenthesizedTypeTree expr, JavaType newType)
    {
        return expr.WithParenthesizedType(expr.ParenthesizedType.WithType(newType));
    }

    public static J.IntersectionType WithJavaType(J.IntersectionType expr, JavaType newType)
    {
        return expr;
    }

    public static J.MethodInvocation WithJavaType(J.MethodInvocation expr, JavaType newType)
    {
        throw new NotImplementedException();
    }

    public static J.NewClass WithJavaType(J.NewClass expr, JavaType newType)
    {
        throw new NotImplementedException();
    }

    public static J.NullableType WithJavaType(J.NullableType expr, JavaType newType)
    {
        var rp = expr.Padding.TypeTree;
        var tt = rp.Element.WithType<TypeTree>(newType);
        return expr.Padding.WithTypeTree(rp.WithElement(tt));
    }

    public static J.Parentheses<J2> WithJavaType<J2>(J.Parentheses<J2> expr, JavaType newType) where J2 : J
    {
        return expr.Tree is Expression ? expr.WithTree(((Expression)expr.Tree).WithType<J2>(newType)) :
            expr.Tree is NameTree ? expr.WithTree(((NameTree)expr.Tree).WithType<J2>(newType)) :
            expr;
    }

    public static J.ControlParentheses<J2> WithJavaType<J2>(J.ControlParentheses<J2> expr, JavaType newType)
        where J2 : J
    {
        return expr.Tree is Expression ? expr.WithTree(((Expression)expr.Tree).WithType<J2>(newType)) :
            expr.Tree is NameTree ? expr.WithTree(((NameTree)expr.Tree).WithType<J2>(newType)) :
            expr;
    }

    public static J.SwitchExpression WithJavaType(J.SwitchExpression expr, JavaType newType)
    {
        return expr;
    }

    public static J.TypeCast WithJavaType(J.TypeCast expr, JavaType newType)
    {
        return expr.WithClazz(expr.Clazz.WithType(newType));
    }

    public static J.Wildcard WithJavaType(J.Wildcard expr, JavaType newType)
    {
        return expr;
    }

    public static J.Unknown WithJavaType(J.Unknown expr, JavaType newType)
    {
        return expr;
    }
}