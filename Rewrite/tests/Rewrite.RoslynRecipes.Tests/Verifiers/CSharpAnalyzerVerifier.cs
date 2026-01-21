using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Rewrite.RoslynRecipes.Tests.Verifiers;

public static partial class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public class Test : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    {
        public Test()
        {
            SolutionTransforms.Insert(0, (solution, projectId) =>
            {
                var project = solution.GetProject(projectId);

                if (project is null)
                {
                    return solution;
                }

                var compilationOptions = project.CompilationOptions as CSharpCompilationOptions;

                if (compilationOptions is null)
                {
                    return solution;
                }

                if (project.ParseOptions is not CSharpParseOptions parseOptions)
                {
                    return solution;
                }
                
                compilationOptions = compilationOptions.WithOutputKind(
                    IsExecutable ? OutputKind.ConsoleApplication : OutputKind.DynamicallyLinkedLibrary);
                
                compilationOptions = compilationOptions
                    .WithNullableContextOptions(NullableContextOptions.Enable)
                    .WithSpecificDiagnosticOptions(compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));

                solution = solution.WithProjectCompilationOptions(projectId, compilationOptions)
                    .WithProjectParseOptions(projectId, parseOptions
                        .WithLanguageVersion(LanguageVersion.Preview)
                    );

                return solution;
            });
        }
        public bool IsExecutable { get; set; }
    }
}