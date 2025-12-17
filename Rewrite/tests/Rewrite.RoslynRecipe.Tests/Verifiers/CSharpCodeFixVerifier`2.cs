using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using TUnit.Assertions.AssertionBuilders;

namespace Rewrite.RoslynRecipe.Tests.Verifiers;

public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic()
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic();

    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic(string diagnosticId)
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(diagnosticId);

    /// <inheritdoc cref="Microsoft.CodeAnalysis.Diagnostic"/>
    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(descriptor);

    // public static async Task VerifyCodeFixDotnet100Async([StringSyntax("c#-test")] string source, [StringSyntax("c#-test")] string fixedSource)
    // {
    //     await VerifyCodeFixAsync(source, fixedSource, ReferenceAssemblies.Net.Net100);
    // }
    // public static async Task VerifyCodeFixDotnet100Async([StringSyntax("c#-test")] string source, [StringSyntax("c#-test")] string fixedSource, Func<ReferenceAssemblies,ReferenceAssemblies> referenceConfiguration)
    // {
    //     await VerifyCodeFixAsync(source, fixedSource, referenceConfiguration(ReferenceAssemblies.Net.Net100));
    // }
    // public static async Task VerifyCodeFixDotnet90Async([StringSyntax("c#-test")] string source, [StringSyntax("c#-test")] string fixedSource)
    // {
    //     await VerifyCodeFixAsync(source, fixedSource, ReferenceAssemblies.Net.Net90);
    // }
    //
    // public static async Task VerifyCodeFixDotnet90Async([StringSyntax("c#-test")] string source, [StringSyntax("c#-test")] string fixedSource, Func<ReferenceAssemblies,ReferenceAssemblies> referenceConfiguration)
    // {
    //     await VerifyCodeFixAsync(source, fixedSource, referenceConfiguration(ReferenceAssemblies.Net.Net90));
    // }

    public static async Task VerifyCodeFixAsync(
        [StringSyntax("c#-test")] string source,
        [StringSyntax("c#-test")] string fixedSource,
        ReferenceAssemblies referenceAssemblies)
    {
        var cu = (CompilationUnitSyntax)await CSharpSyntaxTree.ParseText(CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(source)).GetRootAsync();
        var hasGlobalStatements = cu.DescendantNodes().OfType<GlobalStatementSyntax>().Any();
        var test = new Test
        {
            IsExecutable = hasGlobalStatements,
            TestCode = source,
            FixedCode = fixedSource,
            ReferenceAssemblies = referenceAssemblies,
            // helps deal with scenarios where we're modifying part of member access expression with something that has conditional null propagation
            CodeActionValidationMode = CodeActionValidationMode.None,  
            TestState =
            {
                AdditionalReferences =
                {
                    typeof(TUnitAttribute).Assembly.Location,
                    typeof(AssertionBuilder).Assembly.Location,
                },
            },
            CompilerDiagnostics = CompilerDiagnostics.Errors
        };

        await test.RunAsync(CancellationToken.None);
    }
}