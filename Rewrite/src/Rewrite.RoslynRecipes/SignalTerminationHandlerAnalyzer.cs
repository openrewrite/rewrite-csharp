using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzer to detect event registrations for ProcessExit and Unloading that may have relied on
    /// SIGTERM signal handling in previous .NET versions. In .NET 10, default termination signal handlers
    /// were removed, so these events are no longer triggered by signals.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SignalTerminationHandlerAnalyzer : DiagnosticAnalyzer
    {
        public const string ProcessExitDiagnosticId = "ORNETX0006";
        public const string UnloadingDiagnosticId = "ORNETX0007";

        private static readonly LocalizableString ProcessExitTitle = "AppDomain.ProcessExit no longer triggered by termination signals in .NET 10";
        private static readonly LocalizableString ProcessExitMessageFormat = "AppDomain.ProcessExit event is no longer triggered by SIGTERM/SIGINT signals in .NET 10. If signal handling is needed, use PosixSignalRegistration.Create().";
        private static readonly LocalizableString ProcessExitDescription = "In .NET 10, the runtime no longer provides default termination signal handlers. " +
            "ProcessExit events are no longer raised when SIGTERM or SIGINT signals are received. " +
            "If your code relies on ProcessExit being triggered by signals, manually create signal handlers using PosixSignalRegistration.Create(). " +
            "ASP.NET and higher-level APIs handle this automatically.";

        private static readonly LocalizableString UnloadingTitle = "AssemblyLoadContext.Unloading no longer triggered by termination signals in .NET 10";
        private static readonly LocalizableString UnloadingMessageFormat = "AssemblyLoadContext.Unloading event is no longer triggered by SIGTERM/SIGINT signals in .NET 10. If signal handling is needed, use PosixSignalRegistration.Create().";
        private static readonly LocalizableString UnloadingDescription = "In .NET 10, the runtime no longer provides default termination signal handlers. " +
            "Unloading events are no longer raised when SIGTERM or SIGINT signals are received. " +
            "If your code relies on Unloading being triggered by signals, manually create signal handlers using PosixSignalRegistration.Create(). " +
            "ASP.NET and higher-level APIs handle this automatically.";

        private static readonly DiagnosticDescriptor ProcessExitRule = new DiagnosticDescriptor(
            ProcessExitDiagnosticId,
            ProcessExitTitle,
            ProcessExitMessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: ProcessExitDescription,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sigterm-signal-handler");

        private static readonly DiagnosticDescriptor UnloadingRule = new DiagnosticDescriptor(
            UnloadingDiagnosticId,
            UnloadingTitle,
            UnloadingMessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: UnloadingDescription,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/sigterm-signal-handler");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ProcessExitRule, UnloadingRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Register for += operations (event registration)
            context.RegisterSyntaxNodeAction(AnalyzeAssignment, SyntaxKind.AddAssignmentExpression);
        }

        private void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
        {
            var assignment = (AssignmentExpressionSyntax)context.Node;

            // Get the symbol being assigned to
            var symbolInfo = context.SemanticModel.GetSymbolInfo(assignment.Left);
            if (symbolInfo.Symbol is not IEventSymbol eventSymbol)
                return;

            // Check for AppDomain.ProcessExit
            if (eventSymbol.ContainingType?.ToString() == "System.AppDomain" &&
                eventSymbol.Name == "ProcessExit")
            {
                var location = assignment.Left is MemberAccessExpressionSyntax memberAccess
                    ? memberAccess.Name.GetLocation()
                    : assignment.Left.GetLocation();

                var diagnostic = Diagnostic.Create(ProcessExitRule, location);
                context.ReportDiagnostic(diagnostic);
                return;
            }

            // Check for AssemblyLoadContext.Unloading
            if (eventSymbol.ContainingType?.ToString() == "System.Runtime.Loader.AssemblyLoadContext" &&
                eventSymbol.Name == "Unloading")
            {
                var location = assignment.Left is MemberAccessExpressionSyntax memberAccess
                    ? memberAccess.Name.GetLocation()
                    : assignment.Left.GetLocation();

                var diagnostic = Diagnostic.Create(UnloadingRule, location);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
