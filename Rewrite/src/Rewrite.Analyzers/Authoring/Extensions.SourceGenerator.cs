using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
namespace MyProject.Analyzers.Authoring;

public static partial class Extensions
{
    // public static IncrementalValuesProvider<TModel> CreatePartialTypeSyntaxProvider<TNodeType, TModel>(
    //     this SyntaxValueProvider syntaxValueProvider,
    //     Func<GeneratorSyntaxContext<TNodeType>, CancellationToken, TModel> transform) 
    //     where TNodeType : SyntaxNode
    // {
    //     return syntaxValueProvider.CreateSyntaxProvider(
    //         predicate: (node, _) => node is MemberDeclarationSyntax cds && cds.Modifiers.Any(m => m.IsKind(PartialKeyword)),
    //         transform: (syntaxContext, cancellationToken) => transform(new GeneratorSyntaxContext<TNodeType>(syntaxContext), cancellationToken));
    // }
    // public static IncrementalValuesProvider<TModel> CreateSyntaxProvider<TNodeType, TModel>(
    //     this SyntaxValueProvider syntaxValueProvider,  
    //     Func<GeneratorSyntaxContext<TNodeType>,CancellationToken,TModel> transform) 
    //     where TNodeType : SyntaxNode
    // {
    //     return syntaxValueProvider.CreateSyntaxProvider(
    //         predicate: (node, _) => node is TNodeType,
    //         transform: (syntaxContext, cancellationToken) => transform(new GeneratorSyntaxContext<TNodeType>(syntaxContext), cancellationToken));
    // }

}

// public readonly struct GeneratorSyntaxContext<T>(GeneratorSyntaxContext context) where T : SyntaxNode
// {
//     private GeneratorSyntaxContext Context { get; } = context;
//     public SemanticModel SemanticModel => Context.SemanticModel;
//     public T Node => (T)Context.Node;
// }