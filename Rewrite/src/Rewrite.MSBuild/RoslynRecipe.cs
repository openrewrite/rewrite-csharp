using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using NMica.Utils.IO;
using Rewrite.Core;
using Rewrite.Core.Config;
using Rewrite.RewriteJava.Tree;
using Rewrite.RewriteText;

namespace Rewrite.MSBuild;

[DisplayName("Roslyn Fixup Recipe Runner")]
[Description("Executes Roslyn's Fixups as OpenRewrite Recipes")]
public class RoslynRecipe : ScanningRecipe<object>
{
    [DisplayName("Recipe Assembly")]
    [Description("Assembly containing recipe assembly")]
    public required Assembly RecipeAssembly { get; set; }
    
    [DisplayName("Description")]
    [Description("A special sign to specifically highlight the class found by the recipe")]
    public required string DiagnosticId { get; set; }

    [DisplayName("Should Apply Fixer")]
    [Description("If true, a code fix will be applied, otherwise only location of issues will be reported")]
    public bool ApplyFixer { get; set; } = true;
    
    [DisplayName("Solution File Path")]
    [Description("The path to solution on which recipe is to be run")]
    public required string SolutionFilePath { get; set; }
    public override Tree GetInitialValue(IExecutionContext ctx)
    {
        return J.Empty.Create();
    }

    public override ITreeVisitor<Tree, IExecutionContext> GetScanner(object acc)
    {
        return ITreeVisitor<Tree, IExecutionContext>.Noop();
    }

    public override ICollection<SourceFile> Generate(object acc, IExecutionContext ctx)
    {
        var dir = (AbsolutePath)Assembly.GetExecutingAssembly().Location / "tests";
        return base.Generate(acc, ctx);
    }

    /// <summary>
    /// Executes the recipe against source code stored in <see cref="SolutionFilePath"/>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Documents that have been changed</returns>
    public async Task<IList<Document>> Execute(CancellationToken cancellationToken)
    {
        Stopwatch watch = new();
        watch.Start();
        var workspace = MSBuildWorkspace.Create();

        var originalSolution = await workspace.OpenSolutionAsync(SolutionFilePath, cancellationToken: cancellationToken);
        var solution = new SolutionHolder(originalSolution);
        var analyzerAssembly = RecipeAssembly;
   
        var allAnalyzers = analyzerAssembly
            .GetTypes()
            .Where(x => typeof(DiagnosticAnalyzer).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<DiagnosticAnalyzer>()
            .ToImmutableArray();
        
        var fixers = analyzerAssembly
            .GetTypes()
            .Where(x => typeof(CodeFixProvider).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<CodeFixProvider>()
            .Where(x => x.FixableDiagnosticIds.Any())
            .SelectMany(fixer => fixer.FixableDiagnosticIds.Select(id => (Id: id, Fixer: fixer)))
            .ToDictionary(x => x.Id, x => x.Fixer);

        var analyzersWithFixersById = allAnalyzers
            .SelectMany(analyzer => analyzer
                .SupportedDiagnostics
                .DistinctBy(x => x.Id)
                .Select(descriptor => (descriptor.Id, Analyzer: analyzer)))
            .Join(fixers, x => x.Id, x => x.Key, (a, b) => (a.Id, a.Analyzer, Fixer: b.Value))
            .ToDictionary(x => x.Id, x => x);

        var analyzersToRun = analyzersWithFixersById
            .Where(x => x.Key == DiagnosticId)
            .Select(x => x.Value.Analyzer)
            .Distinct()
            .ToImmutableArray();

        if (analyzersToRun.Length == 0)
        {
            Console.Error.WriteLine($"No analyzers targeting issue {DiagnosticId} has been found");
            return new List<Document>();
        }
        
        Console.WriteLine($"Solution loaded in {watch.Elapsed}");
        Console.WriteLine($"Found {analyzersToRun.Length} code fixers");
        foreach (var fixer in analyzersWithFixersById.Select(x => x.Value.Fixer).Distinct())
        {
            Console.WriteLine($"- {fixer}");
        }

        foreach (var analyzerWithFixer in analyzersToRun)
        {
            var recipeWatch = Stopwatch.StartNew();
            var diagnostics = new List<Diagnostic>();
            var compilationTasks = solution.Solution.Projects.Select(p => p.GetCompilationAsync(cancellationToken));
            var compilations = await Task.WhenAll(compilationTasks);


            foreach (var compilation in compilations.Where(x => x != null).Cast<Compilation>())
            {
                var withAnalyzers = compilation.WithAnalyzers([analyzerWithFixer]);
                var diags = await withAnalyzers.GetAnalyzerDiagnosticsAsync();
                diags = diags.Where(x => solution.Solution.GetDocument(x.Location.SourceTree) is not SourceGeneratedDocument).ToImmutableArray();
                diagnostics.AddRange(diags);
            }

            var diagnosticsById = diagnostics.ToLookup(x => x.Id);

            var diagnosticsToProcess = diagnosticsById.ToList();
            foreach (var diagnosticIssue in diagnosticsToProcess)
            {
                var diagnostic = diagnosticIssue.First();
                var document = solution.Solution.GetDocument(diagnostic.Location.SourceTree) ?? throw new Exception($"Could not find document associated with {diagnostic.Id} {diagnostic.Descriptor.Title}");

                var diagnosticsByDocument = diagnosticIssue
                    .Select(x => (Diagnostic: x, Document: solution.Solution.GetDocument(x.Location.SourceTree)))
                    .Where(x => x.Document != null)
                    .GroupBy(x => x.Document!, x => x.Diagnostic)
                    .Where(x => x.Key is not SourceGeneratedDocument)
                    .ToImmutableDictionary(x => x.Key, x => x.ToImmutableArray());



                var diagnosticProvider = new FixMultipleDiagnosticProvider(diagnosticsByDocument);
                var codeFixProvider = analyzersWithFixersById[diagnosticIssue.Key].Fixer;
                var fixAllProvider = codeFixProvider.GetFixAllProvider() ?? throw new InvalidOperationException($"Bulk fix provider not available for {diagnosticIssue.Key}: {diagnosticIssue.First().Descriptor.Title}");

                Console.WriteLine(
                    $"Fixing {diagnosticIssue.Key}: '{diagnosticIssue.First().Descriptor.Title}' using {codeFixProvider.GetType().Name} in {diagnosticsByDocument.Keys.Count()} documents ({diagnosticsToProcess.SelectMany(x => x).Count()} occuances)");

                var actions = new List<CodeAction>();
                var context = new CodeFixContext(document, diagnostic, (a, d) => actions.Add(a), CancellationToken.None);
                await codeFixProvider.RegisterCodeFixesAsync(context);
                if (actions.Count == 0)
                {
                    Console.WriteLine("No code fixes found");
                    continue;
                }

                var codeFixAction = actions[0];
                if (!codeFixAction.NestedActions.IsEmpty)
                {
                    Console.WriteLine("  Skipping refactoring for this recipe because there's multiple variations of refactoring that can be applied");
                    continue;
                }

                var fixAllContext = new FixAllContext(
                    diagnosticsByDocument.Keys.First(),
                    codeFixProvider,
                    FixAllScope.Solution,
                    actions.First().EquivalenceKey,
                    [diagnosticIssue.Key],
                    diagnosticProvider,
                    cancellationToken);

                var codeAction = await fixAllProvider.GetFixAsync(fixAllContext) ?? throw new Exception("Code action was not found");

                var operations = await codeAction.GetOperationsAsync(cancellationToken);
                var applyChangesOperation = operations.OfType<ApplyChangesOperation>().First();
                var newSolution = applyChangesOperation.ChangedSolution;
                // var affectedDocumentIds = await GetChangedDocumentsAsync(solution.Solution, newSolution, cancellationToken);
                // var affectedDocuments = affectedDocumentIds
                //     .Select(docId => solution.Solution.GetDocument(docId)?.FilePath)
                //     .Where(x => x != null)
                //     .Select(x => ((AbsolutePath)SolutionFilePath).GetRelativePathTo((AbsolutePath)x));
                // changedFiles.AddRange(affectedDocuments);
                // foreach (var docId in affectedDocuments.Take(1))
                // {
                //     var before = (await solution.Solution.GetDocument(docId)!.GetTextAsync()).ToString();
                //     var after = (await newSolution.GetDocument(docId)!.GetTextAsync()).ToString();
                //
                //     var diffs = StringDiffer.GetDifferences(before, after);
                //     Console.WriteLine(solution.Solution.GetDocument(docId).FilePath);
                //     Console.WriteLine("======");
                //     Console.WriteLine(diffs);
                // }

                solution.Solution = newSolution;
                Console.WriteLine(recipeWatch);
                recipeWatch.Stop();
            }
        }
        var affectedDocumentIds = await GetChangedDocumentsAsync(originalSolution, solution.Solution, cancellationToken);
        var affectedDocuments = affectedDocumentIds
            .Select(docId => solution.Solution.GetDocument(docId)!)
            .ToList();
        watch.Stop();
        Console.WriteLine(watch.Elapsed);
        var result = workspace.TryApplyChanges(solution.Solution);
        return affectedDocuments;
    }

    static async Task<IEnumerable<DocumentId>> GetChangedDocumentsAsync(
        Solution oldSolution,
        Solution newSolution,
        CancellationToken cancellationToken = default)
    {
        var changedDocumentIds = new List<DocumentId>();

        foreach (var projectId in newSolution.ProjectIds)
        {
            var oldProject = oldSolution.GetProject(projectId);
            var newProject = newSolution.GetProject(projectId);

            if (oldProject == null || newProject == null)
                continue;

            foreach (var documentId in newProject.DocumentIds)
            {
                var oldDoc = oldProject.GetDocument(documentId);
                var newDoc = newProject.GetDocument(documentId);

                if (oldDoc == null || newDoc == null)
                    continue;

                var oldText = await oldDoc.GetTextAsync(cancellationToken);
                var newText = await newDoc.GetTextAsync(cancellationToken);

                if (!oldText.ContentEquals(newText))
                {
                    changedDocumentIds.Add(documentId);
                }
            }
        }

        return changedDocumentIds;
    }
    
    class SolutionHolder(Solution solution)
    {
        public Solution Solution { get; set; } = solution;
    }

}