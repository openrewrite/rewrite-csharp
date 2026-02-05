using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.NET.Build.Tasks;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Nuke.Common.IO;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild.Tests;

public class ReceiptExecutionContextTests
{
    [Test]
    public async Task Test1(CancellationToken ct)
    {

        var recipeManager = new RecipeManager(NullLoggerFactory.Instance, new NugetLogger(NullLogger<NugetLogger>.Instance));
        var deps = DependencyContext.Default!;
    
        //NuGetFramework.ParseFrameworkName(".NETStandard,Version=v2.0", DefaultFrameworkNameProvider.Instance).Dump();
        //new NuGetFramework(.ParseFrameworkName("netstandard2.0,Version=v2.0", DefaultFrameworkNameProvider.Instance).Dump();
        //return;
        LibraryRange[] requestedPackages =
        [
            new LibraryRange("Microsoft.CodeAnalysis.CSharp.CodeStyle", VersionRange.AllStable, LibraryDependencyTarget.Package),
            // new LibraryRange("Microsoft.CodeAnalysis.Common", VersionRange.AllStable, LibraryDependencyTarget.Package),
            // new LibraryRange("Microsoft.CodeAnalysis.Workspaces.Common", VersionRange.AllStable, LibraryDependencyTarget.Package),
            //new LibraryRange("Microsoft.CodeAnalysis.Common", VersionRange.Parse("3.11.0"), LibraryDependencyTarget.Package),
            //new LibraryRange("Microsoft.Extensions.Logging", VersionRange.Parse("10.0.0"), LibraryDependencyTarget.Package)
        ];
        // var lockFile = await recipeManager.CreateLockFile(
        //     [
        //         new LibraryRange("Microsoft.CodeAnalysis.CSharp.CodeStyle", VersionRange.AllStable, LibraryDependencyTarget.Package),
        //         new LibraryRange("Microsoft.CodeAnalysis.Common", VersionRange.AllStable, LibraryDependencyTarget.Package),
        //         new LibraryRange("Microsoft.CodeAnalysis.Workspaces.Common", VersionRange.AllStable, LibraryDependencyTarget.Package),
        //         //new LibraryRange("Microsoft.CodeAnalysis.Common", VersionRange.Parse("3.11.0"), LibraryDependencyTarget.Package),
        //         //new LibraryRange("Microsoft.Extensions.Logging", VersionRange.Parse("10.0.0"), LibraryDependencyTarget.Package)
        //     ],
        //     ct);
        var executionContext = await recipeManager.CreateExecutionContext(requestedPackages,  ct);
        Console.WriteLine($"{executionContext.Recipes.Count(x => x.Kind == RecipeKind.RoslynAnalyzer)}/{executionContext.Recipes.Count(x => x.Kind != RecipeKind.RoslynAnalyzer)}");
        // var task = new ResolvePackageAssets();
        // task.LockFile = lockFile;
        // task.TargetFramework = "netstandard2.0";
        // task.ProjectLanguage = "C#";
        // task.Execute();
        // //task.Analyzers.Select(x => (x.ItemSpec, x.MetadataNames.Cast<string>().Select(metadataName => (metadataName, x.GetMetadata(metadataName))))).Dump();
        // var conflictResolver = new ResolvePackageFileConflicts();
        // conflictResolver.Analyzers = task.Analyzers;
        // // conflictResolver.References = task.CompileTimeAssemblies;
        // conflictResolver.Execute();
        // var paths = (conflictResolver.AnalyzersWithoutConflicts ?? []).Select(x => (AbsolutePath)x.ItemSpec);
        // var executionContext = new RecipeExecutionContext(paths.ToList(), NullLoggerFactory.Instance);

    }
}