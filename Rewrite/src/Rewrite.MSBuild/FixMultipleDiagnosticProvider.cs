using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Rewrite.MSBuild;

internal sealed class FixMultipleDiagnosticProvider : FixAllContext.DiagnosticProvider
{
    public ImmutableDictionary<Document, ImmutableArray<Diagnostic>> DocumentDiagnosticsMap { get; }
    public ImmutableDictionary<Project, ImmutableArray<Diagnostic>> ProjectDiagnosticsMap { get; }

    public FixMultipleDiagnosticProvider(ImmutableDictionary<Document, ImmutableArray<Diagnostic>> diagnosticsMap)
    {
        DocumentDiagnosticsMap = diagnosticsMap;
        ProjectDiagnosticsMap = ImmutableDictionary<Project, ImmutableArray<Diagnostic>>.Empty;
    }

    public FixMultipleDiagnosticProvider(ImmutableDictionary<Project, ImmutableArray<Diagnostic>> diagnosticsMap)
    {
        ProjectDiagnosticsMap = diagnosticsMap;
        DocumentDiagnosticsMap = ImmutableDictionary<Document, ImmutableArray<Diagnostic>>.Empty;
    }

    public override Task<IEnumerable<Diagnostic>> GetAllDiagnosticsAsync(Project project, CancellationToken cancellationToken)
    {
        var allDiagnosticsBuilder = new List<Diagnostic>();
        ImmutableArray<Diagnostic> diagnostics;
        if (!DocumentDiagnosticsMap.IsEmpty)
        {
            foreach (var document in project.Documents)
            {
                if (DocumentDiagnosticsMap.TryGetValue(document, out diagnostics))
                {
                    allDiagnosticsBuilder.AddRange(diagnostics);
                }
            }
        }

        if (ProjectDiagnosticsMap.TryGetValue(project, out diagnostics))
        {
            allDiagnosticsBuilder.AddRange(diagnostics);
        }

        return Task.FromResult<IEnumerable<Diagnostic>>(allDiagnosticsBuilder);
    }

    public override Task<IEnumerable<Diagnostic>> GetDocumentDiagnosticsAsync(Document document, CancellationToken cancellationToken)
    {
        if (DocumentDiagnosticsMap.TryGetValue(document, out var diagnostics))
        {
            return Task.FromResult<IEnumerable<Diagnostic>>(diagnostics);
        }

        return Task.FromResult(Enumerable.Empty<Diagnostic>());
    }

    public override Task<IEnumerable<Diagnostic>> GetProjectDiagnosticsAsync(Project project, CancellationToken cancellationToken)
    {
        if (ProjectDiagnosticsMap.TryGetValue(project, out var diagnostics))
        {
            return Task.FromResult<IEnumerable<Diagnostic>>(diagnostics);
        }

        return Task.FromResult(Enumerable.Empty<Diagnostic>());
    }
}