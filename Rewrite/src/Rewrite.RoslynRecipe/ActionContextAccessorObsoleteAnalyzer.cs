using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipe.Helpers;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rewrite.RoslynRecipe;

/// <summary>
/// Analyzes C# code for usage of the obsolete IActionContextAccessor interface in constructor parameters
/// and provides diagnostics for migration to IHttpContextAccessor.
/// </summary>
/// <remarks>
/// This analyzer detects when IActionContextAccessor is used as a constructor parameter,
/// as this interface is obsolete in .NET 10.0 and should be replaced with IHttpContextAccessor.
/// See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/iactioncontextaccessor-obsolete
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ActionContextAccessorObsoleteAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic ID for this analyzer.
    /// </summary>
    public const string DiagnosticId = "ORNETX0009";

    private static readonly LocalizableString Title = "IActionContextAccessor is obsolete in .NET 10";

    private static readonly LocalizableString MessageFormat =
        "IActionContextAccessor '{0}' is obsolete and should be replaced with IHttpContextAccessor";

    private static readonly LocalizableString Description =
        "The IActionContextAccessor interface is obsolete in .NET 10.0. " +
        "Use IHttpContextAccessor instead. The ActionContext can be retrieved via HttpContext.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>(). " +
        "See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/iactioncontextaccessor-obsolete";

    private const string Category = "Modernization";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    /// <summary>
    /// Gets the supported diagnostics for this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions.
    /// </summary>
    /// <param name="context">The analysis context to register actions with. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public override void Initialize(AnalysisContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeParameter, SyntaxKind.Parameter);
    }

    /// <summary>
    /// Analyzes a parameter syntax node to determine if it represents an IActionContextAccessor
    /// constructor parameter that should be replaced.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the parameter to analyze.</param>
    private static void AnalyzeParameter(SyntaxNodeAnalysisContext context)
    {
        var parameter = (ParameterSyntax)context.Node;

        // Check if this parameter is in a constructor
        if (parameter.Parent?.Parent is not ConstructorDeclarationSyntax)
            return;

        // Check if the parameter has a type
        if (parameter.Type == null)
            return;

        // Use IsSymbolOneOf to check if the parameter type is IActionContextAccessor
        if (!parameter.Type.IsSymbolOneOf(context.SemanticModel,
                "T:Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor"))
            return;

        // Get the parameter symbol for later use
        var semanticModel = context.SemanticModel;
        var parameterSymbol = semanticModel.GetDeclaredSymbol(parameter, context.CancellationToken);

        if (parameterSymbol == null)
            return;

        // Check if ModelState is used anywhere in the containing type
        // If so, we'll need special handling in the code fix
        var containingType = parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (containingType != null)
        {
            var hasModelStateUsage = CheckForModelStateUsage(containingType, semanticModel, context.CancellationToken);

            // Create diagnostic with custom property to indicate ModelState usage
            var properties = hasModelStateUsage
                ? ImmutableDictionary<string, string?>.Empty.Add("HasModelStateUsage", "true")
                : ImmutableDictionary<string, string?>.Empty;

            var diagnostic = Diagnostic.Create(
                Rule,
                parameter.Type?.GetLocation() ?? parameter.GetLocation(),
                properties,
                parameterSymbol.Name);

            context.ReportDiagnostic(diagnostic);
        }
    }

    /// <summary>
    /// Checks if ActionContext.ModelState is used within the containing type.
    /// </summary>
    /// <param name="containingType">The type declaration to check for ModelState usage.</param>
    /// <param name="semanticModel">The semantic model for the compilation.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>True if ModelState is accessed on ActionContext; otherwise, false.</returns>
    private static bool CheckForModelStateUsage(
        TypeDeclarationSyntax containingType,
        SemanticModel semanticModel,
        System.Threading.CancellationToken cancellationToken)
    {
        // Look for any member access expressions that access ModelState on ActionContext
        var memberAccesses = containingType.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Where(m => m.Name.Identifier.Text == "ModelState");

        foreach (var memberAccess in memberAccesses)
        {
            // Check if the expression is accessing ActionContext.ModelState
            if (memberAccess.Expression is MemberAccessExpressionSyntax parentAccess &&
                parentAccess.Name.Identifier.Text == "ActionContext")
            {
                // Verify through semantic model that this is indeed ActionContext.ModelState
                var symbolInfo = semanticModel.GetSymbolInfo(memberAccess, cancellationToken);
                if (symbolInfo.Symbol != null)
                {
                    var symbolKey = symbolInfo.Symbol.GetDocumentationCommentId();
                    if (symbolKey == "P:Microsoft.AspNetCore.Mvc.ActionContext.ModelState")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}