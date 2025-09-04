using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using NuGet.LibraryModel;
using Rewrite.Core;
using Rewrite.MSBuild;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

[PublicAPI]
public class RunRecipeCommand(RecipeManager recipeManager, ILogger<RunRecipeCommand> logger) : AsyncCommand<RunRecipeCommand.Settings>
{
    [PublicAPI]
    public class Settings : BaseSettings
    {
        private Lazy<Regex> PackageNameRegex = new Lazy<Regex>(() =>
        {
            // Details:
            // id: NuGet package ID rules — starts/ends with alnum; allows ., _, -; max 100 chars.
            //     version: SemVer (NuGet) major.minor.patch[.revision][-prerelease][+build], plus the case-insensitive keywords SNAPSHOT or RELEASE.
            //     Examples that match:
            // Newtonsoft.Json:13.0.3
            // My.Package_Id:1.2.3.4
            // Foo.Bar:1.0.0-alpha.1+exp.sha.5114f85
            // Foo:SNAPSHOT
            // Foo:release

            return new Regex(
                "^(?<id>[A-Za-z0-9](?:[A-Za-z0-9._-]{0,98}[A-Za-z0-9])?)(?::(?<version>(?:(?:0|[1-9]\\d*)\\.(?:0|[1-9]\\d*)\\.(?:0|[1-9]\\d*)(?:\\.(?:0|[1-9]\\d*))?(?:-(?:0|[1-9]\\d*|[A-Za-z-][0-9A-Za-z-]*)(?:\\.(?:0|[1-9]\\d*|[A-Za-z-][0-9A-Za-z-]*))*)?(?:\\+[0-9A-Za-z-]+(?:\\.[0-9A-Za-z-]+)*)?)|(?i:SNAPSHOT|RELEASE)))?$",
                RegexOptions.Compiled);
        });
        
        [CommandOption("-s|--solution <SOLUTION>")]
        [Description("Path to solution file or source directory. If directory is used, it will recursively find all solutions under it")]
        public required string Path { get; set; }

        [CommandOption("-p|--package <NAME>")]
        [Description("Nuget Package Name. By default uses latest version. An explicit version can be specified using syntax <packageName>:<version>. " +
                     "If version is SNAPSHOT, uses latest prerelease version. " +
                     "If version is RELEASE, uses latest stable version." +
                     "Can be specified multiple times.")]
        public required string[] Packages { get; set; }

        [CommandOption("--feed <NUGET_FEED>")]
        [Description("Nuget feed URL. Can be specified multiple times If omitted, defaults to https://api.nuget.org/v3/index.json")]
        public string[] FeedUrls { get; set; } = ["https://api.nuget.org/v3/index.json"];
        
        [CommandOption("--dry-run")]
        [Description("Does not commit changes to disk")]
        public bool DryRun { get; set; } = false;

        [CommandOption("-i|--id <VERSION>")]
        [Description("Recipe IDs. For Open Rewrite recipes this is namespace qualified type name. For Roslyn recipies this is the diagnostic ID (ex. CS1123)")]
        public string[] Ids { get; set; } = [];
        
        public override ValidationResult Validate()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (Path == null)
            {
                return ValidationResult.Error("Path must be specified");
            }
            // if (Ids == null || Ids.Length == 0)
            // {
            //     return ValidationResult.Error("Ids must be specified");
            // }

            if (Packages.Length == 0)
            {
                return ValidationResult.Error("Package Version must be specified");
            }
            var invalidPackageNames = Packages
                .Select(x => PackageNameRegex.Value.Match(x)).Where(x => x.Success == false)
                .ToArray();
            if (invalidPackageNames.Length > 0)
            {
                return ValidationResult.Error($"Invalid package names");
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
        logger.LogInformation("Executing {CommandName} with settings {@Settings}", nameof(RunRecipeCommand), settings);
        var cancellationToken = CancellationToken.None;
        var packageSources = settings.FeedUrls.Select(x => new PackageSource(x)).ToList();
        // var recipeManager = new RecipeManager();
        // CA1802: Use Literals Where Appropriate
        // CA1861: Avoid constant arrays as arguments
        var packages = settings.Packages.Select(InstallableRecipeExtensions.GetLibraryRange).ToList();
        
        // var installableRecipes = settings.Ids
        //     .Select(id =>  new InstallableRecipe(id, settings.PackageName, settings.PackageVersion).GetLibraryRange())
        //     .ToList();
        var recipeExecutionContext = await recipeManager.CreateExecutionContext(packages, cancellationToken, packageSources: packageSources);
        // var recipesPackage = await recipeManager.InstallRecipePackage(installableRecipe, packageSources);

        List<string> solutionPaths = new();
        if (File.Exists(settings.Path))
        {
            solutionPaths.Add(settings.Path);
        }
        else
        {
            solutionPaths = Directory.EnumerateFiles(settings.Path, "*.sln", SearchOption.AllDirectories).ToList();
        }

        foreach (var solutionPath in solutionPaths)
        {
            var recipeStartInfos = recipeExecutionContext.Recipes
                .Where(x => settings.Ids.Length == 0 || settings.Ids.Contains(x.Id))
                .Select(x => recipeExecutionContext
                    .CreateRecipeStartInfo(x)
                    .WithOption(nameof(RoslynRecipe.SolutionFilePath), solutionPath))
                .ToList();
            var recipe = (RoslynRecipe)recipeExecutionContext.CreateRecipe(recipeStartInfos);

            var recipeResult = await recipe.Execute(CancellationToken.None);
            foreach (var issue in recipeResult.FixedIssues)
            {
                logger.LogInformation("Issue {IssueId} fixes: @{FileName}", issue.IssueId, issue.Fixes.Select(x => x.FileName));
            }
        }

        return 0;
    }
}