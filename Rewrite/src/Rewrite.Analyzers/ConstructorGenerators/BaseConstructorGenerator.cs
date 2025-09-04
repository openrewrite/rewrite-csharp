using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Analyzers.Authoring;
using Rewrite.Analyzers.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Rewrite.Analyzers.ConstructorGenerators;

/// <summary>
/// Base class for source generators which generate constructors.
/// </summary>
internal abstract class BaseConstructorGenerator : IIncrementalGenerator
{
	/// <summary>
	/// Initializes this generator.
	/// </summary>
	/// <param name="context"></param>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{

		var sources = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeName, IsCandidate, Transform);
		context.AddSources(sources);
	}

	private bool IsCandidate(SyntaxNode node, CancellationToken cancellationToken)
	{
		return node is ClassDeclarationSyntax or StructDeclarationSyntax;
	}

	private GeneratorResult Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
	{
		var typeDeclaration = (TypeDeclarationSyntax)context.TargetNode;
        
		if (!typeDeclaration.TryValidateType(out var @namespace, out var diagnostic))
		{
			return new GeneratorResult(diagnostic!);
		}
        
        var parts = GetAllTypeParts(typeDeclaration, context.SemanticModel, cancellationToken);
		var (modifier, constructorParameters, constructorBody) = GetConstructorParts(parts, context.Attributes[0], context.SemanticModel);
		// Dirty.
		if (constructorParameters.Parameters.Count == 0 && AttributeName != "Rewrite.Analyzers.ConstructorGenerators")
		{
			// No members were found to generate a constructor for.
			return GeneratorResult.Empty;
		}

		cancellationToken.ThrowIfCancellationRequested();

        var constructor = CreateConstructorCode(typeDeclaration, modifier, constructorParameters, constructorBody);
        constructor = constructor.NormalizeWhitespace();
        var sourceText = SourceText.From( PartialTypeModel.RenderPartialBody(typeDeclaration, constructor));
        
        

		return new GeneratorResult(@typeDeclaration.GetGeneratorQualifiedSourceFileName(this), sourceText);
	}
    
    public IReadOnlyList<TypeDeclarationSyntax> GetAllTypeParts(
        BaseTypeDeclarationSyntax typeDecl,
        SemanticModel semanticModel,
        CancellationToken cancellationToken = default)
    {
        var symbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken) as INamedTypeSymbol;
        if (symbol is null)
            return new List<TypeDeclarationSyntax>();

        var parts = new List<TypeDeclarationSyntax>(symbol.DeclaringSyntaxReferences.Length);
        foreach (var syntaxRef in symbol.DeclaringSyntaxReferences)
        {
            var node = syntaxRef.GetSyntax(cancellationToken);
            if (node is TypeDeclarationSyntax b)
                parts.Add(b);
        }
        return parts;
    }

    /// <summary>
    /// Gets the to-be-generated constructor's parameters as well as its body.
    /// </summary>
    /// <param name="typeDeclaration">The type declaration to generate the parts for.</param>
    /// <param name="attribute">The attribute declared on the type.</param>
    /// <param name="contextSemanticModel"></param>
    /// <returns>The constructor's parameters and its body.</returns>
    protected abstract (SyntaxKind modifier, ParameterListSyntax constructorParameters, BlockSyntax constructorBody) GetConstructorParts<T>(
        IReadOnlyCollection<T> typeDeclaration,
        AttributeData attribute,
        SemanticModel contextSemanticModel) where T : TypeDeclarationSyntax;

	/// <summary>
	/// The name of the target attribute. Should be the result of "typeof(NoArgsConstructorAttribute).FullName".
	/// </summary>
	protected abstract string AttributeName { get; }

	private static ConstructorDeclarationSyntax CreateConstructorCode(TypeDeclarationSyntax typeDeclaration, SyntaxKind modifier, ParameterListSyntax constructorParameters, BlockSyntax constructorBody)
	{
		return ConstructorDeclaration(typeDeclaration.Identifier.Text)
			.WithParameterList(constructorParameters)
			.WithBody(constructorBody)
			.WithModifiers(TokenList(Token(modifier)));

	}
}