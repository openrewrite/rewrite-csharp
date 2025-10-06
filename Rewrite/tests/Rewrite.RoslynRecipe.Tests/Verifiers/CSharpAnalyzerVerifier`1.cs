using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using TUnit.Assertions.AssertionBuilders;

namespace Rewrite.RoslynRecipe.Tests.Verifiers;

public static partial class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic()
        => CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic();

    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic(string diagnosticId)
        => CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(diagnosticId);

    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(descriptor);

    public static async Task VerifyAnalyzerDotnet100Async([StringSyntax("c#-test")] string source, params DiagnosticResult[] expected)
    {
        await VerifyAnalyzerAsync(source, ReferenceAssemblies.Net.Net100, expected);
    }
    
    public static async Task VerifyAnalyzerDotnet90Async([StringSyntax("c#-test")] string source, params DiagnosticResult[] expected)
    {
        await VerifyAnalyzerAsync(source, ReferenceAssemblies.Net.Net90, expected);
    }
    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    private static async Task VerifyAnalyzerAsync([StringSyntax("c#-test")] string source, ReferenceAssemblies referenceAssemblies, params DiagnosticResult[] expected)
    {
        var test = new Test
        {
            TestCode = source,
            ReferenceAssemblies = referenceAssemblies
                // .AddPackages([new PackageIdentity("xunit.v3.assert", "2.0.0")])
            ,
            TestState =
            {
                AdditionalReferences =
                {
                    typeof(TUnitAttribute).Assembly.Location,
                    typeof(AssertionBuilder).Assembly.Location,
                },
            },
            CompilerDiagnostics = CompilerDiagnostics.None
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}