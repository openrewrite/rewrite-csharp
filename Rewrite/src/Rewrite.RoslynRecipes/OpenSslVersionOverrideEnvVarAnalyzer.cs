using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usage of the renamed environment variable <c>CLR_OPENSSL_VERSION_OVERRIDE</c>
    /// which was renamed to <c>DOTNET_OPENSSL_VERSION_OVERRIDE</c> in .NET 10 to align with .NET naming
    /// conventions for configuration switch environment variables.
    /// </summary>
    /// <remarks>
    /// This analyzer detects calls to <see cref="System.Environment.GetEnvironmentVariable(string)"/> and
    /// <see cref="System.Environment.SetEnvironmentVariable(string, string?)"/> where the environment variable
    /// name is the old <c>CLR_OPENSSL_VERSION_OVERRIDE</c> value.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/version-override">
    /// CLR_OPENSSL_VERSION_OVERRIDE renamed documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class OpenSslVersionOverrideEnvVarAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for the CLR_OPENSSL_VERSION_OVERRIDE environment variable rename detection.
        /// </summary>
        public const string DiagnosticId = "ORNETX0017";

        /// <summary>
        /// The old environment variable name that is no longer recognized in .NET 10.
        /// </summary>
        internal const string OldEnvVarName = "CLR_OPENSSL_VERSION_OVERRIDE";

        /// <summary>
        /// The new environment variable name that replaces the old one in .NET 10.
        /// </summary>
        internal const string NewEnvVarName = "DOTNET_OPENSSL_VERSION_OVERRIDE";

        private static readonly LocalizableString Title =
            "CLR_OPENSSL_VERSION_OVERRIDE is renamed to DOTNET_OPENSSL_VERSION_OVERRIDE in .NET 10";

        private static readonly LocalizableString MessageFormat =
            "Environment variable 'CLR_OPENSSL_VERSION_OVERRIDE' is renamed to 'DOTNET_OPENSSL_VERSION_OVERRIDE' in .NET 10. " +
            "Update the variable name to ensure OpenSSL version override continues to work.";

        private static readonly LocalizableString Description =
            "In .NET 10, the environment variable 'CLR_OPENSSL_VERSION_OVERRIDE' has been renamed to " +
            "'DOTNET_OPENSSL_VERSION_OVERRIDE' to align with .NET naming conventions. " +
            "Applications that read or set this variable must use the new name for the OpenSSL version preference to take effect.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/version-override");

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Rule);

        /// <summary>
        /// Registers analysis actions for invocation expressions to detect
        /// Environment.GetEnvironmentVariable and Environment.SetEnvironmentVariable calls
        /// using the old CLR_OPENSSL_VERSION_OVERRIDE variable name.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;
            if (methodName != "GetEnvironmentVariable" && methodName != "SetEnvironmentVariable")
                return;

            // Verify this is System.Environment.GetEnvironmentVariable or SetEnvironmentVariable
            if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                    "M:System.Environment.GetEnvironmentVariable(System.String)",
                    "M:System.Environment.GetEnvironmentVariable(System.String,System.EnvironmentVariableTarget)",
                    "M:System.Environment.SetEnvironmentVariable(System.String,System.String)",
                    "M:System.Environment.SetEnvironmentVariable(System.String,System.String,System.EnvironmentVariableTarget)"))
                return;

            // Check the first argument is the old environment variable name literal
            var arguments = invocation.ArgumentList.Arguments;
            if (arguments.Count == 0)
                return;

            var firstArg = arguments[0].Expression;
            if (firstArg is not LiteralExpressionSyntax literal ||
                !literal.IsKind(SyntaxKind.StringLiteralExpression))
                return;

            if (literal.Token.ValueText != OldEnvVarName)
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, literal.GetLocation()));
        }
    }
}
