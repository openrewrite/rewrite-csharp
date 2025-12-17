using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.RoslynRecipe.Helpers;

public static class SymbolExtensions
{
    extension(ITypeSymbol symbol)
    {
        public IdentifierNameSyntax ToIdentifierName() => SyntaxFactory.IdentifierName(symbol.ToParentTypeQualifiedString());

        public string ToParentTypeQualifiedString() => 
            string.Join("", symbol.ToDisplayParts().Where(x => x.Kind != SymbolDisplayPartKind.NamespaceName)).Trim('.');
    }
}