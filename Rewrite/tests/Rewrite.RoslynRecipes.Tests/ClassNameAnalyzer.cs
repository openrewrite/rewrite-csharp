using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
#pragma warning disable RS1038 // Compiler extensions in test assembly
#pragma warning disable RS1041 // Target framework for compiler extension
#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Rewrite.RoslynRecipes.Tests;

/// <summary>
/// Test analyzer that highlights class names in class declarations.
/// Used for testing the ApplyAnalyzerHighlights functionality.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ClassNameAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "TEST001";

    private static readonly LocalizableString Title = "Class declaration found";
    private static readonly LocalizableString MessageFormat = "Class '{0}' declared";
    private static readonly LocalizableString Description = "Reports all class declarations for testing purposes.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        "Testing",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var className = classDeclaration.Identifier;

        var diagnostic = Diagnostic.Create(Rule, className.GetLocation(), className.Text);
        context.ReportDiagnostic(diagnostic);
    }
}
