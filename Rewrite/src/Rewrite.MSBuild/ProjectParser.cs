using System.Reflection;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using NuGet.Configuration;
using NuGet.Frameworks;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteJava.Tree;
using Serilog;
using static Rewrite.RewriteJava.Tree.Space;
using FileAttributes = Rewrite.Core.FileAttributes;
using Project = Microsoft.Build.Evaluation.Project;

namespace Rewrite.MSBuild;

public class ProjectParser
{

    static ProjectParser()
    {
        if (!MSBuildLocator.CanRegister) return;

        var instance = MSBuildLocator.QueryVisualStudioInstances().OrderByDescending(instance => instance.Version)
            .First();
        MSBuildLocator.RegisterInstance(instance);
    }

    public static bool Init()
    {
        if (!MSBuildLocator.CanRegister) return false;

        var instance = MSBuildLocator.QueryVisualStudioInstances().OrderByDescending(instance => instance.Version)
            .First();
        MSBuildLocator.RegisterInstance(instance);
        return true;
    }

    private readonly Project _project;
    private readonly IList<Parser.Input> _inputs = [];
    private readonly Parser _parser;
    private readonly string solutionDir;


    public ProjectParser(string projectFilePath, string solutionDir)
    {
        var projectOptions = new ProjectOptions();
        // projectOptions.GlobalProperties = new Dictionary<string, string>
        // {
        //     { "Configuration", "Debug" }
        // };
        // _project = Project.FromFile(projectFilePath, projectOptions);

        // FIXME: do we need to unload project then?
        _project =
            ProjectCollection.GlobalProjectCollection.LoadedProjects.FirstOrDefault(d =>
                d.FullPath == projectFilePath) ??
            Project.FromFile(projectFilePath, projectOptions);
        this.solutionDir = solutionDir;
        var settings = Settings.LoadDefaultSettings(root: null);
        var targetFramework = _project.GetPropertyValue("TargetFramework")!;
        NuGetFramework.ParseFolder(targetFramework);

        var projectItems = _project.GetItems("Compile").ToList();
        foreach (var projectItem in projectItems)
        {
            _inputs.Add(new Parser.Input(Path.Combine(projectItem.Project.DirectoryPath, projectItem.EvaluatedInclude),
                () => new FileStream(Path.Combine(projectItem.Project.DirectoryPath, projectItem.EvaluatedInclude),
                    FileMode.Open)));
        }

        projectItems = _project.GetItems("ProjectReference").ToList();
        IList<MetadataReference> references = [];
        foreach (var item in projectItems)
        {
            var include = item.EvaluatedInclude;
            // var reference = MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(item.Project.FullPath)!,
            //     _project.GetPropertyValue("OutputPath").Replace('\\', Path.DirectorySeparatorChar), Path.GetFileName(include)));
            var reference =
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(item.Project.FullPath)!, include));
            references.Add(reference);
        }

        projectItems = _project.GetItems("PackageReference").ToList();
        foreach (var item in projectItems)
        {
            var include = item.EvaluatedInclude;
            var assemblyName = new AssemblyName(include);
            var assembly = Assembly.Load(assemblyName);
            var reference = MetadataReference.CreateFromFile(assembly.Location);
            references.Add(reference);
        }

        var builder = new CSharpParser.Builder();
        builder = builder.References(references);
        foreach (var reference in references)
        {
            Console.WriteLine(reference.Display);
        }

        _parser = builder.Build();
    }

    public IEnumerable<SourceFile> ParseSourceFiles()
    {
        return _parser.ParseInputs(_inputs, solutionDir, new InMemoryExecutionContext());
    }
}
