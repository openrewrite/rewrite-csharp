using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Rewrite.RoslynRecipes;

public static class SymbolAnnotation
{
    public const string Kind = "SymbolId";

    public static SyntaxAnnotation Create(string documentationCommentId)
        => new(Kind, documentationCommentId);
    public static SyntaxAnnotation Create(ISymbol symbol)
        => new(Kind, DocumentationCommentId.CreateReferenceId(symbol));

    public static ISymbol? GetSymbol(SyntaxAnnotation annotation, Compilation compilation)
        => GetSymbols(annotation, compilation).FirstOrDefault();

    public static ImmutableArray<ISymbol> GetSymbols(SyntaxAnnotation annotation, Compilation compilation)
        => annotation.Data is null
            ? []
            : DocumentationCommentId.GetSymbolsForReferenceId(annotation.Data, compilation);
}
