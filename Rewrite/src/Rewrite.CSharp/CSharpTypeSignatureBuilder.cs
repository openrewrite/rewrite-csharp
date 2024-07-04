using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Rewrite.RewriteJava;

namespace Rewrite.RewriteCSharp;

internal class CSharpTypeSignatureBuilder : JavaTypeSignatureBuilder<ISymbol>
{
    public string Signature(ISymbol type)
    {
        if (type is ITypeSymbol)
            return type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
        throw new NotImplementedException();
    }

    public string ArraySignature(ISymbol type)
    {
        throw new NotImplementedException();
    }

    public string ClassSignature(ISymbol type)
    {
        return type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
    }

    public string GenericSignature(ISymbol type)
    {
        throw new NotImplementedException();
    }

    public string ParameterizedSignature(ISymbol type)
    {
        throw new NotImplementedException();
    }

    public string PrimitiveSignature(ISymbol type)
    {
        throw new NotImplementedException();
    }

    public string MethodSignature(IMethodSymbol methodSymbol)
    {
        var containingSymbol = methodSymbol.ContainingSymbol;
        string owner;
        if (containingSymbol is INamedTypeSymbol nt)
            owner = ClassSignature(nt);
        else
            owner = Signature(containingSymbol);
        return owner + "." + methodSymbol.Name + '(' + string.Join(", ", methodSymbol.Parameters.Select(p => Signature(p.Type))) + ')';
    }

    public string ParameterSignature(IParameterSymbol parameterSymbol)
    {
        var containingSymbol = parameterSymbol.ContainingSymbol;
        while (containingSymbol is IMethodSymbol { MethodKind: MethodKind.AnonymousFunction } or IMethodSymbol { MethodKind: MethodKind.LocalFunction } or IFieldSymbol)
        {
            containingSymbol = containingSymbol.ContainingSymbol;
        }
        
        string owner;
        if (containingSymbol is IMethodSymbol ms)
        {
            owner = MethodSignature(ms);
        }
        else
            owner = Signature(containingSymbol);
        return owner + '{' + parameterSymbol.Name + ',' + Signature(parameterSymbol.Type) + '}';
    }
}