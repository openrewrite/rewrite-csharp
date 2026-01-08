using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Rewrite.RoslynRecipes.Tests.Verifiers;

internal static class CSharpVerifierHelper
{
    /// <summary>
    /// By default, the compiler reports diagnostics for nullable reference types at
    /// <see cref="DiagnosticSeverity.Warning"/>, and the analyzer test framework defaults to only validating
    /// diagnostics at <see cref="DiagnosticSeverity.Error"/>. This map contains all compiler diagnostic IDs
    /// related to nullability mapped to <see cref="ReportDiagnostic.Error"/>, which is then used to enable all
    /// of these warnings for default validation during analyzer and code fix tests.
    /// </summary>
    internal static ImmutableDictionary<string, ReportDiagnostic> NullableWarnings { get; } = GetNullableWarningsFromCompiler();

    private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
    {
        string[] args = ["/warnaserror:nullable", "-p:LangVersion=preview"];
        var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
        var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

        // Workaround for https://github.com/dotnet/roslyn/issues/41610
        nullableWarnings = nullableWarnings
            .SetItem("CS8602", ReportDiagnostic.Suppress)
            .SetItem("CS8632", ReportDiagnostic.Error)
            .SetItem("CS8669", ReportDiagnostic.Error)
            .SetItem("CS8652", ReportDiagnostic.Suppress);

        return nullableWarnings;
    }

    /// <summary>
    /// Strips analyzer boundary markers from C# code strings to produce valid C# code.
    /// </summary>
    /// <param name="code">The C# code containing analyzer boundary markers.</param>
    /// <returns>The C# code with all analyzer boundary markers removed.</returns>
    /// <remarks>
    /// This method removes diagnostic markers like {|DiagnosticId:code|} used in analyzer tests,
    /// leaving only the code portion. For example:
    /// - Input: "{|ORNETX0009:IActionContextAccessor|}"
    /// - Output: "IActionContextAccessor"
    ///
    /// The pattern matches:
    /// - Opening: {|
    /// - Diagnostic ID: Any characters except : and |
    /// - Separator: :
    /// - Code: Any characters except |
    /// - Closing: |}
    /// </remarks>
    public static string StripAnalyzerBoundaryMarkers(string code)
    {

        // Pattern explanation:
        // \{\|      - Matches the opening {|
        // [^:|]+    - Matches the diagnostic ID (one or more characters that are not : or |)
        // :         - Matches the colon separator
        // ([^|]+)   - Captures the code content (one or more characters that are not |)
        // \|\}      - Matches the closing |}
        //
        // The captured group (1) contains the code we want to keep
        var pattern = @"\{\|[^:|]+:([^|]+)\|\}";
        return Regex.Replace(code, pattern, "$1");
    }
}