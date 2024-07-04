using System.Reflection;
using Microsoft.Build.Definition;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using NuGet.Configuration;
using NuGet.Frameworks;
using Rewrite.Core;
using Rewrite.RewriteCSharp;
using Project = Microsoft.Build.Evaluation.Project;

namespace Rewrite.MSBuild;

public class ProjectParser
{
    static ProjectParser()
    {
        var instance = MSBuildLocator.QueryVisualStudioInstances().OrderByDescending(instance => instance.Version)
            .First();
        MSBuildLocator.RegisterInstance(instance);
    }

    private readonly Project _project;
    private readonly IList<Parser.Input> _inputs = [];
    private readonly Parser _parser;


    public ProjectParser(string projectFilePath)
    {
        var projectOptions = new ProjectOptions();
        // projectOptions.GlobalProperties = new Dictionary<string, string>
        // {
        //     { "Configuration", "Debug" }
        // };
        _project = Project.FromFile(projectFilePath, projectOptions);
        var settings = Settings.LoadDefaultSettings(root: null);
        var targetFramework = _project.GetPropertyValue("TargetFramework")!;
        NuGetFramework.ParseFolder(targetFramework);

        var projectItems = _project.GetItems("Compile").ToList();
        foreach (var projectItem in projectItems)
        {
            _inputs.Add(new Parser.Input(projectItem.EvaluatedInclude,
                () => new FileStream(projectItem.EvaluatedInclude, FileMode.Open)));
        }

        projectItems = _project.GetItems("ProjectReference").ToList();
        IList<MetadataReference> references = [];
        foreach (var item in projectItems)
        {
            var include = item.EvaluatedInclude;
            var reference = MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(_project.FullPath)!,
                _project.GetPropertyValue("OutputPath"), Path.GetFileName(include).Replace(".csproj", ".dll")));
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
        _parser = builder.Build();
    }

    public static void Main(string[] args)
    {
        var lstBuilder = new ProjectParser(args.Length > 0 ? args[0] : "Rewrite.MSBuild.csproj");
        var sourceFiles = lstBuilder.ParseSourceFiles();

        foreach (var sourceFile in sourceFiles)
        {
            Console.WriteLine(sourceFile);
        }
    }

    public IEnumerable<SourceFile> ParseSourceFiles()
    {
        return _parser.ParseInputs(_inputs, ".", new InMemoryExecutionContext());
    }
}