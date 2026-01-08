using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

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
    //
    // // For ASP.NET Core tests, we need to add the ASP.NET Core reference assemblies
    // // The Microsoft.AspNetCore.App.Ref package contains the reference assemblies for ASP.NET Core
    // private static ReferenceAssemblies AspNet100ReferenceAssemblies = ReferenceAssemblies.Net.Net100
    //     .AddPackages(ImmutableArray.Create(
    //         new PackageIdentity("Microsoft.AspNetCore.App.Ref", "10.0.0-rc.2.25502.107")));
    //
    // private static ReferenceAssemblies AspNet90ReferenceAssemblies = ReferenceAssemblies.Net.Net100
    //     .AddPackages(ImmutableArray.Create(
    //         new PackageIdentity("Microsoft.AspNetCore.App.Ref", "9.0.10")));
    //
    // public static async Task VerifyAnalyzerAspnet100Async([StringSyntax("c#-test")] string source, params DiagnosticResult[] expected)
    // {
    //     await VerifyAnalyzerAsync(source, AspNet100ReferenceAssemblies, expected);
    // }
    // public static async Task VerifyAnalyzerAspnet100Async([StringSyntax("c#-test")] string source, Func<ReferenceAssemblies,ReferenceAssemblies> referenceConfiguration, params DiagnosticResult[] expected)
    // {
    //     await VerifyAnalyzerAsync(source, referenceConfiguration(AspNet100ReferenceAssemblies), expected);
    // } 
    // public static async Task VerifyAnalyzerDotnet100Async([StringSyntax("c#-test")] string source, params DiagnosticResult[] expected)
    // {
    //     await VerifyAnalyzerAsync(source, ReferenceAssemblies.Net.Net100, expected);
    // }
    //
    // public static async Task VerifyAnalyzerDotnet90Async([StringSyntax("c#-test")] string source, params DiagnosticResult[] expected)
    // {
    //     await VerifyAnalyzerAsync(source, ReferenceAssemblies.Net.Net90, expected);
    // }
    // public static async Task VerifyAnalyzerAspnet90Async([StringSyntax("c#-test")] string source, Func<ReferenceAssemblies,ReferenceAssemblies> referenceConfiguration, params DiagnosticResult[] expected)
    // {
    //     await VerifyAnalyzerAsync(source, referenceConfiguration(AspNet90ReferenceAssemblies), expected);
    // } 
    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    public static async Task VerifyAnalyzerAsync([StringSyntax("c#-test")] string source, ReferenceAssemblies referenceAssemblies, params DiagnosticResult[] expected)
    {
        // referenceAssemblies = referenceAssemblies.AddPackage("TUnit.Core")
        var test = new Test
        {
            TestCode = source,
            ReferenceAssemblies = referenceAssemblies,
            TestState =
            {
                // AdditionalReferences =
                // {
                //     typeof(TUnitAttribute).Assembly.Location,
                //     typeof(AssertionBuilder).Assembly.Location,
                // },
            },
            CompilerDiagnostics = CompilerDiagnostics.Warnings
        };
        test.DisabledDiagnostics.AddRange(["CS1701","CS1591","CS0067","CS0169","CS0414"]);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}