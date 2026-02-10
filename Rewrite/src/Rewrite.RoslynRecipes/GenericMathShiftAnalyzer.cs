using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for shift operations used through the generic math
    /// <c>IShiftOperators&lt;TSelf, TOther, TResult&gt;</c> interface, which have behavioral changes in .NET 10.
    /// In .NET 10, shift operations on <c>byte</c>, <c>sbyte</c>, <c>short</c>, <c>ushort</c>, and <c>char</c>
    /// now consistently mask the shift amount, changing behavior for "overshifting" scenarios.
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/generic-math">
    /// Consistent behavior for shift operations in generic math</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GenericMathShiftAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for generic math shift behavior change detection.
        /// </summary>
        public const string DiagnosticId = "ORNETX0013";

        private static readonly LocalizableString Title =
            "Generic math shift operations have consistent masking behavior in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "Shift operation through IShiftOperators may behave differently in .NET 10 for byte, sbyte, short, ushort, " +
            "and char types due to consistent shift amount masking. Review usage if code relies on overshifting behavior.";

        private static readonly LocalizableString Description =
            "In .NET 10, shift operations (<<, >>, >>>) on byte, sbyte, short, ushort, and char via the generic math " +
            "IShiftOperators<TSelf, TOther, TResult> interface now consistently mask the shift amount by the type's bit width. " +
            "Previously, some types did not mask, leading to inconsistent overshifting behavior. " +
            "Code that relied on the previous inconsistent behavior must be reviewed.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/generic-math");

        private const string ShiftOperatorsDocId = "T:System.Numerics.IShiftOperators`3";

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for shift expressions and compound shift assignment expressions
        /// to detect operations resolved through the <c>IShiftOperators</c> generic math interface.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeShiftOperation,
                SyntaxKind.LeftShiftExpression,
                SyntaxKind.RightShiftExpression,
                SyntaxKind.UnsignedRightShiftExpression,
                SyntaxKind.LeftShiftAssignmentExpression,
                SyntaxKind.RightShiftAssignmentExpression,
                SyntaxKind.UnsignedRightShiftAssignmentExpression);
        }

        private void AnalyzeShiftOperation(SyntaxNodeAnalysisContext context)
        {
            var expression = (ExpressionSyntax)context.Node;

            var symbolInfo = context.SemanticModel.GetSymbolInfo(expression);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
                return;

            // Check if the operator is defined on the IShiftOperators<,,> interface.
            // When shift operators are resolved through generic math constraints (e.g. T : IShiftOperators<T, int, T>
            // or T : IBinaryInteger<T>), the resolved operator's containing type traces back to IShiftOperators.
            var containingType = methodSymbol.ContainingType?.OriginalDefinition;
            if (containingType?.GetDocumentationCommentId() != ShiftOperatorsDocId)
                return;

            Location location = expression switch
            {
                BinaryExpressionSyntax binary => binary.OperatorToken.GetLocation(),
                AssignmentExpressionSyntax assignment => assignment.OperatorToken.GetLocation(),
                _ => expression.GetLocation()
            };

            context.ReportDiagnostic(Diagnostic.Create(Rule, location));
        }
    }
}
