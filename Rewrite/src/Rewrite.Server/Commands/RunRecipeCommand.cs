using System.ComponentModel;
using NuGet.Configuration;
using Rewrite.Core;
using Rewrite.MSBuild;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

[PublicAPI]
public class RunRecipeCommand(RecipeManager recipeManager) : AsyncCommand<RunRecipeCommand.Settings>
{
    [PublicAPI]
    public class Settings : BaseSettings
    {
        [CommandOption("-s|--solution <SOLUTION>")]
        [Description("Path to solution file")]
        public required string SolutionPath { get; set; }

        [CommandOption("-p|--package <NAME>")]
        [Description("Nuget Package Name")]
        public required string PackageName { get; set; }

        [CommandOption("-v|--version <VERSION>")]
        [Description("Nuget Package Version")]
        public required string PackageVersion { get; set; }

        [CommandOption("--feed <NUGET_FEED>")]
        [Description("Nuget feed URL. Can be specified multiple times If omitted, defaults to https://api.nuget.org/v3/index.json")]
        public string[] FeedUrls { get; set; } = ["https://api.nuget.org/v3/index.json"];
        
        [CommandOption("--dry-run")]
        [Description("Does not commit changes to disk")]
        public bool DryRun { get; set; } = false;
        
        [CommandOption("-i|--id <VERSION>")]
        [Description("Recipe IDs, comma separated. For Open Rewrite recipes this is namespace qualified type name. For Roslyn recipies this is the diagnostic ID (ex. CS1123)")]
        public required string[] Ids { get; set; }
        
        public override ValidationResult Validate()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (SolutionPath == null)
            {
                return ValidationResult.Error("Solution path must be specified");
            }
            if (Ids == null)
            {
                return ValidationResult.Error("Id must be specified");
            }
            if (PackageVersion == null)
            {
                return ValidationResult.Error("Recipe Id must be specified");
            }
            if (PackageName == null)
            {
                return ValidationResult.Error("Package Version must be specified");
            }

            // if (FeedUrls.Count == 0)
            // {
            //     FeedUrls.Add("https://api.nuget.org/v3/index.json");
            // }
            // ReSharper enable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

            return base.Validate();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        
        
        var packageSources = settings.FeedUrls.Select(x => new PackageSource(x)).ToList();
        // var recipeManager = new RecipeManager();
        // CA1802: Use Literals Where Appropriate
        // CA1861: Avoid constant arrays as arguments
        var installableRecipe = new InstallableRecipe(settings.Ids, settings.PackageName, settings.PackageVersion);
        var recipesPackage = await recipeManager.InstallRecipePackage(installableRecipe, packageSources);
        var recipeDescriptor = recipesPackage.GetRecipeDescriptor(installableRecipe.Id);
        var recipeStartInfo = recipesPackage.CreateRecipeStartInfo(recipeDescriptor);
        recipeStartInfo.WithOption(nameof(RoslynRecipe.SolutionFilePath), settings.SolutionPath);
        var recipe = (RoslynRecipe)recipeManager.CreateRecipe(recipeStartInfo);
        
        var recipeResult = await recipe.Execute(CancellationToken.None);
        foreach (var issue in recipeResult.FixedIssues)
        {
            Console.WriteLine($"Issue {issue.IssueId} fixes:");
            foreach (var doc in issue.Fixes)
            {
                Console.WriteLine(doc.FileName);
            }
        }

        return 0;
    }
}