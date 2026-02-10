using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for overrides of <c>BackgroundService.ExecuteAsync</c> which has behavioral
    /// changes in .NET 10. In .NET 10, <c>ExecuteAsync</c> runs entirely on a background thread,
    /// whereas previously the synchronous portion before the first <c>await</c> ran on the main thread
    /// and blocked other hosted services from starting.
    /// </summary>
    /// <remarks>
    /// Code that relies on synchronous startup behavior in <c>ExecuteAsync</c> should be moved to the
    /// constructor, <c>StartAsync</c>, or use <c>IHostedLifecycleService</c>.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/backgroundservice-executeasync-task">
    /// BackgroundService.ExecuteAsync Task runs in the background documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BackgroundServiceExecuteAsyncAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for BackgroundService.ExecuteAsync behavioral change detection.
        /// </summary>
        public const string DiagnosticId = "ORNETX0018";

        private const string ExecuteAsyncDocId =
            "M:Microsoft.Extensions.Hosting.BackgroundService.ExecuteAsync(System.Threading.CancellationToken)";

        private static readonly LocalizableString Title =
            "BackgroundService.ExecuteAsync runs entirely on a background thread in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "BackgroundService.ExecuteAsync now runs entirely on a background thread in .NET 10. " +
            "Synchronous code before the first await no longer blocks other hosted services from starting. " +
            "If startup blocking is needed, move code to StartAsync or the constructor.";

        private static readonly LocalizableString Description =
            "In .NET 10, BackgroundService.ExecuteAsync runs entirely on a background thread. Previously, " +
            "the synchronous portion before the first await ran on the main thread and blocked other services " +
            "from starting. Code that relies on this startup-blocking behavior should be moved to the constructor, " +
            "StartAsync override, or use IHostedLifecycleService for more granular control.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/backgroundservice-executeasync-task");

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for method declarations to detect overrides of
        /// <c>BackgroundService.ExecuteAsync</c>.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDecl = (MethodDeclarationSyntax)context.Node;

            // Quick syntax check: must be named ExecuteAsync and be an override
            if (methodDecl.Identifier.Text != "ExecuteAsync")
                return;

            if (!methodDecl.Modifiers.Any(SyntaxKind.OverrideKeyword))
                return;

            // Semantic check: verify this overrides BackgroundService.ExecuteAsync
            var declaredSymbol = context.SemanticModel.GetDeclaredSymbol(methodDecl);
            if (declaredSymbol is not IMethodSymbol methodSymbol)
                return;

            var overriddenMethod = methodSymbol.OverriddenMethod;
            while (overriddenMethod != null)
            {
                if (overriddenMethod.GetDocumentationCommentId() == ExecuteAsyncDocId)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, methodDecl.Identifier.GetLocation()));
                    return;
                }

                overriddenMethod = overriddenMethod.OverriddenMethod;
            }
        }
    }
}
