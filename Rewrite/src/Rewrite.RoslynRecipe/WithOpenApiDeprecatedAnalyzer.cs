using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipe.Helpers;

namespace Rewrite.RoslynRecipe
{
    /// <summary>
    /// Analyzer that detects usage of the deprecated WithOpenApi extension method in ASP.NET Core 10.
    /// The WithOpenApi method is deprecated in favor of AddOpenApiOperationTransformer for Microsoft.AspNetCore.OpenApi,
    /// IOperationFilter for Swashbuckle, or IOperationProcessor for NSwag.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WithOpenApiDeprecatedAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0008";

        private static readonly LocalizableString Title = "WithOpenApi is deprecated in .NET 10";
        private static readonly LocalizableString MessageFormat = "WithOpenApi is deprecated and will be removed in a future release. Use AddOpenApiOperationTransformer instead.";
        private static readonly LocalizableString Description = "The WithOpenApi extension method is deprecated in .NET 10 Preview 7. " +
            "Its functionality has been replaced by the built-in OpenAPI document generation pipeline. " +
            "For Microsoft.AspNetCore.OpenApi, use AddOpenApiOperationTransformer. " +
            "For Swashbuckle, use IOperationFilter. " +
            "For NSwag, use IOperationProcessor.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/withopenapi-deprecated");

        /// <summary>
        /// Gets the list of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initializes the analyzer by registering analysis actions.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for invocation expressions to detect WithOpenApi method calls
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        /// <summary>
        /// Analyzes an invocation expression to determine if it's a call to the deprecated WithOpenApi method.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Check if this is a member access expression (e.g., something.WithOpenApi())
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            // Check if the method name is WithOpenApi
            if (memberAccess.Name.Identifier.Text != "WithOpenApi")
                return;


            if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                    "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1",
                    "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1(System.Func{Microsoft.OpenApi.OpenApiOperation,Microsoft.OpenApi.OpenApiOperation})",
                    "M:Microsoft.AspNetCore.Builder.OpenApiEndpointConventionBuilderExtensions.WithOpenApi``1(System.Func{Microsoft.OpenApi.Models.OpenApiOperation,Microsoft.OpenApi.Models.OpenApiOperation})"
                ))
                return;

            var diagnosticLocation = memberAccess.Name.GetLocation();
                var diagnosticToReport = Diagnostic.Create(Rule, diagnosticLocation);
                context.ReportDiagnostic(diagnosticToReport);
            
        }
    }
}