using System.Collections.Frozen;
using static Rewrite.Test.CSharp.Assertions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NMica.Utils.IO;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Nuke.Common;
using Nuke.Common.IO;
using Rewrite.Core;
using Rewrite.Test;
using Rewrite.Test.CSharp;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using AbsolutePath = Nuke.Common.IO.AbsolutePath;

namespace Rewrite.MSBuild.Tests;

public class RecipeManagerTests : BaseTests
{
    // [Before(HookType.Test)]
    // public void Before()
    // {
    //     // var loggerConfig = new LoggerConfiguration()
    //     //     .MinimumLevel.Debug()
    //     //     .Destructure.With<PrettyJsonDestructuringPolicy>()
    //     //     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}", applyThemeToRedirectedOutput: true, theme: AnsiConsoleTheme.Literate);
    //     // var log = loggerConfig.CreateLogger();
    //     var globalPackagesFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
    //     var rewritePackagesInGlobalCache = Directory.EnumerateDirectories(globalPackagesFolder, "Rewrite.*", SearchOption.TopDirectoryOnly)
    //         .Select(x => (AbsolutePath)x)
    //         .ToList();
    //     foreach (var rewritePackagePath in rewritePackagesInGlobalCache)
    //     {
    //         // rewritePackagePath.DeleteDirectory();
    //         for (int i = 0; i < 5; i++)
    //         {
    //             try
    //             {
    //                 if(Directory.Exists(rewritePackagePath))
    //                     Directory.Delete(rewritePackagePath, recursive: true);
    //                 break;
    //             }
    //             catch (UnauthorizedAccessException)
    //             {
    //                 Console.WriteLine($"Skipping rewrite package {rewritePackagePath}");
    //                 // log.Information($"Skipping rewrite package {rewritePackagePath}");
    //                 Thread.Sleep(1000);
    //                 continue;
    //             }
    //         }
    //     }
    // }
    //
    [Test]
    public async Task InstallRecipe()
    {
    
        
        var recipeManager = CreateObject<RecipeManager>();
        
        string[] packageSources =
        [
            NukeBuild.RootDirectory / "artifacts",
            NukeBuild.RootDirectory / "artifacts" / "test",
            "https://api.nuget.org/v3/index.json"
        ];
        
        // var recipeManager = new RecipeManager();
        var recipeIdentity = new InstallableRecipe("Rewrite.Recipes.FindClass", "Rewrite.Recipes", "0.0.1");
        var installable = await recipeManager.InstallRecipePackage(recipeIdentity, packageSources: packageSources.Select(x => new PackageSource(x)).ToList());
        installable.Recipes.Should().NotBeEmpty();
        var lst = Assertions.CSharp(
            """
            public    class Foo;
            """
        ).Parse();
    
        var recipeDescriptor = recipeManager.FindRecipeDescriptor(recipeIdentity);
        var recipeStartInfo = recipeDescriptor.CreateRecipeStartInfo()
            .WithOption("Description", "!!--->");
        var recipe = recipeManager.CreateRecipe(recipeIdentity, recipeStartInfo);
        recipe.Should().NotBeNull();
        recipe.GetType().GetProperty("Description")!.GetValue(recipe).Should().Be("!!--->");
        
        var executionContext = new InMemoryExecutionContext();
        var visitor = recipe.GetVisitor();
        visitor.IsAcceptable(lst, executionContext).Should().BeTrue();
    
        var afterLst = visitor.Visit(lst, executionContext);
        afterLst!.ToString().ShouldBeSameAs($"/*~~(!!--->)~~>*/{lst}");
    }

    
    [Test]
    [Explicit]
    public async Task InstallRoslynRecipe()
    {
        var recipeManager = CreateObject<RecipeManager>();
        
        string[] feeds =
        [
            "https://api.nuget.org/v3/index.json"
        ];
        
        var packageSources = feeds.Select(x => new PackageSource(x)).ToList();
        // var recipeManager = new RecipeManager();
        // CA1802: Use Literals Where Appropriate
        // CA1861: Avoid constant arrays as arguments
        var installableRecipe = new InstallableRecipe("CA1861", "Microsoft.CodeAnalysis.NetAnalyzers", "9.0.0");
        var recipesPackage = await recipeManager.InstallRecipePackage(installableRecipe, packageSources);
        recipesPackage.Recipes.Should().NotBeEmpty();
        var recipeDescriptor = recipesPackage.GetRecipeDescriptor(installableRecipe.Id);
        var recipeStartInfo = recipeDescriptor.CreateRecipeStartInfo();
        recipeStartInfo.WithOption(nameof(RoslynRecipe.SolutionFilePath), @"C:\Projects\openrewrite\rewrite-csharp\Rewrite\tests\fixtures\server\bitwarden-server.sln");
        var recipe = (RoslynRecipe)recipeManager.CreateRecipe(installableRecipe, recipeStartInfo);
        recipe.Should().NotBeNull();
        var affectedDocuments = await recipe.Execute(CancellationToken.None);
        affectedDocuments.Should().NotBeEmpty();
    }
}