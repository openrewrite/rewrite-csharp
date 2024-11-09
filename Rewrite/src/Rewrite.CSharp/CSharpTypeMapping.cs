using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public partial class CSharpTypeMapping : JavaTypeMapping<ISymbol>
{
    private readonly JavaTypeCache _typeCache = new();
    private readonly CSharpTypeSignatureBuilder _signatureBuilder = new();

    public JavaType Type(ISymbol? roslynSymbol)
    {
        switch (roslynSymbol)
        {
            case IArrayTypeSymbol arrayType:
                return Type(arrayType);
            case IMethodSymbol methodSymbol:
                return Type(methodSymbol);
            case INamedTypeSymbol typeSymbol:
                return Type(typeSymbol);
            case IParameterSymbol { IsParams: true }:
                // return new JavaType.Array()
                return default!;
            case IParameterSymbol parameterSymbol:
                return Type(parameterSymbol);
            case ILocalSymbol localSymbol:
                return new JavaType.Variable(
                    null,
                    0,
                    roslynSymbol.Name,
                    null,
                    Type(localSymbol.Type),
                    MapAttributes(localSymbol.GetAttributes())
                );
            case IFieldSymbol fieldSymbol:
                return new JavaType.Variable(
                    null,
                    0,
                    roslynSymbol.Name,
                    null,
                    Type(fieldSymbol.Type),
                    MapAttributes(fieldSymbol.GetAttributes())
                );
        }

        return default!;
    }

    private JavaType Type(IArrayTypeSymbol arrayType)
    {
        var fullyQualifiedName = arrayType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
        var javaType = _typeCache[fullyQualifiedName];

        if (javaType != null)
            return javaType;

        var type = new JavaType.Array();
        _typeCache[fullyQualifiedName] = type;

        type.UnsafeSet(
            Type(arrayType.ElementType),
            MapAttributes(arrayType.GetAttributes())
        );
        return type;
    }

    private JavaType Type(IMethodSymbol methodSymbol)
    {
        return new JavaType.Method(
            null,
            0,
            Type(methodSymbol.ReceiverType) as JavaType.FullyQualified,
            methodSymbol.Name,
            Type(methodSymbol.ReturnType),
            methodSymbol.Parameters.Select(p => p.Name).ToList(),
            methodSymbol.Parameters.Select(Type).ToList(),
            null,
            null,
            null
        );
    }

    private JavaType Type(INamedTypeSymbol typeSymbol)
    {
        switch (typeSymbol.SpecialType)
        {
            case SpecialType.System_Boolean:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Boolean);
            case SpecialType.System_Byte:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Byte);
            case SpecialType.System_Int16:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Short);
            case SpecialType.System_Int32:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Int);
            case SpecialType.System_Int64:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Long);
            case SpecialType.System_Single:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Float);
            case SpecialType.System_Double:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Double);
            case SpecialType.System_String:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.String);
            case SpecialType.System_Void:
                return new JavaType.Primitive(JavaType.Primitive.PrimitiveType.Void);
            // These primitive types don't have a counterpart in Java
            case SpecialType.System_SByte:
            case SpecialType.System_UInt16:
            case SpecialType.System_UInt32:
            case SpecialType.System_UInt64:
            case SpecialType.System_Decimal:
            case SpecialType.System_IntPtr:
            case SpecialType.System_UIntPtr:
                return new JavaType.Class(
                    null,
                    0,
                    typeSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat),
                    GetKind(typeSymbol),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
        }

        var fullyQualifiedName = typeSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
        var javaType = _typeCache[fullyQualifiedName];

        if (javaType != null)
            return javaType;

        var clazz = new JavaType.Class(
            null,
            0,
            fullyQualifiedName,
            GetKind(typeSymbol),
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        _typeCache[fullyQualifiedName] = clazz;

        clazz.UnsafeSet(
            typeSymbol.TypeArguments.Select(Type).ToList(),
            null,
            typeSymbol.ContainingType != null ? Type(typeSymbol.ContainingType) as JavaType.FullyQualified : null,
            null,
            null,
            null,
            null
        );

        return clazz;
    }

    private JavaType Type(IParameterSymbol parameterSymbol)
    {
        var signature = _signatureBuilder.ParameterSignature(parameterSymbol);

        if (_typeCache[signature] is JavaType.Variable javaType)
            return javaType;

        var type = new JavaType.Variable(
            null,
            0,
            parameterSymbol.Name,
            null,
            Type(parameterSymbol.Type),
            null
        );

        _typeCache[signature] = type;

        type.UnsafeSet(
            null,
            Type(parameterSymbol.Type),
            null
        );
        return type;
    }

    private JavaType.FullyQualified.TypeKind GetKind(ITypeSymbol sym)
    {
        switch (sym.TypeKind)
        {
            case TypeKind.Enum:
                return JavaType.FullyQualified.TypeKind.Enum;
            case TypeKind.Interface:
                return JavaType.FullyQualified.TypeKind.Interface;
            case TypeKind.Class:
                return JavaType.FullyQualified.TypeKind.Class;
            case TypeKind.Struct:
                return JavaType.FullyQualified.TypeKind.Value;
        }

        return sym.IsRecord ? JavaType.FullyQualified.TypeKind.Record : JavaType.FullyQualified.TypeKind.Class;
    }

    private IList<JavaType.FullyQualified>? MapAttributes(ImmutableArray<AttributeData> attributeData)
    {
        return attributeData.Length == 0 ? null : [];
    }
}
