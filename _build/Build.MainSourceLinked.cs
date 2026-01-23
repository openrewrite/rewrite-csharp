using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                 var packageNameFirstSegment = resolvedPackage.Id.ToLower().Split('.').First();
                 var compositeRecipe = new StringBuilder($$"""
                     #
                     # Copyright 2026 the original author or authors.
                     # <p>
                     # Licensed under the Moderne Source Available License (the "License");
                     # you may not use this file except in compliance with the License.
                     # You may obtain a copy of the License at
                     # <p>
                     # https://docs.moderne.io/licensing/moderne-source-available-license
                     # <p>
                     # Unless required by applicable law or agreed to in writing, software
                     # distributed under the License is distributed on an "AS IS" BASIS,
                     # WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
                     # See the License for the specific language governing permissions and
                     # limitations under the License.
                     #

                     # -------------------THIS FILE IS AUTO GENERATED--------------------------
                     # Changes to this file may cause incorrect behavior and will be lost if
                     # the code is regenerated.
                     #
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
                     // className = className.ReplaceRegex("(Analyzer|Fixer|CodeFixProvider)$", _ => "");
                     className = className.ReplaceRegex("CodeFixProvider$", _ => "Fixer");
                     className = $"{className}{recipe.Id}";

                     // var packageNameFirstSegment = recipe.TypeName.FullName.Split('.').First();
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
                 var license = File.ReadAllText(Solution._solution._build.Directory / "License.txt").Trim();

                 string RenderTags(IEnumerable<string> tags) => $"\"{string.Join("\", \"", tags)}\"";
                 foreach (var model in models)
                 {
                     compositeRecipe.AppendLine($"  - org.openrewrite.csharp.recipes.{model.Namespace}.{model.ClassName}");
                 }

                 var compositeRecipePath = RootDirectory / "rewrite-csharp" / "src" / "main" / "resources" / "META-INF" / "rewrite" / $"{package}.yml";
                 compositeRecipePath.WriteAllText(compositeRecipe.ToString());
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
