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
using Nuke.Common.Utilities.Collections;
using Rewrite.Core;
using Rewrite.Server.Commands;
using Rewrite.Test;
using Rewrite.Test.CSharp;
using Rewrite.Tests;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Spectre.Console.Cli;
using TUnit.Core;
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
    public async Task InstallRecipe(CancellationToken cancellationToken)
    {
        var recipeManager = CreateObject<RecipeManager>();
        
        string[] packageSources =
        [
            DirectoryHelper.RepositoryRoot / "artifacts",
            DirectoryHelper.RepositoryRoot / "artifacts" / "test",
            "https://api.nuget.org/v3/index.json"
        ];
        
        var recipeIdentity = new InstallableRecipe("Rewrite.Recipes.FindClass", "Rewrite.Recipes", "0.0.1");
        var recipeExecutionContext = await recipeManager.CreateExecutionContext(
            [recipeIdentity.GetLibraryRange()], 
            cancellationToken, 
            packageSources: packageSources.Select(x => new PackageSource(x)).ToList());

        var lst = Assertions.CSharp(
            """
            public    class Foo;
            """
        ).Parse();
    
        var recipeDescriptor = recipeExecutionContext.GetRecipeDescriptor(recipeIdentity.Id) ?? throw new Exception("Recipe not loaded");
        var recipeStartInfo = recipeExecutionContext.CreateRecipeStartInfo(recipeDescriptor)
            .WithOption("Description", "!!--->");
        var recipe = recipeExecutionContext.CreateRecipe(recipeStartInfo);
        recipe.Should().NotBeNull();
        recipe.GetType().GetProperty("Description")!.GetValue(recipe).Should().Be("!!--->");
        
        var executionContext = new InMemoryExecutionContext();
        var visitor = recipe.GetVisitor();
        visitor.IsAcceptable(lst, executionContext).Should().BeTrue();
    
        var afterLst = visitor.Visit(lst, executionContext);
        afterLst!.ToString().ShouldBeSameAs($"/*~~(!!--->)~~>*/{lst}");
    }

    
    /// <summary>
    /// Verifies that we can act on multiple solutions via one invocation of the command
    /// </summary>
    [Test]
    public async Task<VerifyResult> RoslynRecipeBatchWithTwoSolution()
    {
        var directory = CreateRecipeInputDirectory(FixturesDir / "TwoSolutions");
        
        var settings = new RunRecipeCommand.Settings
        {
            Ids = [
                "CA1861", // Avoid constant arrays as arguments,
                "CA1311"  // Specify a culture or use an invariant version
            ], 
            Packages = ["Microsoft.CodeAnalysis.NetAnalyzers:9.0.0"],
            Path = directory
        };
        var command = CreateObject<RunRecipeCommand>();
        await command.ExecuteAsync(settings);
        
        return await VerifyDirectory(directory, IncludeTestFile);
    }
    
    
    /// <summary>
    /// Verifies that we can act on multiple solutions via one invocation of the command
    /// </summary>
    [Test]
    public async Task MorganStanley()
    {
        // var directory = CreateRecipeInputDirectory(FixturesDir / "TwoSolutions");
        var directory = (AbsolutePath)"C:\\Projects\\morganstanley\\ComposeUI";
        var settings = new RunRecipeCommand.Settings
        {
            // Ids = [
            //     "CA1861", // Avoid constant arrays as arguments,
            //     "CA1311"  // Specify a culture or use an invariant version
            // ], 
            Packages = ["Microsoft.CodeAnalysis.NetAnalyzers"],
            Path = directory
        };
        var command = CreateObject<RunRecipeCommand>();
        await command.ExecuteAsync(settings);
        
        // return await VerifyDirectory(directory, IncludeTestFile);
    }

}