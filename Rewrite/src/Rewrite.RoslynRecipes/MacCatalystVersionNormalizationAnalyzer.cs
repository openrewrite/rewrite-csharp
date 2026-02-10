using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usages of APIs affected by the MacCatalyst version normalization
    /// breaking change in .NET 10. In .NET 10, the MacCatalyst build component is normalized to
    /// <c>0</c> if undefined (<c>-1</c>), and the revision component is always set to <c>-1</c>,
    /// ensuring consistent version checks across iOS and MacCatalyst platforms.
    /// </summary>
    /// <remarks>
    /// Affected APIs:
    /// <list type="bullet">
    ///   <item><see cref="System.OperatingSystem.IsMacCatalystVersionAtLeast(int, int, int)"/></item>
    ///   <item><see cref="System.OperatingSystem.IsOSPlatformVersionAtLeast(string, int, int, int, int)"/></item>
    ///   <item><see cref="System.Environment.OSVersion"/></item>
    /// </list>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/maccatalyst-version-normalization">
    /// MacCatalyst version normalization documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class MacCatalystVersionNormalizationAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic ID for IsMacCatalystVersionAtLeast behavioral change.
        /// </summary>
        public const string IsMacCatalystVersionAtLeastDiagnosticId = "ORNETX0014";

        /// <summary>
        /// The diagnostic ID for IsOSPlatformVersionAtLeast behavioral change.
        /// </summary>
        public const string IsOSPlatformVersionAtLeastDiagnosticId = "ORNETX0015";

        /// <summary>
        /// The diagnostic ID for Environment.OSVersion behavioral change.
        /// </summary>
        public const string EnvironmentOSVersionDiagnosticId = "ORNETX0016";

        private const string Category = "Compatibility";
        private const string HelpLinkUri =
            "https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/10.0/maccatalyst-version-normalization";

        private static readonly LocalizableString IsMacCatalystVersionAtLeastTitle =
            "IsMacCatalystVersionAtLeast has behavioral changes in .NET 10 due to version normalization";

        private static readonly LocalizableString IsMacCatalystVersionAtLeastMessageFormat =
            "IsMacCatalystVersionAtLeast may behave differently in .NET 10. The MacCatalyst build component " +
            "is now normalized to 0 if undefined, ensuring consistent version checks.";

        private static readonly LocalizableString IsMacCatalystVersionAtLeastDescription =
            "In .NET 10, OperatingSystem.IsMacCatalystVersionAtLeast normalizes the MacCatalyst build component " +
            "to 0 if undefined (-1), and the revision component is always set to -1. " +
            "This may change the result of version checks that previously relied on inconsistent normalization. " +
            "Use versions of up to three components (major, minor, build) on MacCatalyst for consistent behavior.";

        private static readonly LocalizableString IsOSPlatformVersionAtLeastTitle =
            "IsOSPlatformVersionAtLeast has behavioral changes in .NET 10 due to MacCatalyst version normalization";

        private static readonly LocalizableString IsOSPlatformVersionAtLeastMessageFormat =
            "IsOSPlatformVersionAtLeast may behave differently in .NET 10 on MacCatalyst. The build component " +
            "is now normalized to 0 if undefined, ensuring consistent version checks.";

        private static readonly LocalizableString IsOSPlatformVersionAtLeastDescription =
            "In .NET 10, OperatingSystem.IsOSPlatformVersionAtLeast normalizes the MacCatalyst build component " +
            "to 0 if undefined (-1), and the revision component is always set to -1. " +
            "This may change the result of version checks on MacCatalyst that previously relied on inconsistent normalization.";

        private static readonly LocalizableString EnvironmentOSVersionTitle =
            "Environment.OSVersion has behavioral changes in .NET 10 due to MacCatalyst version normalization";

        private static readonly LocalizableString EnvironmentOSVersionMessageFormat =
            "Environment.OSVersion may return a differently normalized version on MacCatalyst in .NET 10. " +
            "The build component is now normalized to 0 if undefined.";

        private static readonly LocalizableString EnvironmentOSVersionDescription =
            "In .NET 10, Environment.OSVersion on MacCatalyst normalizes the build component to 0 if undefined (-1), " +
            "and the revision component is always set to -1. Code that compares version components may get different results.";

        private static readonly DiagnosticDescriptor IsMacCatalystVersionAtLeastRule = new(
            IsMacCatalystVersionAtLeastDiagnosticId,
            IsMacCatalystVersionAtLeastTitle,
            IsMacCatalystVersionAtLeastMessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: IsMacCatalystVersionAtLeastDescription,
            helpLinkUri: HelpLinkUri);

        private static readonly DiagnosticDescriptor IsOSPlatformVersionAtLeastRule = new(
            IsOSPlatformVersionAtLeastDiagnosticId,
            IsOSPlatformVersionAtLeastTitle,
            IsOSPlatformVersionAtLeastMessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: IsOSPlatformVersionAtLeastDescription,
            helpLinkUri: HelpLinkUri);

        private static readonly DiagnosticDescriptor EnvironmentOSVersionRule = new(
            EnvironmentOSVersionDiagnosticId,
            EnvironmentOSVersionTitle,
            EnvironmentOSVersionMessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: EnvironmentOSVersionDescription,
            helpLinkUri: HelpLinkUri);

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(IsMacCatalystVersionAtLeastRule, IsOSPlatformVersionAtLeastRule, EnvironmentOSVersionRule);

        /// <summary>
        /// Registers analysis actions for invocation expressions and member access expressions
        /// to detect usages of APIs affected by MacCatalyst version normalization.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
        }

        /// <summary>
        /// Analyzes invocation expressions to detect calls to IsMacCatalystVersionAtLeast
        /// and IsOSPlatformVersionAtLeast.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;

            switch (methodName)
            {
                case "IsMacCatalystVersionAtLeast":
                    if (invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                            "M:System.OperatingSystem.IsMacCatalystVersionAtLeast(System.Int32,System.Int32,System.Int32)"))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            IsMacCatalystVersionAtLeastRule, memberAccess.Name.GetLocation()));
                    }
                    break;

                case "IsOSPlatformVersionAtLeast":
                    if (invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                            "M:System.OperatingSystem.IsOSPlatformVersionAtLeast(System.String,System.Int32,System.Int32,System.Int32,System.Int32)"))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            IsOSPlatformVersionAtLeastRule, memberAccess.Name.GetLocation()));
                    }
                    break;
            }
        }

        /// <summary>
        /// Analyzes member access expressions to detect usage of the Environment.OSVersion property.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the member access to analyze.</param>
        private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;

            if (memberAccess.Name.Identifier.Text != "OSVersion")
                return;

            // Skip if this is the expression part of an invocation (handled by AnalyzeInvocation)
            if (memberAccess.Parent is InvocationExpressionSyntax)
                return;

            if (!memberAccess.IsSymbolOneOf(context.SemanticModel,
                    "P:System.Environment.OSVersion"))
                return;

            context.ReportDiagnostic(Diagnostic.Create(
                EnvironmentOSVersionRule, memberAccess.Name.GetLocation()));
        }
    }
}
