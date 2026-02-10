using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usages of <c>X509Certificate.GetKeyAlgorithmParameters()</c>,
    /// <c>X509Certificate.GetKeyAlgorithmParametersString()</c>, and <c>PublicKey.EncodedParameters</c>
    /// which now return <c>null</c> in .NET 10 when key algorithm parameters are absent, instead of
    /// returning empty arrays or empty <c>AsnEncodedData</c> objects.
    /// </summary>
    /// <remarks>
    /// In .NET 10, these APIs return <c>null</c> when key algorithm parameters are not present on the
    /// certificate, whereas previously they returned empty values. Code that assumes non-null return values
    /// must be updated to handle <c>null</c>.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/x509-publickey-null">
    /// X509Certificate and PublicKey null parameters documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class X509PublicKeyNullParametersAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic ID for X509Certificate.GetKeyAlgorithmParameters and GetKeyAlgorithmParametersString
        /// returning null in .NET 10.
        /// </summary>
        public const string MethodsDiagnosticId = "ORNETX0019";

        /// <summary>
        /// The diagnostic ID for PublicKey.EncodedParameters returning null in .NET 10.
        /// </summary>
        public const string PropertyDiagnosticId = "ORNETX0020";

        private const string Category = "Compatibility";
        private const string HelpLinkUri =
            "https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/x509-publickey-null";

        private static readonly LocalizableString MethodsTitle =
            "X509Certificate.GetKeyAlgorithmParameters/GetKeyAlgorithmParametersString may return null in .NET 10";

        private static readonly LocalizableString MethodsMessageFormat =
            "X509Certificate.{0} may return null in .NET 10 when key algorithm parameters are absent. " +
            "Previously it returned an empty value. Add a null check before using the result.";

        private static readonly LocalizableString MethodsDescription =
            "In .NET 10, X509Certificate.GetKeyAlgorithmParameters() returns null instead of an empty byte array, " +
            "and GetKeyAlgorithmParametersString() returns null instead of an empty string, when key algorithm " +
            "parameters are absent from the certificate. Code that assumes non-null return values must be updated.";

        private static readonly LocalizableString PropertyTitle =
            "PublicKey.EncodedParameters may return null in .NET 10";

        private static readonly LocalizableString PropertyMessageFormat =
            "PublicKey.EncodedParameters may return null in .NET 10 when key algorithm parameters are absent. " +
            "Previously it returned an empty AsnEncodedData. Add a null check before using the result.";

        private static readonly LocalizableString PropertyDescription =
            "In .NET 10, PublicKey.EncodedParameters returns null instead of an AsnEncodedData with empty value " +
            "when key algorithm parameters are absent. Code that assumes a non-null return value must be updated.";

        private static readonly DiagnosticDescriptor MethodsRule = new(
            MethodsDiagnosticId,
            MethodsTitle,
            MethodsMessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: MethodsDescription,
            helpLinkUri: HelpLinkUri);

        private static readonly DiagnosticDescriptor PropertyRule = new(
            PropertyDiagnosticId,
            PropertyTitle,
            PropertyMessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: PropertyDescription,
            helpLinkUri: HelpLinkUri);

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(MethodsRule, PropertyRule);

        /// <summary>
        /// Registers analysis actions for invocation expressions and member access expressions to detect
        /// usages of APIs that now return null in .NET 10.
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
        /// Analyzes invocation expressions to detect calls to GetKeyAlgorithmParameters and
        /// GetKeyAlgorithmParametersString on X509Certificate.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;
            if (methodName != "GetKeyAlgorithmParameters" && methodName != "GetKeyAlgorithmParametersString")
                return;

            if (!invocation.Expression.IsSymbolOneOf(context.SemanticModel,
                    "M:System.Security.Cryptography.X509Certificates.X509Certificate.GetKeyAlgorithmParameters",
                    "M:System.Security.Cryptography.X509Certificates.X509Certificate.GetKeyAlgorithmParametersString"))
                return;

            context.ReportDiagnostic(Diagnostic.Create(MethodsRule, memberAccess.Name.GetLocation(), methodName));
        }

        /// <summary>
        /// Analyzes member access expressions to detect usage of PublicKey.EncodedParameters property.
        /// </summary>
        /// <param name="context">The syntax node analysis context containing the member access to analyze.</param>
        private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;

            if (memberAccess.Name.Identifier.Text != "EncodedParameters")
                return;

            // Skip if this is the expression part of an invocation (not a property access)
            if (memberAccess.Parent is InvocationExpressionSyntax)
                return;

            if (!memberAccess.IsSymbolOneOf(context.SemanticModel,
                    "P:System.Security.Cryptography.X509Certificates.PublicKey.EncodedParameters"))
                return;

            context.ReportDiagnostic(Diagnostic.Create(PropertyRule, memberAccess.Name.GetLocation()));
        }
    }
}
