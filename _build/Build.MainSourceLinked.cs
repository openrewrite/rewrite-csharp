using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;
using NuGet.LibraryModel;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities;
using Rewrite.Core;
using Rewrite.Core.Config;
using Rewrite.MSBuild;
using Rewrite.RewriteXml.Tree;
using Serilog;

// This is a weird hack that allows us to use main codebase as part of compilation execution task to generate "java recipe stubs"
// Since build project is USUALLY used for compiling purposes, we don't want to have it dependent on main project as it will cause file locks and build fialures

partial class Build
{

     Target GenerateRoslynRecipes => _ => _
         .Description("Generates Java recipe classes per .NET roslyn recipe found in common packages")
         .DependsOn(CleanNugetCache, Pack)
         // .DependsOn(Restore)
         .Executes(async () =>
         {
             CancellationToken ct = CancellationToken.None;
             var services = new ServiceCollection();
             services.AddLogging(c => c
                 .AddSerilog());
             services.AddSingleton<RecipeManager>();
             services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
             var serviceProvider = services.BuildServiceProvider();
             T CreateObject<T>(params object[] args) => ActivatorUtilities.CreateInstance<T>(serviceProvider, args);

             var recipeManager = CreateObject<RecipeManager>();

             string[] feeds =
             [
                 ArtifactsDirectory / "test",
                 NugetFeed
             ];

             var packageSources = feeds.Select(x => new PackageSource(x)).ToList();
             // var recipeManager = new RecipeManager();
             // CA1802: Use Literals Where Appropriate
             // CA1861: Avoid constant arrays as arguments
             var packages =  new[]
             {
                 "Microsoft.CodeAnalysis.CSharp.CodeStyle",
                 "Roslynator.Analyzers", //https://github.com/dotnet/roslynator
                 "Microsoft.CodeAnalysis.NetAnalyzers", //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories
                 "Meziantou.Analyzer", //https://github.com/meziantou/Meziantou.Analyzer
                 "StyleCop.Analyzers", //https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master
                 "WpfAnalyzers",
             };
             var recipesDir = RootDirectory / "rewrite-csharp" / "src" / "main" / "java" / "org" / "openrewrite" / "csharp" / "recipes";
             recipesDir.CreateOrCleanDirectory();

             Dictionary<string, (int Analyzers, int Fixups)> summary = new();
             foreach (var package in packages)
             {
                 var libraryRange = new[] {new LibraryRange(package)};
                 var resolvedPackage = (await recipeManager.ResolvePackages(libraryRange, ct, includePrerelease:false, packageSources: packageSources)).First();
                 // var libraryRange = packages.Select(x => new LibraryRange(x)).ToList();
                 var executionContext = await recipeManager.CreateExecutionContext(libraryRange, includePrerelease: false, packageSources: packageSources, cancellationToken: CancellationToken.None);

                 // var installablePackages = await Task.WhenAll(packages.Select(x => recipeManager.InstallRecipePackage(x.Package, packageSources: packageSources)));

                 var analyzerCount = executionContext.Recipes.Count(x => x.Kind == RecipeKind.RoslynAnalyzer);
                 var fixupCount = executionContext.Recipes.Count(x => x.Kind == RecipeKind.RoslynFixer);
                 Log.Information("Found {AnalyzerCount} analyzers and {FixupCount} fixups", analyzerCount, fixupCount);
                 summary.Add(package, (analyzerCount, fixupCount));

                 static string EscapeQuotesAndRemoveLineBreaks(string input) => input.Replace("\"", "\\\"").Replace('\r', ' ').Replace('\n', ' ');
                 var models = executionContext.Recipes.Select(recipe =>
                 {
                     var className = recipe.Id.StartsWith(recipe.TypeName) ? recipe.Id : recipe.TypeName.FullName.Split('.').Last();
                     // className = className.ReplaceRegex("(Analyzer|Fixer|CodeFixProvider)$", _ => "");
                     className = className.ReplaceRegex("CodeFixProvider$", _ => "Fixer");
                     className = $"{className}{recipe.Id}";
                     var packageNameFirstSegment = resolvedPackage.Id.ToLower().Split('.').First();
                     // var packageNameFirstSegment = recipe.TypeName.FullName.Split('.').First();
                     var @namespace = packageNameFirstSegment;
                     var tags = recipe.Tags.Append(packageNameFirstSegment)
                         .Append("csharp")
                         .Append("dotnet")
                         .Append("c#")
                         .ToList();
                     var displayNamePrefix = recipe.Kind == RecipeKind.RoslynAnalyzer ? "Analysis: " : "";
                     var descriptionPostfix  = recipe.Kind == RecipeKind.RoslynAnalyzer ? "This is a reporting only recipe. " : "";
                     return new
                     {
                         recipe.Id,
                         Description = $"{descriptionPostfix}{EscapeQuotesAndRemoveLineBreaks(recipe.Description)}",
                         DisplayName = $"{displayNamePrefix}{EscapeQuotesAndRemoveLineBreaks(recipe.DisplayName)}",
                         PackageName = resolvedPackage.Id,
                         PackageVersion = resolvedPackage.Version,
                         Tags = tags,
                         ClassName = className,
                         Namespace = @namespace,
                         RunCodeFixup = recipe.Kind == RecipeKind.RoslynFixer ? "true" : "false",
                         FileName = recipesDir / @namespace.Replace(".", "/") / $"{className}.java"
                     };
                 }).ToList();
                 var license = File.ReadAllText(Solution._solution._build.Directory / "License.txt").Trim();

                 string RenderTags(IEnumerable<string> tags) => $"\"{string.Join("\", \"", tags)}\"";

                 var result = models.Select(model => (model.FileName, Content: $$""""
                         {{license}}
                         /*
                          * -------------------THIS FILE IS AUTO GENERATED--------------------------
                          * Changes to this file may cause incorrect behavior and will be lost if
                          * the code is regenerated.
                         */

                         package org.openrewrite.csharp.recipes.{{model.Namespace}};

                         import lombok.Getter;
                         import org.openrewrite.csharp.RoslynRecipe;

                         import java.util.Set;
                         import java.util.stream.Collectors;
                         import java.util.stream.Stream;

                         public class {{model.ClassName}} extends RoslynRecipe {
                             @Getter
                             final String recipeId = "{{model.Id}}";

                             @Getter
                             final boolean runCodeFixup = {{model.RunCodeFixup}};

                             @Getter
                             final String nugetPackageName = "{{model.PackageName}}";

                             @Getter
                             final String nugetPackageVersion = "{{model.PackageVersion}}";

                             @Getter
                             final String displayName = "{{model.DisplayName}}";

                             @Getter
                             final String description = "{{model.Description}}";

                             @Getter
                             final Set<String> tags = Stream.of({{RenderTags(model.Tags)}}).collect(Collectors.toSet());

                         }

                         """"))
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
