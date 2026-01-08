using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzer to detect usage of DistributedContextPropagator APIs affected by .NET 10 behavior changes.
    /// In .NET 10, the default trace context propagator switched from legacy to W3C standard,
    /// using 'baggage' header instead of 'Correlation-Context' and enforcing W3C-compliant encoding.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DistributedContextPropagatorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ORNETX0003";

        private static readonly LocalizableString Title = "Default trace context propagator changed to W3C in .NET 10";
        private static readonly LocalizableString MessageFormat = "DistributedContextPropagator default changed from legacy to W3C propagator in .NET 10. Uses 'baggage' header and enforces strict W3C formatting.";
        private static readonly LocalizableString Description = "In .NET 10, DistributedContextPropagator.CreateDefaultPropagator() and DistributedContextPropagator.Current " +
            "return the W3C propagator instead of the legacy propagator. The W3C propagator uses the 'baggage' header instead of 'Correlation-Context' " +
            "and enforces W3C-compliant encoding with stricter formatting for trace parent, trace state, and baggage keys/values. " +
            "To retain legacy behavior, use: DistributedContextPropagator.Current = DistributedContextPropagator.CreatePreW3CPropagator();";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/default-trace-context-propagator");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Detect invocations of CreateDefaultPropagator()
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);

            // Detect reads/writes of DistributedContextPropagator.Current
            context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
        }

        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Check if it's a method invocation
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
                return;

            // Check if it's CreateDefaultPropagator()
            if (methodSymbol.ContainingType?.ToString() != "System.Diagnostics.DistributedContextPropagator" ||
                methodSymbol.Name != "CreateDefaultPropagator")
                return;

            // Report diagnostic on the method name only (not the entire invocation)
            Location location;
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                location = memberAccess.Name.GetLocation();
            }
            else
            {
                location = invocation.GetLocation();
            }

            var diagnostic = Diagnostic.Create(Rule, location);
            context.ReportDiagnostic(diagnostic);
        }

        private void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;

            // Check if the member being accessed is "Current"
            if (memberAccess.Name.Identifier.Text != "Current")
                return;

            // Get the symbol information
            var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
            if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
                return;

            // Check if it's DistributedContextPropagator.Current
            if (propertySymbol.ContainingType?.ToString() != "System.Diagnostics.DistributedContextPropagator" ||
                propertySymbol.Name != "Current")
                return;

            // Check if this is being assigned to (write) vs being read
            if (IsBeingAssigned(memberAccess))
            {
                // If it's being assigned, check if it's being set to CreatePreW3CPropagator()
                // If so, don't report diagnostic (they're already handling the change)
                var assignment = memberAccess.Parent as AssignmentExpressionSyntax;
                if (assignment != null && IsCreatePreW3CPropagatorCall(assignment.Right, context.SemanticModel))
                    return;
            }

            // Report diagnostic for reads or writes that aren't using CreatePreW3CPropagator
            var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }

        private bool IsBeingAssigned(MemberAccessExpressionSyntax memberAccess)
        {
            // Check if this member access is on the left side of an assignment
            return memberAccess.Parent is AssignmentExpressionSyntax assignment &&
                   assignment.Left == memberAccess;
        }

        private bool IsCreatePreW3CPropagatorCall(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            // Check if the expression is a call to CreatePreW3CPropagator()
            if (expression is InvocationExpressionSyntax invocation)
            {
                var symbolInfo = semanticModel.GetSymbolInfo(invocation);
                if (symbolInfo.Symbol is IMethodSymbol methodSymbol)
                {
                    return methodSymbol.ContainingType?.ToString() == "System.Diagnostics.DistributedContextPropagator" &&
                           methodSymbol.Name == "CreatePreW3CPropagator";
                }
            }

            return false;
        }
    }
}
