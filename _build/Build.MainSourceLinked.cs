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
using Rewrite.MSBuild;
using Rewrite.RewriteXml.Tree;
using Serilog;

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
                 "Microsoft.CodeAnalysis.NetAnalyzers", //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories
                 "Roslynator.Analyzers", //https://github.com/dotnet/roslynator
                 "Meziantou.Analyzer", //https://github.com/meziantou/Meziantou.Analyzer
                 "StyleCop.Analyzers", //https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master
                 "WpfAnalyzers",
             };
             var recipesDir = RootDirectory / "rewrite-csharp" / "src" / "main" / "java" / "org" / "openrewrite" / "csharp" / "recipes";
             recipesDir.CreateOrCleanDirectory();

             foreach (var package in packages)
             {
                 var libraryRange = new[] {new LibraryRange(package)};
                 var resolvedPackage = (await recipeManager.ResolvePackages(libraryRange, ct, includePrerelease:false, packageSources: packageSources)).First();
                 // var libraryRange = packages.Select(x => new LibraryRange(x)).ToList();
                 var executionContext = await recipeManager.CreateExecutionContext(libraryRange, includePrerelease: false, packageSources: packageSources, cancellationToken: CancellationToken.None);

                 // var installablePackages = await Task.WhenAll(packages.Select(x => recipeManager.InstallRecipePackage(x.Package, packageSources: packageSources)));

                 var models = executionContext.Recipes.Select(recipe =>
                 {
                     var className = recipe.Id.StartsWith(recipe.TypeName) ? recipe.Id : recipe.TypeName.FullName.Split('.').Last();
                     className = className.ReplaceRegex("(Analyzer|Fixer|CodeFixProvider)$", _ => "");
                     className = $"{className}{recipe.Id}";
                     var packageNameFirstSegment = resolvedPackage.Id.ToLower().Split('.').First();
                     // var packageNameFirstSegment = recipe.TypeName.FullName.Split('.').First();
                     var @namespace = packageNameFirstSegment;
                     var tags = recipe.Tags.Append(packageNameFirstSegment)
                         .Append("csharp")
                         .Append("dotnet")
                         .Append("c#")
                         .ToList();
                     return new
                     {
                         recipe.Id,
                         Description = recipe.Description.Replace("\"", "\\\"").Replace('\r', ' ').Replace('\n', ' '),
                         DisplayName = recipe.DisplayName.Replace("\"", "\\\"").Replace('\r', ' ').Replace('\n', ' '),
                         PackageName = resolvedPackage.Id,
                         PackageVersion = resolvedPackage.Version,
                         Tags = tags,
                         ClassName = className,
                         Namespace = @namespace,
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

                         import org.openrewrite.csharp.RoslynRecipe;

                         import java.util.Set;
                         import java.util.stream.Collectors;
                         import java.util.stream.Stream;

                         public class {{model.ClassName}} extends RoslynRecipe {

                             @Override
                             public String getRecipeId() {
                                 return "{{model.Id}}";
                             }

                             @Override
                             public String getNugetPackageName() {
                                 return "{{model.PackageName}}";
                             }

                             @Override
                             public String getNugetPackageVersion() {
                                 return "{{model.PackageVersion}}";
                             }

                             @Override
                             public String getDisplayName() {
                                 return "{{model.DisplayName}}";
                             }

                             @Override
                             public String getDescription() {
                                 return "{{model.Description}}";
                             }

                             @Override
                             public Set<String> getTags() {
                                 return Stream.of({{RenderTags(model.Tags)}}).collect(Collectors.toSet());
                             }
                             }

                         """"))
                     .ToList();
                 foreach (var (filename, source) in result)
                 {
                     filename.WriteAllText(source);
                 }
             }
         });
}
