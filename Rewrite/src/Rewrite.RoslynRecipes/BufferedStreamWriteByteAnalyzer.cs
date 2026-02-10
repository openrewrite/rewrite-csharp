using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usages of <see cref="System.IO.BufferedStream.WriteByte(byte)"/> which has
    /// behavioral changes in .NET 10. In .NET 10, <c>WriteByte</c> no longer performs an implicit flush
    /// when the internal buffer is full, making it consistent with other <c>Write</c> methods.
    /// Code that relies on the implicit flush behavior must be updated to explicitly call <c>Flush()</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/bufferedstream-writebyte-flush">
    /// BufferedStream.WriteByte breaking change documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BufferedStreamWriteByteAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for BufferedStream.WriteByte behavioral change detection.
        /// </summary>
        public const string DiagnosticId = "ORNETX0012";

        private static readonly LocalizableString Title =
            "BufferedStream.WriteByte no longer implicitly flushes in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "BufferedStream.WriteByte no longer performs an implicit flush when the buffer is full in .NET 10. " +
            "Review usage and add an explicit Flush() call if needed.";

        private static readonly LocalizableString Description =
            "In .NET 10, BufferedStream.WriteByte no longer implicitly flushes the buffer when it is full. " +
            "This behavioral change makes it consistent with other Write methods in BufferedStream. " +
            "Code that depends on the implicit flush must be updated to call Flush() explicitly.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/bufferedstream-writebyte-flush");

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for invocation expressions to detect WriteByte calls on BufferedStream.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Quick syntax check: must be a member access with name "WriteByte"
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            if (memberAccess.Name.Identifier.Text != "WriteByte")
                return;

            // Semantic check: verify this is BufferedStream.WriteByte
            if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                    "M:System.IO.BufferedStream.WriteByte(System.Byte)"))
                return;

            var diagnostic = Diagnostic.Create(Rule, memberAccess.Name.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
