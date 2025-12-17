using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.RoslynRecipe.Helpers;

public static class SemanticModelExtensions
{
    public static IEnumerable<Location> FindLocations(this SemanticModel model, ISymbol symbol, SyntaxNode? scope = null)
    {
        if (scope == null)
            scope = model.SyntaxTree.GetCompilationUnitRoot();
        return scope.DescendantTokens()
            .Where(x =>
            {
                if (symbol == null)
                {
                    Debugger.Break();
                }
                return TokenMaybeRelatedToSymbol(x, symbol!.Name) && SymbolEqualityComparer.Default.Equals(model.GetTypeSymbol(x), symbol);
            })
            .Select(token => 
            {
                if(symbol is ITypeSymbol)
                {
                    return GetTypeSyntaxNode(token)!.GetLocation();
                }
                return token.GetLocation();
            });
    }
    
    private static TypeSyntax? GetTypeSyntaxNode(SyntaxToken token)
    {
        // Find the first TypeSyntax ancestor
        var typeSyntax = token.Parent as TypeSyntax;
        if (typeSyntax == null) return null;

        // Keep going up while parent is also a TypeSyntax (to find root QualifiedNameSyntax)
        while (typeSyntax.Parent is QualifiedNameSyntax parentType)
        {
            typeSyntax = parentType;
        }

        return typeSyntax;
    }
    
    
    private static ISymbol? GetTypeSymbol(this SemanticModel model, SyntaxToken token)
    {
        var parent = token.Parent;
        if (parent == null) return null;

        // Try to get symbol info first

        if (parent is MemberDeclarationSyntax or VariableDeclaratorSyntax or ParameterSyntax)
        {
            return model.GetDeclaredSymbol(parent);
        }
        return model.GetSymbolInfo(parent).Symbol;

    }
    private static bool TokenMaybeRelatedToSymbol(SyntaxToken token, string symbol)
    {
        var name = token.Text;
        if(!token.IsKind(SyntaxKind.IdentifierToken) && !token.IsKeyword())
            return false;
            
        
        if (token.Parent is PredefinedTypeSyntax)
        {
            name = token.Text switch
            {
                "string" => "String",
                "int" => "Int32",
                "bool" => "Boolean",
                "byte" => "Byte",
                "sbyte" => "SByte",
                "char" => "Char",
                "decimal" => "Decimal",
                "double" => "Double",
                "float" => "Single",
                "uint" => "UInt32",
                "long" => "Int64",
                "ulong" => "UInt64",
                "short" => "Int16",
                "ushort" => "UInt16",
                "object" => "Object",
                _ => token.Text
            };
        }
        return name == symbol;
    }
}