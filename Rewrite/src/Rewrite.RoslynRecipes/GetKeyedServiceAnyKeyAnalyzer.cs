using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usages of <c>GetKeyedService</c> and <c>GetKeyedServices</c>
    /// extension methods with <c>KeyedService.AnyKey</c> as the service key argument.
    /// In .NET 10, <c>GetKeyedService</c> with <c>AnyKey</c> throws an
    /// <see cref="System.InvalidOperationException"/>, and <c>GetKeyedServices</c> with
    /// <c>AnyKey</c> no longer returns <c>AnyKey</c> registrations.
    /// </summary>
    /// <remarks>
    /// The previous behavior was inconsistent with the intended semantics of <c>AnyKey</c>,
    /// which is meant as a special case for fallback registrations, not as a specific key
    /// for resolution. Code should use specific keys instead.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/getkeyedservice-anykey">
    /// GetKeyedService and GetKeyedServices with AnyKey documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class GetKeyedServiceAnyKeyAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic ID for GetKeyedService/GetKeyedServices called with KeyedService.AnyKey.
        /// </summary>
        public const string DiagnosticId = "ORNETX0021";

        private const string Category = "Compatibility";
        private const string HelpLinkUri =
            "https://learn.microsoft.com/en-us/dotnet/core/compatibility/extensions/10.0/getkeyedservice-anykey";

        private static readonly LocalizableString Title =
            "GetKeyedService/GetKeyedServices with KeyedService.AnyKey has changed behavior in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "{0} with KeyedService.AnyKey has changed behavior in .NET 10. " +
            "GetKeyedService now throws InvalidOperationException and GetKeyedServices no longer " +
            "returns AnyKey registrations. Use a specific key instead.";

        private static readonly LocalizableString Description =
            "In .NET 10, calling GetKeyedService with KeyedService.AnyKey throws an InvalidOperationException, " +
            "and GetKeyedServices with KeyedService.AnyKey no longer returns AnyKey registrations. " +
            "Code should use specific keys instead of AnyKey for service resolution.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: HelpLinkUri);

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for invocation expressions to detect calls to
        /// GetKeyedService and GetKeyedServices with KeyedService.AnyKey.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        /// <summary>
        /// Analyzes invocation expressions to detect calls to GetKeyedService or GetKeyedServices
        /// with KeyedService.AnyKey as the service key argument.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;
            if (methodName != "GetKeyedService" && methodName != "GetKeyedServices")
                return;

            // Find an argument that syntactically looks like *.AnyKey
            var anyKeyArg = FindAnyKeySyntaxCandidate(invocation);
            if (anyKeyArg == null)
                return;

            // Semantically verify the argument is KeyedService.AnyKey
            if (!anyKeyArg.IsSymbolOneOf(context.SemanticModel,
                    "P:Microsoft.Extensions.DependencyInjection.KeyedService.AnyKey"))
                return;

            // Verify the method is one of the targeted extension methods or interface methods.
            // Extension methods called as instance methods produce "reduced" symbols whose doc IDs
            // omit the first (this) parameter, so we include both reduced and original doc IDs.
            if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                    // Reduced extension method doc IDs (instance-style calls, without IServiceProvider)
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedService``1(System.Object)",
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedServices``1(System.Object)",
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedServices(System.Type,System.Object)",
                    // Original extension method doc IDs (static-style calls, with IServiceProvider)
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedService``1(System.IServiceProvider,System.Object)",
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedServices``1(System.IServiceProvider,System.Object)",
                    "M:Microsoft.Extensions.DependencyInjection.ServiceProviderKeyedServiceExtensions.GetKeyedServices(System.IServiceProvider,System.Type,System.Object)",
                    // IKeyedServiceProvider interface method (not an extension method)
                    "M:Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(System.Type,System.Object)"))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, anyKeyArg.GetLocation(), methodName));
        }

        /// <summary>
        /// Searches the invocation's argument list for a member access expression with the name "AnyKey",
        /// which is a syntactic candidate for <c>KeyedService.AnyKey</c>.
        /// </summary>
        /// <param name="invocation">The invocation expression to search.</param>
        /// <returns>The member access expression if found; otherwise, <c>null</c>.</returns>
        private static MemberAccessExpressionSyntax? FindAnyKeySyntaxCandidate(InvocationExpressionSyntax invocation)
        {
            foreach (var arg in invocation.ArgumentList.Arguments)
            {
                if (arg.Expression is MemberAccessExpressionSyntax { Name.Identifier.Text: "AnyKey" } memberAccess)
                    return memberAccess;
            }

            return null;
        }
    }
}
