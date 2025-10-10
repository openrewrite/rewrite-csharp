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
using Microsoft.Extensions.Logging;
using NMica.Utils.IO;
using Nuke.Common.Tooling;
using Rewrite.Core;
using Rewrite.Core.Config;
using Rewrite.RewriteJava.Tree;
using Rewrite.RewriteText;

namespace Rewrite.MSBuild;

[DisplayName("Roslyn Fixup Recipe Runner")]
[Description("Executes Roslyn's Fixups as OpenRewrite Recipes")]
public class RoslynRecipe(IEnumerable<Assembly> recipeAssemblies, ILogger<RoslynRecipe> log) : ScanningRecipe<object>
{
    internal List<Assembly> RecipeAssemblies { get; set; } = recipeAssemblies.ToList();
    
    [DisplayName("Description")]
    [Description("A roslyn analyzer id to target for refactoring")]
    public string DiagnosticId { get => DiagnosticIds.First(); set => DiagnosticIds.Add(value); }

    [DisplayName("Description")]
    [Description("A special sign to specifically highlight the class found by the recipe")]
    public HashSet<string> DiagnosticIds { get; init; } = new();

    [DisplayName("Should Apply Fixer")]
    [Description("If true, a code fix will be applied, otherwise only location of issues will be reported")]
    public bool ApplyFixer { get; set; } = true;
    
    [DisplayName("Solution File Path")]
    [Description("The path to solution on which recipe is to be run")]
    public required string SolutionFilePath { get; set; }

    [DisplayName("Dry Run")]
    [Description("Run without applying")]
    public bool DryRun { get; set; } = false;
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
    /// <returns>Documents that have been changed, grouped by issue ID</returns>
    public async Task<RecipeExecutionResult> Execute(CancellationToken cancellationToken)
    {
        List<IssueFixResult> fixedIssues = new();
        Stopwatch watch = new();
        watch.Start();
        var workspace = MSBuildWorkspace.Create();
        var result = ProcessTasks.StartProcess("dotnet", $"restore {SolutionFilePath}");
        result.WaitForExit();
        if(result.ExitCode != 0)
            Debugger.Break();
        var originalSolution = await workspace.OpenSolutionAsync(SolutionFilePath, cancellationToken: cancellationToken);
        var solution = new SolutionHolder(originalSolution);
        // var analyzerAssembly = RecipeAssemblies;
   
        var allAnalyzers = RecipeAssemblies
            .SelectMany(x => x.ExportedTypes)
            .Where(x => typeof(DiagnosticAnalyzer).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<DiagnosticAnalyzer>()
            .ToImmutableArray();
        
        var fixers = RecipeAssemblies
            .SelectMany(x => x.ExportedTypes)
            .Where(x => typeof(CodeFixProvider).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<CodeFixProvider>()
            .Where(x => x.FixableDiagnosticIds.Any())
            .SelectMany(fixer => fixer.FixableDiagnosticIds.Select(id => (Id: id, Fixer: fixer)))
            .DistinctBy(x => x.Id)
            .ToDictionary(x => x.Id, x => x.Fixer);

        var analyzersWithFixersById = allAnalyzers
            .SelectMany(analyzer => analyzer
                .SupportedDiagnostics
                .DistinctBy(x => x.Id)
                .Select(descriptor => (descriptor.Id, Analyzer: analyzer)))
            .Join(fixers, x => x.Id, x => x.Key, (a, b) => (a.Id, a.Analyzer, Fixer: b.Value))
            .Where(x => DiagnosticIds.Contains(x.Id))
            .GroupBy(x => x.Id)
            .Select(x => (x.Key, x.First().Analyzer, x.FirstOrDefault().Fixer))
            .ToDictionary(x => x.Key, x => (x.Analyzer, x.Fixer));
        
        if (analyzersWithFixersById.Count == 0)
        {
            log.LogError("No analyzers targeting issue {DiagnosticId} has been found", DiagnosticId);
            return new RecipeExecutionResult(SolutionFilePath, watch.Elapsed, []);
        }

        // var issuesBeingFixed = analyzersWithFixersById.Select(x =>
        // {
        //     var diagnostic = x.Value.Analyzer.SupportedDiagnostics.First(y => y.Id == x.Key);
        //     return $"{diagnostic.Id}: {diagnostic.Title}";
        // });

        var loadedTime = watch.Elapsed;

        var allDiagnostics = await GetDiagnostics(solution, analyzersWithFixersById, cancellationToken);
        var issuesTypesInCodebase = allDiagnostics
            .Where(x => this.DiagnosticIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToHashSet();
        var analyzersWithFixersByIdForIssuesInCodebase = analyzersWithFixersById
            .Where(x => issuesTypesInCodebase.Contains(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
        
        log.LogDebug("Solution {SolutionFilePath} loaded in {Elapsed}", SolutionFilePath, loadedTime);
        if (analyzersWithFixersByIdForIssuesInCodebase.Count == 0)
        {
            log.LogDebug("No fixable issues found in solution");
        }
        // else
        // {
        //     var issueCounts = allDiagnostics
        //         .GroupBy(x => x.Id)
        //         .Select(x  => new 
        //         {
        //             IssueId = $"{x.Key}: {x.First().Descriptor.Title}",
        //             Occurances = x.Count()
        //         });
        //     log.LogDebug("Fixing {@Issues}", issueCounts);
        // }

        foreach (var (issueId, (analyzer, codeFixProvider)) in analyzersWithFixersByIdForIssuesInCodebase)
        {
            var recipeWatch = Stopwatch.StartNew();
            // var compilationTasks = solution.Solution.Projects.Select(p => p.GetCompilationAsync(cancellationToken));
            // var compilations = await Task.WhenAll(compilationTasks);

            var analyzersToRun = new Dictionary<string, (DiagnosticAnalyzer, CodeFixProvider)>()
            {
                {issueId, (analyzer, codeFixProvider)}
            };
            var diagnostics = await GetDiagnostics(solution, analyzersToRun, cancellationToken);
            diagnostics = diagnostics
                .Where(x => x.Id == issueId)
                .ToList();
            if (diagnostics.Count == 0)
            {
                continue;
            }

            var diagnosticsByDocument = diagnostics
                .Select(x => (Diagnostic: x, Document: solution.Solution.GetDocument(x.Location.SourceTree)))
                .Where(x => x.Document != null)
                .GroupBy(x => x.Document!, x => x.Diagnostic)
                .Where(x => x.Key is not SourceGeneratedDocument)
                .ToImmutableDictionary(x => x.Key, x => x.ToImmutableArray());

            var diagnosticProvider = new FixMultipleDiagnosticProvider(diagnosticsByDocument);
            // var codeFixProvider = analyzersWithFixersById[diagnosticIssue.Key].Fixer;
            var fixAllProvider = codeFixProvider.GetFixAllProvider();

            if (fixAllProvider == null)
            {
                fixAllProvider = WellKnownFixAllProviders.BatchFixer;
                // throw new InvalidOperationException($"Bulk fix provider not available for {issueId}: {diagnostic.Descriptor.Title}");
            }

            Diagnostic? sampledDiagnostic = null;
            string? equivalenceKey = null;
            // try to find first viable fixup type for this issue type
            foreach(var diagnostic in diagnostics)
            {
                var document = solution.Solution.GetDocument(diagnostic.Location.SourceTree);
                if (document == null)
                    throw new Exception($"Could not find document associated with {diagnostic.Id} {diagnostic.Descriptor.Title}");

                var actions = new List<CodeAction>();
                
                var context = new CodeFixContext(document!, diagnostic, (a, d) => actions.Add(a), CancellationToken.None);
                try
                {
                    await codeFixProvider.RegisterCodeFixesAsync(context);
                }
                catch (Exception)
                {
                    // log.LogError(e, "Code fix up for {IssueId} failed due it its own internal logic", issueId);
                    continue;
                }


                if (actions.Count == 0)
                {
                    // log.LogDebug("No fixable issues found");
                    continue;
                }
                
                var codeFixAction = actions[0];
                
                //codeFixAction.
                if (!codeFixAction.NestedActions.IsEmpty)
                {
                    // log.LogWarning("Skipping refactoring of recipe {DiagnosticId} because there's multiple variations of refactoring that can be applied", issueId);
                    continue;
                }
                sampledDiagnostic = diagnostic;
                equivalenceKey = codeFixAction.EquivalenceKey;
                break;
            }

            if (sampledDiagnostic == null)
            {
                // generally we end up here when an analyzer has a fixer associated with it, but fixer determined that it can't actually fix it automatically (didn't register any code actions)
                log.LogDebug("No fixable issues found in solution");
                break;
            }

            log.LogDebug("Fixing {DiagnosticId}: '{Title}' using {TypeName} in {DocumentCount} documents ({OccurrenceCount} occurrences)",
                issueId,
                sampledDiagnostic.Descriptor.Title,
                codeFixProvider.GetType().Name,
                diagnosticsByDocument.Keys.Count(),
                diagnostics.Count()
            );
            
            var fixAllContext = new FixAllContext(
                diagnosticsByDocument.Keys.First(),
                codeFixProvider,
                FixAllScope.Solution,
                equivalenceKey,
                [issueId],
                diagnosticProvider,
                cancellationToken);
            Solution newSolution;
            try
            {
                var codeAction = await fixAllProvider.GetFixAsync(fixAllContext) ?? throw new Exception("Code action was not found");
            
                var operations = await codeAction.GetOperationsAsync(cancellationToken);
                var applyChangesOperation = operations.OfType<ApplyChangesOperation>().First();
                newSolution = applyChangesOperation.ChangedSolution;
            }
            catch (Exception e)
            {
                log.LogError(e, "Unable to apply {IssueId} do to internal CodeFixup logic error", issueId);
                continue;
            }
            
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
            var affectedDocumentIds = await GetChangedDocumentsAsync(originalSolution, solution.Solution, cancellationToken);
            var affectedDocuments = affectedDocumentIds
                .Select(x => (Document: solution.Solution.GetDocument(x.DocumentId)!, x.ChangedLineNumbers))
                .ToList();
            var issueFixResult = new IssueFixResult(
                IssueId: issueId, 
                ExecutionTime: recipeWatch.Elapsed, 
                Fixes: affectedDocuments
                    .Select(x => new DocumentFixResult(x.Document.FilePath!, x.ChangedLineNumbers))
                    .ToList());
            fixedIssues.Add(issueFixResult);
            recipeWatch.Stop();
            
        }

        if (!DryRun)
        {
            workspace.TryApplyChanges(solution.Solution);
        }

        watch.Stop();
        log.LogDebug("Executed recipes in {Elapsed}", watch.Elapsed);
        var recipeExecutionResult = new RecipeExecutionResult(SolutionFilePath, watch.Elapsed, fixedIssues);
        return recipeExecutionResult;
    }

    private async Task<List<Diagnostic>> GetDiagnostics(
        SolutionHolder solution, 
        Dictionary<string, (DiagnosticAnalyzer Analyzer, CodeFixProvider CodeFixProvider)> analyzersWithFixers, 
        CancellationToken cancellationToken)
    {
        var compilationTasks = solution.Solution.Projects.Select(p => p.GetCompilationAsync(cancellationToken));
        var compilations = await Task.WhenAll(compilationTasks);

        List<Diagnostic> diagnostics = [];
        foreach (var compilation in compilations.Where(x => x != null).Cast<Compilation>())
        {
            var withAnalyzers = compilation.WithAnalyzers(analyzersWithFixers
                .Values
                .Select(x => x.Analyzer)
                .Distinct()
                .ToImmutableArray());
            var diags = await withAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken);
            diags = diags
                .Where(x => solution.Solution.GetDocument(x.Location.SourceTree) is not SourceGeneratedDocument)
                .ToImmutableArray();
            diagnostics.AddRange(diags);
        }

        return diagnostics;
    }
    
    static async Task<IEnumerable<(DocumentId DocumentId, List<int> ChangedLineNumbers)>> GetChangedDocumentsAsync(
        Solution oldSolution,
        Solution newSolution,
        CancellationToken cancellationToken = default)
    {
        var changedDocumentIds = new List<(DocumentId, List<int> ChangedLineNumbers)>();

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
                    var diffLineNumbers = DiffHelper.GetDifferentLineNumbers(oldText.ToString(), newText.ToString());
                    changedDocumentIds.Add((documentId,diffLineNumbers));
                    
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