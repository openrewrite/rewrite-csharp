using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzer to detect FilePatternMatch constructor calls with null stem parameter.
    /// In .NET 10, the stem parameter is non-nullable and passing null throws ArgumentNullException.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FilePatternMatchStemAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0005";

        private static readonly LocalizableString Title = "FilePatternMatch.Stem is non-nullable in .NET 10";
        private static readonly LocalizableString MessageFormat = "FilePatternMatch constructor 'stem' parameter cannot be null in .NET 10. This will throw ArgumentNullException at runtime.";
        private static readonly LocalizableString Description = "In .NET 10, FilePatternMatch.Stem changed from nullable to non-nullable. " +
            "The constructor now throws ArgumentNullException if stem is null. Previous nullability annotations were inaccurate. " +
            "Review usage and ensure the stem argument is never null. Remove any nullability warning suppressions.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/filepatternmatch-stem-nonnullable");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for object creation expressions
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        }

        private void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            var objectCreation = (ObjectCreationExpressionSyntax)context.Node;

            // Get the symbol for the constructor being called
            var symbolInfo = context.SemanticModel.GetSymbolInfo(objectCreation);
            if (symbolInfo.Symbol is not IMethodSymbol constructorSymbol)
                return;

            // Check if it's the FilePatternMatch constructor
            if (constructorSymbol.ContainingType?.ToString() != "Microsoft.Extensions.FileSystemGlobbing.FilePatternMatch")
                return;

            // Check if it's the constructor with 2 parameters (path, stem)
            if (constructorSymbol.Parameters.Length != 2)
                return;

            // Verify parameter names match expected signature
            if (constructorSymbol.Parameters[0].Name != "path" ||
                constructorSymbol.Parameters[1].Name != "stem")
                return;

            // Check if we have arguments
            if (objectCreation.ArgumentList == null || objectCreation.ArgumentList.Arguments.Count < 2)
                return;

            // Get the stem argument (second parameter)
            var stemArgument = objectCreation.ArgumentList.Arguments[1];

            // Check if the stem argument is null
            if (IsNullLiteral(stemArgument.Expression, context.SemanticModel))
            {
                // Report diagnostic on the null literal
                var diagnostic = Diagnostic.Create(Rule, stemArgument.Expression.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool IsNullLiteral(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            // Check for null literal keyword
            if (expression.IsKind(SyntaxKind.NullLiteralExpression))
                return true;

            // Check for default literal that represents null
            if (expression.IsKind(SyntaxKind.DefaultLiteralExpression))
            {
                var typeInfo = semanticModel.GetTypeInfo(expression);
                return typeInfo.Type == null || typeInfo.Type.IsReferenceType || typeInfo.Type.NullableAnnotation == NullableAnnotation.Annotated;
            }

            // Check for default(string) or default(string?)
            if (expression is DefaultExpressionSyntax defaultExpr)
            {
                var typeInfo = semanticModel.GetTypeInfo(defaultExpr);
                return typeInfo.Type == null || typeInfo.Type.IsReferenceType || typeInfo.Type.NullableAnnotation == NullableAnnotation.Annotated;
            }

            return false;
        }
    }
}
