using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;
using NuGet.LibraryModel;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities;
using Rewrite.Core.Config;
using Rewrite.MSBuild;
using Serilog;

// This is a weird hack that allows us to use main codebase as part of compilation execution task to generate "java recipe stubs"
// Since build project is USUALLY used for compiling purposes, we don't want to have it dependent on main project as it will cause file locks and build fialures

partial class Build
{
     (string Package, bool IncludePreRelease)[] NugetRecipePackages =
     [
         ("OpenRewrite.RoslynRecipes", true),
         ("Microsoft.CodeAnalysis.CSharp.CodeStyle", false),
         ("Roslynator.Analyzers", false), //https://github.com/dotnet/roslynator
         ("Microsoft.CodeAnalysis.NetAnalyzers", false), //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories
         ("Meziantou.Analyzer", false), //https://github.com/meziantou/Meziantou.Analyzer
         ("StyleCop.Analyzers", false), //https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master
         ("WpfAnalyzers", false), // https://github.com/DotNetAnalyzers/WpfAnalyzers
     ];

     Target GenerateRoslynRecipes => _ => _
         .Description("Generates Java recipe classes per .NET roslyn recipe found in common packages")
         .DependsOn(CleanNugetCache, Pack)
         .Executes(async () =>
         {
             CancellationToken ct = CancellationToken.None;
             var services = new ServiceCollection();
             services.AddLogging(c => c
                 .AddSerilog());
             services.AddSingleton<RecipeManager>();
             services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
             services.AddSingleton(provider => Settings.LoadDefaultSettings(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
             var serviceProvider = services.BuildServiceProvider();
             T CreateObject<T>(params object[] args) => ActivatorUtilities.CreateInstance<T>(serviceProvider, args);

             var recipeManager = CreateObject<RecipeManager>();
             var recipesDir = RootDirectory / "rewrite-csharp" / "src" / "main" / "java" / "org" / "openrewrite" / "csharp" / "recipes";
             recipesDir.CreateOrCleanDirectory();

             Dictionary<string, (int Analyzers, int Fixups)> summary = new();

             var nugetCacheFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
             foreach (var (package, includePreRelease) in NugetRecipePackages)
             {
                 var libraryRange = new[] {new LibraryRange(package)};
                 var resolvedPackage = (await recipeManager.ResolvePackages(libraryRange, ct, includePrerelease:includePreRelease)).First();

                // if this package is coming from a local folder and not a remote feed, clear it out from nuget cache as want to force a restore from the .nupkg file
                 // this addresses the issue with local builds where we may create a package multiple times during development without bumping up the version
                 // we want to ensure it's always using the latest build, not whatever it cached into the global nuget cache
                 if (resolvedPackage.Source.PackageSource.IsLocal)
                 {
                     var packageCacheFolder = nugetCacheFolder / resolvedPackage.Id.ToLowerInvariant() / resolvedPackage.Version.ToNormalizedString();
                     packageCacheFolder.DeleteDirectory();
                 }
                 var executionContext = await recipeManager.CreateExecutionContext(libraryRange, includePrerelease: includePreRelease, cancellationToken: ct);
                 var analyzerCount = executionContext.Recipes.Count(x => x.Kind == RecipeKind.RoslynAnalyzer);
                 var fixupCount = executionContext.Recipes.Count(x => x.Kind == RecipeKind.RoslynFixer);
                 Log.Information("Found {AnalyzerCount} analyzers and {FixupCount} fixups", analyzerCount, fixupCount);
                 summary.Add(package, (analyzerCount, fixupCount));
                 var packageNameFirstSegment = resolvedPackage.Id.ToLower().Split('.').First();
                 var compositeRecipe = new StringBuilder($$"""
                     ---
                     type: specs.openrewrite.org/v1beta/recipe
                     name: org.openrewrite.csharp.recipes.{{packageNameFirstSegment}}.{{package.Replace(".","")}}
                     displayName: All '{{resolvedPackage.Id}}' Recipes
                     description: All '{{resolvedPackage.Id}}' Recipes.
                     recipeList:

                     """);

                 static string GetClassName(RecipeDescriptor recipe) => recipe.Id.StartsWith(recipe.TypeName) ? recipe.Id : recipe.TypeName.FullName.Split('.').Last();
                 static string EscapeQuotesAndRemoveLineBreaks(string input) => input.Replace("\"", "\\\"").Replace('\r', ' ').Replace('\n', ' ');
                 var models = executionContext.Recipes
                     .OrderBy(GetClassName)
                     .Select(recipe =>
                 {
                     var className = GetClassName(recipe);
                     className = className.ReplaceRegex("CodeFixProvider$", _ => "Fixer");
                     className = $"{className}{recipe.Id}";

                     var @namespace = packageNameFirstSegment;
                     var tags = recipe.Tags.Append(packageNameFirstSegment)
                         .Append("csharp")
                         .Append("dotnet")
                         .Append("c#")
                         .ToList();
                     var displayNamePostfix = recipe.Kind == RecipeKind.RoslynAnalyzer ? " (search)" : "";
                     var descriptionPostfix  = recipe.Kind == RecipeKind.RoslynAnalyzer ? "This is a reporting only recipe. " : "";
                     return new
                     {
                         recipe.Id,
                         Description = $"{descriptionPostfix}{EscapeQuotesAndRemoveLineBreaks(recipe.Description)}",
                         DisplayName = $"{EscapeQuotesAndRemoveLineBreaks(recipe.DisplayName)}{displayNamePostfix}",
                         PackageName = resolvedPackage.Id,
                         PackageVersion = resolvedPackage.Version,
                         Tags = tags,
                         ClassName = className,
                         Namespace = @namespace,
                         RunCodeFixup = recipe.Kind == RecipeKind.RoslynFixer ? "true" : "false",
                         FileName = recipesDir / @namespace.Replace(".", "/") / $"{className}.java"
                     };
                 }).ToList();

                 string RenderTags(IEnumerable<string> tags) => $"\"{string.Join("\", \"", tags)}\"";
                 foreach (var model in models)
                 {
                     compositeRecipe.AppendLine($"  - org.openrewrite.csharp.recipes.{model.Namespace}.{model.ClassName}");
                 }

                 var compositeRecipeText = SourceFileHeader.Apply(compositeRecipe.ToString(), SourceFileHeader.DocumentType.Yaml, true);
                 var compositeRecipePath = RootDirectory / "rewrite-csharp" / "src" / "main" / "resources" / "META-INF" / "rewrite" / $"{package}.yml";
                 compositeRecipePath.WriteAllText(compositeRecipeText);
                 var result = models.Select(model => (model.FileName, Content: SourceFileHeader.Apply($$""""
                         package org.openrewrite.csharp.recipes.{{model.Namespace}};

                         import lombok.Getter;
                         import org.openrewrite.csharp.RoslynRecipe;

                         import java.util.Set;
                         import java.util.stream.Stream;

                         import static java.util.stream.Collectors.toSet;

                         @Getter
                         public class {{model.ClassName}} extends RoslynRecipe {

                             final String recipeId = "{{model.Id}}";
                             final boolean runCodeFixup = {{model.RunCodeFixup}};

                             final String nugetPackageName = "{{model.PackageName}}";
                             final String nugetPackageVersion = "{{model.PackageVersion}}";

                             final String displayName = "{{model.DisplayName}}";
                             final String description = "{{model.Description}}";
                             final Set<String> tags = Stream.of({{RenderTags(model.Tags)}}).collect(toSet());

                         }

                         """", SourceFileHeader.DocumentType.Java, true)))
                     .ToList();
                 foreach (var (filename, source) in result)
                 {
                     filename.WriteAllText(source);
                 }
             }

             foreach (var (package, count) in summary)
             {
                 Log.Information($"{package}:  {count.Analyzers} analyzers / {count.Fixups} fixups");
             }
             Log.Information($"Total:  {summary.Select(x => x.Value.Analyzers).Sum()} analyzers / {summary.Select(x => x.Value.Fixups).Sum()}");

         });
}
