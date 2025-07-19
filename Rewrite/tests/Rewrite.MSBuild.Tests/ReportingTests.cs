using System.Diagnostics.CodeAnalysis;
using NuGet.Configuration;
using Rewrite.Core;
using Serilog;

namespace Rewrite.MSBuild.Tests;

public class ReportingTests :  BaseTests
{
    [Test]
    [Explicit]
    public async Task RecipePackageAnalysisReport()
    {
        Log.Logger = new LoggerConfiguration().CreateLogger();

        var recipeManager = CreateObject<RecipeManager>();
        string[] feeds =
        [
            "https://api.nuget.org/v3/index.json"
        ];
        
        var packageSources = feeds.Select(x => new PackageSource(x)).ToList();
        var packages =  new []
        {
            "Microsoft.CodeAnalysis.NetAnalyzers", //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories
            "Roslynator.Analyzers", //https://github.com/dotnet/roslynator
            "Meziantou.Analyzer", //https://github.com/meziantou/Meziantou.Analyzer
            "StyleCop.Analyzers",
            // "SonarAnalyzer.CSharp", 
            // "AsyncFixer", 
            // "ErrorProne.NET.CoreAnalyzers",
            "WpfAnalyzers",
        };
        var totalRecipes = 0;
        foreach (var nugetPackage in packages)
        {
            var packageInfo = await recipeManager.InstallRecipePackage(nugetPackage, includePrerelease: false);
            totalRecipes += packageInfo.Recipes.Count;
            Console.WriteLine($"{packageInfo.Package}: {packageInfo.Recipes.Count}");
        }
        Console.WriteLine($"Total: {totalRecipes}");

    }
}