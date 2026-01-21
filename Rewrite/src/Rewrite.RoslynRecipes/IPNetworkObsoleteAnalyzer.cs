using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes;

/// <summary>
/// Analyzes C# code for usage of the obsolete Microsoft.AspNetCore.HttpOverrides.IPNetwork type
/// and ForwardedHeadersOptions.KnownNetworks property, which are deprecated in .NET 10.
/// </summary>
/// <remarks>
/// In .NET 10, the custom IPNetwork type in Microsoft.AspNetCore.HttpOverrides is obsolete.
/// Code should migrate to System.Net.IPNetwork instead.
/// Additionally, ForwardedHeadersOptions.KnownNetworks should be replaced with KnownIPNetworks.
/// See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IPNetworkObsoleteAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic ID for the IPNetwork type obsoletion.
    /// </summary>
    public const string IPNetworkTypeDiagnosticId = "ORNETX0010";

    /// <summary>
    /// The diagnostic ID for the KnownNetworks property obsoletion.
    /// </summary>
    public const string KnownNetworksDiagnosticId = "ORNETX0011";

    private static readonly LocalizableString IPNetworkTitle =
        "Microsoft.AspNetCore.HttpOverrides.IPNetwork is obsolete in .NET 10";

    private static readonly LocalizableString IPNetworkMessageFormat =
        "IPNetwork type from Microsoft.AspNetCore.HttpOverrides is obsolete. Use System.Net.IPNetwork instead.";

    private static readonly LocalizableString IPNetworkDescription =
        "The Microsoft.AspNetCore.HttpOverrides.IPNetwork type is obsolete in .NET 10. " +
        "Use System.Net.IPNetwork instead. " +
        "See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete";

    private static readonly LocalizableString KnownNetworksTitle =
        "ForwardedHeadersOptions.KnownNetworks is obsolete in .NET 10";

    private static readonly LocalizableString KnownNetworksMessageFormat =
        "KnownNetworks property is obsolete. Use KnownIPNetworks instead.";

    private static readonly LocalizableString KnownNetworksDescription =
        "The ForwardedHeadersOptions.KnownNetworks property is obsolete in .NET 10. " +
        "Use KnownIPNetworks instead. " +
        "See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete";

    private const string Category = "Compatibility";

    private static readonly DiagnosticDescriptor IPNetworkTypeRule = new(
        IPNetworkTypeDiagnosticId,
        IPNetworkTitle,
        IPNetworkMessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: IPNetworkDescription,
        helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete");

    private static readonly DiagnosticDescriptor KnownNetworksRule = new(
        KnownNetworksDiagnosticId,
        KnownNetworksTitle,
        KnownNetworksMessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: KnownNetworksDescription,
        helpLinkUri: "https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete");

    /// <summary>
    /// Gets the supported diagnostics for this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(IPNetworkTypeRule, KnownNetworksRule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions.
    /// </summary>
    /// <param name="context">The analysis context to register actions with. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public override void Initialize(AnalysisContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register for identifier names to detect IPNetwork type usage
        context.RegisterSyntaxNodeAction(AnalyzeIdentifierName, SyntaxKind.IdentifierName);

        // Register for member access to detect KnownNetworks property usage
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    /// <summary>
    /// Analyzes identifier names to detect usage of the obsolete IPNetwork type.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the identifier to analyze.</param>
    private static void AnalyzeIdentifierName(SyntaxNodeAnalysisContext context)
    {
        var identifier = (IdentifierNameSyntax)context.Node;

        // Quick check: only look at identifiers named "IPNetwork"
        if (identifier.Identifier.Text != "IPNetwork")
            return;

        // Don't analyze if this is part of a member access expression where we're not the type reference
        // e.g., in "someVar.IPNetwork", we don't want to flag "IPNetwork"
        if (identifier.Parent is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name == identifier)
            return;

        // Use semantic model to verify this is the obsolete type
        if (!identifier.IsSymbolOneOf(context.SemanticModel,
                "T:Microsoft.AspNetCore.HttpOverrides.IPNetwork"))
            return;

        // Find the outermost qualified name if this identifier is part of one
        // e.g., for "Microsoft.AspNetCore.HttpOverrides.IPNetwork", report on the entire name
        var nodeToReport = GetOutermostQualifiedName(identifier);

        var diagnostic = Diagnostic.Create(
            IPNetworkTypeRule,
            nodeToReport.GetLocation());

        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Gets the outermost qualified name that contains the given identifier.
    /// If the identifier is not part of a qualified name, returns the identifier itself.
    /// </summary>
    /// <param name="identifier">The identifier to find the outermost qualified name for.</param>
    /// <returns>The outermost qualified name or the identifier if not in a qualified name.</returns>
    private static SyntaxNode GetOutermostQualifiedName(IdentifierNameSyntax identifier)
    {
        SyntaxNode current = identifier;

        // Walk up the tree to find the outermost QualifiedNameSyntax
        while (current.Parent is QualifiedNameSyntax qualifiedName && qualifiedName.Right == current)
        {
            current = qualifiedName;
        }

        return current;
    }

    /// <summary>
    /// Analyzes member access expressions to detect usage of the obsolete KnownNetworks property.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the member access to analyze.</param>
    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;

        // Quick check: only look at accesses to "KnownNetworks"
        if (memberAccess.Name.Identifier.Text != "KnownNetworks")
            return;

        // Use semantic model to verify this is the obsolete property
        if (!memberAccess.IsSymbolOneOf(context.SemanticModel,
                "P:Microsoft.AspNetCore.Builder.ForwardedHeadersOptions.KnownNetworks"))
            return;

        var diagnostic = Diagnostic.Create(
            KnownNetworksRule,
            memberAccess.Name.GetLocation());

        context.ReportDiagnostic(diagnostic);
    }
}
