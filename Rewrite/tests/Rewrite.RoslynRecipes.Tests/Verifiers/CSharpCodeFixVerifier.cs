using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Rewrite.RoslynRecipes.Tests.Verifiers;

public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
    {
        public Test()
        {
            OptionsTransforms.Add(options => options
                .WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, false)
                .WithChangedOption(CSharpFormattingOptions.IndentBlock, true)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, true)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true)
            );
            
            SolutionTransforms.Insert(0, (solution, projectId) =>
            {
                var project = solution.GetProject(projectId);
                
                if (project is null) 
                {
                    return solution;
                }

                if (project.CompilationOptions is not CSharpCompilationOptions compilationOptions)
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

                solution = solution
                    .WithProjectCompilationOptions(projectId, compilationOptions)
                    .WithProjectParseOptions(projectId, parseOptions.WithLanguageVersion(LanguageVersion.Preview));

                return solution;
            });
        }

        public bool IsExecutable { get; set; }
    }
}