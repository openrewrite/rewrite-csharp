using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Rewrite.Core;
using Rewrite.RewriteCSharp;

namespace Rewrite.MSBuild;

public class SolutionParser
{
    public async Task<Solution> LoadSolutionAsync(string solutionFilePath, CancellationToken cancellationToken)
    {
        var workspace = MSBuildWorkspace.Create();
        return await workspace.OpenSolutionAsync(solutionFilePath, cancellationToken: cancellationToken);
    }

    public IEnumerable<SourceFile> ParseProjectSources(Solution solution, string projectName, string rootDir,
        InMemoryExecutionContext ctx)
    {
        var project = solution.Projects.First(it => it.FilePath!.Equals(projectName));
        var fullProjectPath = Path.GetFullPath(Path.GetDirectoryName(project.FilePath!)!);
        var objPath = Path.Combine(Path.GetDirectoryName(project.FilePath!)!, "obj");
        var builder = new CSharpParser.Builder();
        var allMetadataReferences = GetAllMetadataReferences(project);
        var parser = builder.References(allMetadataReferences).Build();

        IList<Parser.Input> inputs = [];
        foreach (var doc in project.Documents
                     .Where(d => d.SourceCodeKind == SourceCodeKind.Regular)
                     .Where(d => Path.GetFullPath(d.FilePath!).StartsWith(fullProjectPath))
                     .Where(d => !d.FilePath!.StartsWith(objPath)))
        {
            inputs.Add(new Parser.Input(Path.GetFullPath(doc.FilePath!),
                () => new FileStream(doc.FilePath!, FileMode.Open)));
        }

        var sourceFiles = parser.ParseInputs(inputs, rootDir, ctx);
        return sourceFiles;
    }

    private static IEnumerable<MetadataReference> GetAllMetadataReferences(Project project)
    {
        var allReferences = new HashSet<MetadataReference>();
        var projectsToProcess = new Queue<Project>();
        projectsToProcess.Enqueue(project);

        while (projectsToProcess.Count > 0)
        {
            var currentProject = projectsToProcess.Dequeue();

            // Add metadata references of the current project
            foreach (var reference in currentProject.MetadataReferences)
                allReferences.Add(reference);

            // Queue up the referenced projects for processing
            foreach (var projectReference in currentProject.ProjectReferences)
            {
                var referencedProject = project.Solution.GetProject(projectReference.ProjectId);
                if (referencedProject != null && !projectsToProcess.Contains(referencedProject))
                    projectsToProcess.Enqueue(referencedProject);
            }
        }

        return allReferences;
    }
}
