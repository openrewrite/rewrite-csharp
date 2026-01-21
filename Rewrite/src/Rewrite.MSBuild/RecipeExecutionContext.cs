using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Nuke.Common.IO;
using Rewrite.Core;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

public class RecipeExecutionContext : AssemblyLoadContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<RecipeExecutionContext> _log;
    public Dictionary<AssemblyName, AbsolutePath> _assemblies { get; set; }
    private readonly HashSet<Assembly> _recipeAssemblies = [];
    public IReadOnlyList<RecipeDescriptor> Recipes { get; private set; }

    private static IEqualityComparer<AssemblyName> _assemblyNameEqualityComparer = EqualityComparer<AssemblyName>.Create((a, b) => a?.Name == b?.Name, a => a.Name!.GetHashCode());

    public RecipeExecutionContext(List<AbsolutePath> assemblies, ILoggerFactory loggerFactory)
    {
        
        _loggerFactory = loggerFactory;
        _log = loggerFactory.CreateLogger<RecipeExecutionContext>();
        _assemblies = assemblies
            .Where(x => !x.Name.EndsWith(".resources.dll"))
            .ToDictionary(x => AssemblyName.GetAssemblyName(x), x => x, _assemblyNameEqualityComparer);
        Recipes = FindRecipesInAssemblies();
        _log.LogDebug("Created execution context with {RecipeCount} available recipes", Recipes.Count);

    }


    public Recipe CreateRecipe(params IReadOnlyCollection<RecipeStartInfo> recipeStartInfos)
    {

        if (recipeStartInfos.Count == 0)
        {
            throw new InvalidOperationException($"Parameter {nameof(recipeStartInfos)} should not be empty");
        }
        if (recipeStartInfos.Any(x => x.Kind == RecipeKind.OpenRewrite) && recipeStartInfos.Any(x => x.Kind != RecipeKind.OpenRewrite))
        {
            throw new InvalidOperationException("Different recipe types cannot be mixed in same execution context");
        }

        var recipeStartInfo = recipeStartInfos.First();
        var recipeType = recipeStartInfo.Kind;
        
        
        
        if (recipeType == RecipeKind.OpenRewrite)
        {
            if (recipeStartInfos.Count > 1)
            {
                throw new InvalidOperationException("Bulk execution of OpenRewrite native .net recipes is not currently supported");
            }
            return CreateOpenRewriteRecipe(recipeStartInfos.First());
        }
        
        if(recipeType is RecipeKind.RoslynAnalyzer or RecipeKind.RoslynFixer) // roslyn recipe
        {
            var solutionPaths = recipeStartInfos
                .Select(x => x.Arguments.GetValueOrDefault(nameof(RoslynRecipe.SolutionFilePath))?.ToString())
                .ToList();
            var distinctSolutionPaths = solutionPaths.Distinct().ToList();
            if (distinctSolutionPaths.Count > 1)
            {
                throw new InvalidOperationException($"All items in {nameof(recipeStartInfos)} must target the same solution file");
            }

            
            return CreateRoslynRecipe(recipeStartInfos);
        }
        
        throw new NotSupportedException($"Recipe type {recipeType} is not supported");
        
    }
    private Recipe CreateRecipe(RecipeStartInfo recipeStartInfo)
    {
        
        // if (!_recipeAssemblyMap.TryGetValue(descriptor, out var assemblyPath))
        //     throw new InvalidOperationException($"The provided recipe descriptor for recipe {descriptor} is not part of execution context {Name}");
        // var assembly = LoadFromAssemblyPath(assemblyPath);
        var recipe = recipeStartInfo.Kind switch
        {
            RecipeKind.OpenRewrite => CreateOpenRewriteRecipe(recipeStartInfo),
            RecipeKind.RoslynFixer => CreateRoslynRecipe(recipeStartInfo),
            RecipeKind.RoslynAnalyzer => throw new NotSupportedException("This type of analyzer is not yet supported"),
            _ => throw new ArgumentOutOfRangeException()
        };
        return recipe;
    }

    
    private Recipe CreateOpenRewriteRecipe(RecipeStartInfo recipeStartInfo)
    {
        var recipeType = GetRecipeType(recipeStartInfo);
        var recipe = (Recipe)Activator.CreateInstance(recipeType)!;
         
        // todo: add validation to ensure everything is satisfied
        var recipeProperties = recipeType
            .GetProperties()
            .Join(recipeStartInfo.Arguments.Values, x => x.Name, x => x.Name, (property, argument) => (property, argument))
            .ToList();
        foreach(var (propertyInfo, argument) in recipeProperties)
        {
            propertyInfo.SetValue(recipe, argument.Value);
        }
        return recipe;
    }

    private Recipe CreateRoslynRecipe(params IReadOnlyCollection<RecipeStartInfo> recipeStartInfos)
    {
        var recipeStartInfo =  recipeStartInfos.First();
        var solutionFilePath = (string)recipeStartInfo.Arguments[nameof(RoslynRecipe.SolutionFilePath)].Value!;
        var dryRun = (bool)recipeStartInfo.Arguments[nameof(RoslynRecipe.DryRun)].Value!;
        var fixableDiagnosticIds = recipeStartInfos
            .Where(x => x.Kind == RecipeKind.RoslynFixer)
            .Select(x => x.Id)
            .ToHashSet();
        
        var reportOnlyDiagnosticIds = recipeStartInfos
            .Where(x => x.Kind == RecipeKind.RoslynAnalyzer)
            .Select(x => x.Id)
            .ToHashSet();
        
        var roslynRecipe = new RoslynRecipe(_recipeAssemblies, _loggerFactory.CreateLogger<RoslynRecipe>())
        {
            DiagnosticIdsToFix = fixableDiagnosticIds,
            DiagnosticIdsToReport = reportOnlyDiagnosticIds,
            SolutionFilePath = solutionFilePath,
            DryRun = dryRun
        };
        return roslynRecipe;
    }

    private Type GetRecipeType(RecipeStartInfo recipeStartInfo)
    {
        var assembly = LoadFromAssemblyName(new AssemblyName(recipeStartInfo.TypeName.AssemblyName));
        var recipeType = assembly.GetType(recipeStartInfo.TypeName.FullName) ?? throw new InvalidOperationException($"Cannot resolve recipe type {recipeStartInfo.TypeName.FullName} from {assembly.FullName}");
        return recipeType;
    }


    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var isCoreRewriteAssembly = AssemblyResolution.CoreRewriteAssemblies.Any(x => x.GetName().Name == assemblyName.Name);
        if (isCoreRewriteAssembly)
        {
            return null; // this will cause fallback to load from default assembly context
        }

        if (_assemblies.TryGetValue(assemblyName, out var assembly))
        {
            return LoadFromAssemblyPath(assembly);
        }

        return null;
    }

    
    private List<RecipeDescriptor> FindRecipesInAssemblies()
    {
        var assembliesToLoad = _assemblies.ToDictionary();
        // assembliesToLoad.Add(typeof(RecipesAttribute).Assembly.GetName(), typeof(RecipesAttribute).Assembly.Location);
        
        // assembliesToLoad.Add(typeof(RecipesAttribute).Assembly.GetName(), typeof(RecipesAttribute).Assembly.Location);
        var resolver = new RecipeAssemblyResolver(_assemblies.Values);
        using var mlc = new MetadataLoadContext(resolver);
        var mlcRecipeAttributeAssembly = mlc.LoadFromAssemblyPath(typeof(RecipesAttribute).Assembly.Location);
        var mlcRecipeAttributeType = mlcRecipeAttributeAssembly.GetType(typeof(RecipesAttribute).FullName!)!;
        // var mlcDiagnosticAnalyzerAssembly = mlc.LoadFromAssemblyPath(typeof(DiagnosticAnalyzer).Assembly.Location);
        var mlcDiagnosticAnalyzerAssembly = mlc.LoadFromAssemblyName(typeof(DiagnosticAnalyzer).Assembly.FullName!);
        var mlcDiagnosticAnalyzerAttributeType = mlcDiagnosticAnalyzerAssembly.GetType(typeof(DiagnosticAnalyzerAttribute).FullName!)!;
        // var mlcCodeFixupAssembly = mlc.LoadFromAssemblyPath(typeof(ExportCodeFixProviderAttribute).Assembly.Location);
        var mlcCodeFixupAssembly = mlc.LoadFromAssemblyName(typeof(ExportCodeFixProviderAttribute).Assembly.FullName!);
        var mlcCodeFixupAttributeType = mlcCodeFixupAssembly.GetType(typeof(ExportCodeFixProviderAttribute).FullName!)!;
        // Load assembly into MetadataLoadContext.
        
        var metadataAssemblies = _assemblies.Values.Select(x => mlc.LoadFromAssemblyPath(x)).ToList();

        var openRewriteRecipes = metadataAssemblies
            .SelectMany(assembly => assembly
                .GetCustomAttributesData()
                .Where(x => x.AttributeType == mlcRecipeAttributeType)
                .Select(x => (assembly, CustomAttributeData: x)))
            .SelectMany(assemblyWithRecipes =>
            {
                var (assembly, recipeAttribute) = assemblyWithRecipes;
                IReadOnlyCollection<CustomAttributeTypedArgument> recipeAttributeArguments = recipeAttribute
                    .ConstructorArguments[0].Value as IReadOnlyCollection<CustomAttributeTypedArgument> ?? new List<CustomAttributeTypedArgument>();
                if (recipeAttributeArguments.Count > 0)
                {
                    this._recipeAssemblies.Add(LoadFromAssemblyPath(assembly.Location));
                }
                var recipeDescriptorsJson = recipeAttributeArguments
                    .Where(x => x.Value != null)
                    .Select(x => (string)x.Value!)
                    .ToList();
                var recipeDescriptors = recipeDescriptorsJson.Select(x => JsonConvert.DeserializeObject<RecipeDescriptor>(x)!).ToList();
                return recipeDescriptors;
            })
            .ToList();
        
        

        var analyzerRecipes = metadataAssemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(type => type.GetCustomAttributesData().Any(attr => attr.AttributeType == mlcDiagnosticAnalyzerAttributeType))
            .Select(x =>
            {
                
                var assembly = LoadFromAssemblyPath(x.Assembly.Location);
                _recipeAssemblies.Add(assembly);
                return assembly.GetType(x.FullName!)!;
            })
            .Select(Activator.CreateInstance)
            .Cast<DiagnosticAnalyzer>()
            .SelectMany(analyzer => analyzer.SupportedDiagnostics.Select(descriptor => (Descriptor: descriptor, Analyzer: analyzer)))
            .DistinctBy(x => x.Descriptor.Id) // while we could have multiple analyzers contributing same ID, for description purposes we just need single
            .Select(x => new RecipeDescriptor()
            {
                Id = x.Descriptor.Id,
                Kind = RecipeKind.RoslynAnalyzer,
                DisplayName = x.Descriptor.Title.ToString(),
                Description = x.Descriptor.Description.ToString(),
                TypeName = TypeName.Parse(x.Analyzer.GetType().AssemblyQualifiedName!),
                Tags = ["roslyn", "analyzer", x.Descriptor.Id],
                Options = OptionDescriptor.FromRecipeType<RoslynRecipe>() 
            })
            .ToList();
        
        
        var fixersById = metadataAssemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(type => type.GetCustomAttributesData().Any(attr => attr.AttributeType == mlcCodeFixupAttributeType))
            .Select(x =>
            {
                var assembly = LoadFromAssemblyPath(x.Assembly.Location);
                _recipeAssemblies.Add(assembly);
                return assembly.GetType(x.FullName!)!;
            })
            .Select(Activator.CreateInstance)
            .Cast<CodeFixProvider>()
            .SelectMany(fix => fix.FixableDiagnosticIds.Select(x => (Id: x, Fixer: fix)))
            .DistinctBy(x => x.Id)
            .ToDictionary(x => x.Id, x => x.Fixer);

        var fixupRecipes = analyzerRecipes
            .Join(fixersById, x => x.Id, x => x.Key, (descriptor, fixer) => (Descriptor: descriptor, Fixer: fixer.Value))
            .Select(x => x.Descriptor with
            {
                Kind = RecipeKind.RoslynFixer,
                TypeName = TypeName.Parse(x.Fixer.GetType().AssemblyQualifiedName!),
                Tags = ["roslyn", "codefix", x.Descriptor.Id],
            })
            .ToList();

        return openRewriteRecipes.Union(analyzerRecipes).Union(fixupRecipes).ToList();
        //
        // foreach (var assemblyPath in _assemblies.Values)
        // {
        //     
        //     var metadataAssembly = mlc.LoadFromAssemblyPath(assemblyPath);
        //     var recipeAttribute = metadataAssembly.GetCustomAttributesData()
        //         .FirstOrDefault(x => x.AttributeType == mlcRecipeAttributeType);
        //
        //     if (recipeAttribute != null)
        //     {
        //         IReadOnlyCollection<CustomAttributeTypedArgument> recipeAttributeArguments = recipeAttribute
        //             .ConstructorArguments[0].Value as IReadOnlyCollection<CustomAttributeTypedArgument> ?? new List<CustomAttributeTypedArgument>();
        //         var recipeDescriptorsJson = recipeAttributeArguments
        //             .Where(x => x.Value != null)
        //             .Select(x => (string)x.Value!)
        //             .ToList();
        //         var recipeDescriptors = recipeDescriptorsJson.Select(x => JsonConvert.DeserializeObject<RecipeDescriptor>(x)).ToList();
        //         results.AddRange(recipeDescriptors.Select(x => (assemblyPath, x!)));
        //     }
        //
        //     var hasAnalyzersOrFixups = metadataAssembly.ExportedTypes
        //         .Any(x => x.GetCustomAttributesData().Any(attr => attr.AttributeType == mlcDiagnosticAnalyzerAttributeType || attr.AttributeType == mlcCodeFixupAttributeType));
        //     if (hasAnalyzersOrFixups)
        //     {
        //         // for roslyn based recipes, we need to actually load them to grab descriptions
        //         // var loadedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        //
        //         var loadedAssembly = this.LoadFromAssemblyPath(assemblyPath);
        //         var diagnosticAnalyzerType = typeof(DiagnosticAnalyzerAttribute);
        //         var analyzerTypes = loadedAssembly.ExportedTypes.Where(x => x.GetCustomAttributesData().Any(attr => attr.AttributeType == diagnosticAnalyzerType)).ToList();
        //         var analyzers = analyzerTypes.Select(Activator.CreateInstance).Cast<DiagnosticAnalyzer>().ToList();
        //         var analyzersWithDescriptors = analyzers
        //             .SelectMany(analyzer => analyzer.SupportedDiagnostics
        //                 .DistinctBy(x => x.Id)
        //                 .Select(descriptor => (analyzer, descriptor)))
        //             .ToList();
        //         
        //         var codeFixupTypes = loadedAssembly.ExportedTypes.Where(x => x.GetCustomAttributesData().Any(attr => attr.AttributeType == mlcCodeFixupAttributeType)).ToList();
        //         var codeFixups = codeFixupTypes.Select(Activator.CreateInstance).Cast<CodeFixProvider>().ToList();
        //         var fixableIds = codeFixups
        //             .SelectMany(analyzer => analyzer.FixableDiagnosticIds)
        //             .ToHashSet();
        //
        //         analyzersWithDescriptors = analyzersWithDescriptors
        //             .Where(x => fixableIds.Contains(x.descriptor.Id))
        //             .ToList();
        //         
        //         var roslynBasedRecipeDescriptors = analyzersWithDescriptors.Select(x => new RecipeDescriptor()
        //         {
        //             Id = x.descriptor.Id,
        //             Kind = RecipeKind.RoslynFixer,
        //             DisplayName = x.descriptor.Title.ToString(),
        //             Description = x.descriptor.Description.ToString(),
        //             TypeName = TypeName.Parse(x.analyzer.GetType().AssemblyQualifiedName!),
        //             Tags = ["roslyn"],
        //             Options = OptionDescriptor.FromRecipeType<RoslynRecipe>() 
        //         }).ToList();
        //         results.AddRange(roslynBasedRecipeDescriptors.Select(x => (assemblyPath, x)));
        //     }
        // }

        // return results.ToLookup(x => x.Path, x => x.Recipie);
    }
    
    
     public RecipeStartInfo CreateRecipeStartInfo(RecipeDescriptor recipeDescriptor)
    {
        var startInfo = new RecipeStartInfo()
        {
            Id = recipeDescriptor.Id,
            DisplayName = recipeDescriptor.DisplayName,
            Description = recipeDescriptor.Description,
            Kind = recipeDescriptor.Kind,
            TypeName = recipeDescriptor.TypeName,
            // NugetPackageId = Package,
            Arguments = recipeDescriptor.Options.Select(x => new RecipeArgument
            {
                Name = x.Name,
                Description = x.Description,
                Type = x.Type,
                DisplayName = x.DisplayName,
                Example = x.Example,
                Required = x.Required,
            }).ToDictionary(x => x.Name, x => x)
        };
        return startInfo;
    }

    public RecipeStartInfo CreateRecipeStartInfo(RecipeDescriptor recipeDescriptor, Dictionary<string, JToken> arguments)
    {
        var startInfo = CreateRecipeStartInfo(recipeDescriptor);
        var requiredOptionNames = startInfo.Arguments.Values.Where(x => x.Required).Select(x => x.Name).ToHashSet();
        var providedOptionNames = arguments.Keys.ToHashSet();
        var missingOptions = requiredOptionNames.ToHashSet();
        missingOptions.ExceptWith(providedOptionNames);
        if (missingOptions.Count > 0)
        {
            throw new ArgumentException($"Missing required options for recipe {recipeDescriptor.Id}: {string.Join(", ", missingOptions)}");
        }
        foreach (var (propertyName, propertyValueToken) in arguments)
        {
            if (!startInfo.Arguments.TryGetValue(propertyName, out var argument))
            {
                throw new ArgumentException($"Recipe option {propertyName} not found as part of recipe {recipeDescriptor.Id} yet was provided as an argument");
            }
            argument.Value = propertyValueToken.ToObject(argument.GetArgumentType());
        }
        return startInfo;
    }

    public RecipeDescriptor GetRecipeDescriptor(string id)
    {
        return Recipes.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Recipe not loaded");
    }
}