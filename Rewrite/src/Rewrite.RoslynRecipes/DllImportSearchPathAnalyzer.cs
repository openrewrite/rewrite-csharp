using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Analyzes C# code for usage of <c>DllImportSearchPath.AssemblyDirectory</c> as the sole search
    /// flag, which no longer falls back to OS default library search paths in .NET 10.
    /// </summary>
    /// <remarks>
    /// In .NET 10, specifying <c>DllImportSearchPath.AssemblyDirectory</c> as the only search flag now
    /// searches exclusively in the assembly directory. Previously, if the library was not found there,
    /// the runtime would fall back to the OS default library search behavior.
    /// This analyzer detects:
    /// <list type="bullet">
    /// <item><c>[DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]</c> attributes</item>
    /// <item><c>NativeLibrary.Load</c> and <c>NativeLibrary.TryLoad</c> calls with
    /// <c>DllImportSearchPath.AssemblyDirectory</c> as the sole search path</item>
    /// </list>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/search-assembly-directory">
    /// DllImportSearchPath.AssemblyDirectory behavior change documentation</see> for details.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DllImportSearchPathAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The diagnostic identifier for the <c>DefaultDllImportSearchPaths</c> attribute detection.
        /// </summary>
        public const string AttributeDiagnosticId = "ORNETX0023";

        /// <summary>
        /// The diagnostic identifier for the <c>NativeLibrary.Load</c>/<c>TryLoad</c> method call detection.
        /// </summary>
        public const string MethodDiagnosticId = "ORNETX0024";

        private static readonly LocalizableString AttributeTitle =
            "DefaultDllImportSearchPaths with AssemblyDirectory only no longer falls back to OS default search in .NET 10";

        private static readonly LocalizableString AttributeMessageFormat =
            "DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory) now searches only the " +
            "assembly directory without falling back to OS default paths in .NET 10. " +
            "Remove the attribute to restore fallback behavior.";

        private static readonly LocalizableString AttributeDescription =
            "In .NET 10, specifying DllImportSearchPath.AssemblyDirectory as the only search flag in " +
            "DefaultDllImportSearchPaths searches exclusively in the assembly directory. Previously, " +
            "it would fall back to the OS default library search behavior if the library was not found. " +
            "Remove the attribute to use the runtime's default search which includes the assembly " +
            "directory first followed by OS default paths.";

        private static readonly LocalizableString MethodTitle =
            "DllImportSearchPath.AssemblyDirectory no longer falls back to OS default search in .NET 10";

        private static readonly LocalizableString MethodMessageFormat =
            "Passing DllImportSearchPath.AssemblyDirectory to NativeLibrary.{0} now searches only the " +
            "assembly directory without falling back to OS default paths. " +
            "Use null to restore default search behavior.";

        private static readonly LocalizableString MethodDescription =
            "In .NET 10, passing DllImportSearchPath.AssemblyDirectory as the sole search path to " +
            "NativeLibrary.Load or NativeLibrary.TryLoad searches exclusively in the assembly directory. " +
            "Previously, it would fall back to the OS default library search behavior if the library " +
            "was not found. Pass null to use the runtime's default search which includes the assembly " +
            "directory first followed by OS default paths.";

        private const string HelpLinkUri =
            "https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/search-assembly-directory";

        private static readonly DiagnosticDescriptor AttributeRule = new(
            AttributeDiagnosticId,
            AttributeTitle,
            AttributeMessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: AttributeDescription,
            helpLinkUri: HelpLinkUri);

        private static readonly DiagnosticDescriptor MethodRule = new(
            MethodDiagnosticId,
            MethodTitle,
            MethodMessageFormat,
            "Compatibility",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: MethodDescription,
            helpLinkUri: HelpLinkUri);

        /// <summary>
        /// Returns the set of diagnostic descriptors supported by this analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(AttributeRule, MethodRule);

        /// <summary>
        /// Registers analysis actions for attribute and invocation syntax nodes to detect
        /// <c>DllImportSearchPath.AssemblyDirectory</c> used as the sole search flag.
        /// </summary>
        /// <param name="context">The analysis context to register actions with. Must not be null.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeAttribute, SyntaxKind.Attribute);
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeAttribute(SyntaxNodeAnalysisContext context)
        {
            var attribute = (AttributeSyntax)context.Node;

            // Quick syntax check: name should contain "DefaultDllImportSearchPaths"
            var attrName = attribute.Name.ToString();
            if (!attrName.Contains("DefaultDllImportSearchPaths"))
                return;

            // Check argument is sole DllImportSearchPath.AssemblyDirectory (not combined with |)
            if (attribute.ArgumentList is null || attribute.ArgumentList.Arguments.Count == 0)
                return;

            var argExpr = attribute.ArgumentList.Arguments[0].Expression;
            if (!IsStandaloneAssemblyDirectory(argExpr))
                return;

            // Semantic check: verify this is DefaultDllImportSearchPathsAttribute
            var attrSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
            if (attrSymbol?.ContainingType?.ToDisplayString() !=
                "System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute")
                return;

            context.ReportDiagnostic(Diagnostic.Create(AttributeRule, attribute.GetLocation()));
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Quick syntax check: method name is Load or TryLoad
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;
            if (methodName is not ("Load" or "TryLoad"))
                return;

            // Find an argument that is standalone DllImportSearchPath.AssemblyDirectory
            ExpressionSyntax? assemblyDirArg = null;
            foreach (var arg in invocation.ArgumentList.Arguments)
            {
                if (IsStandaloneAssemblyDirectory(arg.Expression))
                {
                    assemblyDirArg = arg.Expression;
                    break;
                }
            }

            if (assemblyDirArg is null)
                return;

            // Semantic check: verify this is NativeLibrary.Load or NativeLibrary.TryLoad
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
                return;

            if (methodSymbol.ContainingType?.ToDisplayString() !=
                "System.Runtime.InteropServices.NativeLibrary")
                return;

            // Verify the argument is actually DllImportSearchPath enum
            var argSymbol = context.SemanticModel.GetSymbolInfo(assemblyDirArg).Symbol;
            if (argSymbol is not IFieldSymbol fieldSymbol)
                return;

            if (fieldSymbol.ContainingType?.ToDisplayString() !=
                "System.Runtime.InteropServices.DllImportSearchPath")
                return;

            context.ReportDiagnostic(
                Diagnostic.Create(MethodRule, assemblyDirArg.GetLocation(), methodName));
        }

        private static bool IsStandaloneAssemblyDirectory(ExpressionSyntax expression)
        {
            return expression is MemberAccessExpressionSyntax memberAccess &&
                   memberAccess.Name.Identifier.Text == "AssemblyDirectory";
        }
    }
}
