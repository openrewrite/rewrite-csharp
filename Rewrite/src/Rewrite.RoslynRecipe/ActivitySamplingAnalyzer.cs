using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Rewrite.RoslynRecipe
{
    /// <summary>
    /// Analyzer to detect ActivityListener.Sample implementations that may be affected by .NET 10 behavior changes.
    /// In .NET 10, when creating an Activity as PropagationData with a parent marked as Recorded,
    /// Activity.Recorded is now set to False (previously was True), aligning with OpenTelemetry specification.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ActivitySamplingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0002";

        private static readonly LocalizableString Title = "ActivityListener.Sample behavior changed in .NET 10";
        private static readonly LocalizableString MessageFormat = "ActivityListener.Sample delegate returns or may return ActivitySamplingResult.PropagationData. In .NET 10, Activity.Recorded is now False (was True) when parent is Recorded.";
        private static readonly LocalizableString Description = "In .NET 10, when creating an Activity as PropagationData with a parent marked as Recorded, " +
            "Activity.Recorded is set to False and Activity.IsAllDataRequested is set to False (previously Recorded was True). " +
            "This change aligns with the OpenTelemetry specification. Verify your ActivityListener.Sample implementation and, " +
            "if needed, manually set Activity.ActivityTraceFlags to Recorded after CreateActivity or StartActivity.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/activity-sampling");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            
            // Register for property assignments to detect ActivityListener.Sample
            context.RegisterSyntaxNodeAction(AnalyzeAssignment, SyntaxKind.SimpleAssignmentExpression);
        }

        private void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
        {
            var assignment = (AssignmentExpressionSyntax)context.Node;

            // Get the symbol being assigned to (handles both "Sample" and "listener.Sample")
            var symbolInfo = context.SemanticModel.GetSymbolInfo(assignment.Left);
            if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
                return;

            if (propertySymbol.ContainingType?.ToString() != "System.Diagnostics.ActivityListener" ||
                propertySymbol.Name != "Sample")
                return;

            // Now analyze the right-hand side (the delegate being assigned)
            if (assignment.Right is not (ParenthesizedLambdaExpressionSyntax or SimpleLambdaExpressionSyntax or
                AnonymousMethodExpressionSyntax or IdentifierNameSyntax or MemberAccessExpressionSyntax))
                return;

            // For lambda/anonymous methods, analyze the body directly
            if (assignment.Right is ParenthesizedLambdaExpressionSyntax lambda)
            {
                if (AnalyzeDelegateBody(lambda.Body, context.SemanticModel))
                {
                    var diagnostic = Diagnostic.Create(Rule, assignment.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (assignment.Right is SimpleLambdaExpressionSyntax simpleLambda)
            {
                if (AnalyzeDelegateBody(simpleLambda.Body, context.SemanticModel))
                {
                    var diagnostic = Diagnostic.Create(Rule, assignment.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else if (assignment.Right is AnonymousMethodExpressionSyntax anonymousMethod)
            {
                if (anonymousMethod.Block != null && AnalyzeDelegateBody(anonymousMethod.Block, context.SemanticModel))
                {
                    var diagnostic = Diagnostic.Create(Rule, assignment.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
            // For method references, we can't easily analyze external methods, so flag it conservatively
            else if (assignment.Right is IdentifierNameSyntax or MemberAccessExpressionSyntax)
            {
                // Conservative approach: flag all method group assignments as potentially affected
                var diagnostic = Diagnostic.Create(Rule, assignment.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private bool AnalyzeDelegateBody(SyntaxNode body, SemanticModel semanticModel)
        {
            // Handle expression-bodied lambdas (no return statement, just an expression)
            if (body is ExpressionSyntax expression)
            {
                return AnalyzeExpression(expression, body, semanticModel);
            }

            // Get all return statements in the body (for block-bodied lambdas)
            var returnStatements = body.DescendantNodes().OfType<ReturnStatementSyntax>();

            foreach (var returnStatement in returnStatements)
            {
                if (returnStatement.Expression == null)
                    continue;

                if (AnalyzeExpression(returnStatement.Expression, body, semanticModel))
                    return true;
            }

            return false;
        }

        private bool AnalyzeExpression(ExpressionSyntax expression, SyntaxNode bodyContext, SemanticModel semanticModel)
        {
            // Approach 1: Direct return of ActivitySamplingResult.PropagationData
            if (IsPropagationDataLiteral(expression, semanticModel))
                return true;

            // Approach 2: Return of local variable - trace back to see if it's assigned PropagationData
            if (expression is IdentifierNameSyntax identifier)
            {
                var symbol = semanticModel.GetSymbolInfo(identifier).Symbol;
                if (symbol is ILocalSymbol localSymbol)
                {
                    // Find assignments to this local variable
                    var declaringSyntax = localSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();

                    // Check variable declaration with initializer
                    if (declaringSyntax is VariableDeclaratorSyntax declarator)
                    {
                        if (declarator.Initializer?.Value != null &&
                            IsPropagationDataLiteral(declarator.Initializer.Value, semanticModel))
                            return true;
                    }

                    // Check for assignments in the body
                    var assignments = bodyContext.DescendantNodes()
                        .OfType<AssignmentExpressionSyntax>()
                        .Where(a => a.Left is IdentifierNameSyntax id &&
                                   semanticModel.GetSymbolInfo(id).Symbol?.Equals(localSymbol, SymbolEqualityComparer.Default) == true);

                    foreach (var assignment in assignments)
                    {
                        if (IsPropagationDataLiteral(assignment.Right, semanticModel))
                            return true;
                    }
                }
            }

            // Approach 3: Return from method call or any other expression we can't definitively analyze
            // Treat as potentially returning PropagationData
            if (expression is InvocationExpressionSyntax or
                ConditionalExpressionSyntax or
                BinaryExpressionSyntax or
                SwitchExpressionSyntax)
            {
                // For method calls and complex expressions, conservatively flag as potentially affected
                return true;
            }

            return false;
        }

        private bool IsPropagationDataLiteral(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            // Check if the expression is ActivitySamplingResult.PropagationData
            var symbolInfo = semanticModel.GetSymbolInfo(expression);
            if (symbolInfo.Symbol is IFieldSymbol fieldSymbol)
            {
                return fieldSymbol.ContainingType?.ToString() == "System.Diagnostics.ActivitySamplingResult" &&
                       fieldSymbol.Name == "PropagationData";
            }

            return false;
        }
    }
}
