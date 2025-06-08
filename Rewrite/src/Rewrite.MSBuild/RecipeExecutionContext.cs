using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using NMica.Utils.IO;
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
using Rewrite.Core;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

public class RecipeExecutionContext : AssemblyLoadContext
{
    private Dictionary<AssemblyName, AbsolutePath> _assemblies;
    private readonly HashSet<Assembly> _recipeAssemblies = [];
    public IReadOnlyList<RecipeDescriptor> Recipes { get; private set; }

    private static IEqualityComparer<AssemblyName> _assemblyNameEqualityComparer = EqualityComparer<AssemblyName>.Create((a, b) => a?.Name == b?.Name, a => a.Name!.GetHashCode());

    public RecipeExecutionContext(PackageIdentity recipePackageName, List<AbsolutePath> assemblies) : base(recipePackageName.ToString())
    {
        SyntaxFactory.CompilationUnit();
        _assemblies = assemblies
            .Where(x => !x.Name.EndsWith(".resources.dll"))
            .ToDictionary(x => AssemblyName.GetAssemblyName(x), x => x, _assemblyNameEqualityComparer);
        Recipes = FindRecipesInAssemblies();
        // Recipes = recipes
            // .SelectMany(x => x)
            // .GroupBy(x => x.Id)
            // .Select(x => x.First()) // eliminate roslyn recipes where multiple types address same diagnostic id
            // .ToList();
    }
    

    public Recipe CreateRecipe(RecipeStartInfo recipeStartInfo)
    {
        // if (!_recipeAssemblyMap.TryGetValue(descriptor, out var assemblyPath))
        //     throw new InvalidOperationException($"The provided recipe descriptor for recipe {descriptor} is not part of execution context {Name}");
        // var assembly = LoadFromAssemblyPath(assemblyPath);
        var recipe = recipeStartInfo.Kind switch
        {
            RecipeKind.OpenRewrite => CreateOpenRewriteRecipe(recipeStartInfo),
            RecipeKind.RoslynAnalyzer => CreateRoslynRecipe(recipeStartInfo, fixup: false),
            RecipeKind.RoslynFixer =>  CreateRoslynRecipe(recipeStartInfo, fixup: true),
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

    private Recipe CreateRoslynRecipe(RecipeStartInfo recipeStartInfo, bool fixup)
    {
        if (!recipeStartInfo.Arguments.TryGetValue(nameof(RoslynRecipe.SolutionFilePath), out var solutionFilePath) || solutionFilePath.Value is null)
            throw new InvalidOperationException($"{nameof(RoslynRecipe)} requires {nameof(RoslynRecipe.SolutionFilePath)} property to be set to valid path");
        var recipeTypes = GetRecipeType(recipeStartInfo);
        var roslynRecipe = new RoslynRecipe(_recipeAssemblies)
        {
            DiagnosticId = recipeStartInfo.Id,
            ApplyFixer = fixup,
            SolutionFilePath = (string)solutionFilePath.Value
        };
        return roslynRecipe;
    }

    private Type GetRecipeType(RecipeStartInfo recipeStartInfo)
    {
        var assembly = LoadFromAssemblyName(new AssemblyName(recipeStartInfo.TypeName.AssemblyName));
        var recipeType = assembly.GetType(recipeStartInfo.TypeName.FullName) ?? throw new InvalidOperationException($"Cannot resolve recipe type {recipeStartInfo.TypeName.FullName} from {assembly.FullName}");
        return recipeType;
    }

    // private List<Recipe> FindAllRecipes(Assembly assembly)
    // {
    //     var recipes = new List<Recipe>();
    //     foreach (var type in assembly.GetExportedTypes())
    //     {
    //         if (!typeof(Recipe).IsAssignableFrom(type)) continue;
    //
    //         var constructorInfo = type.GetConstructors()[0];
    //         var parameters = new object?[constructorInfo.GetParameters().Length];
    //         for (var index = 0; index < constructorInfo.GetParameters().Length; index++)
    //         {
    //             var parameterInfo = constructorInfo.GetParameters()[index];
    //             var parameterInfoParameterType = parameterInfo.ParameterType;
    //
    //             parameters[index] = parameterInfoParameterType.IsValueType
    //                 ? Activator.CreateInstance(parameterInfoParameterType)
    //                 : null;
    //         }
    //
    //         recipes.Add((Recipe)constructorInfo.Invoke(parameters));
    //     }
    //
    //     return recipes;
    // }

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

        // var candidateAssembly = AppDomain.CurrentDomain.GetAssemblies()
        //     .FirstOrDefault(x => 
        //         // x.GetName().Name!.StartsWith("Microsoft.CodeAnalysis") && 
        //                          x.GetName().Name == assemblyName.Name);
        // if (candidateAssembly != null)
        // {
        //     return candidateAssembly;
        // }
        return null;
    }

    
    private List<RecipeDescriptor> FindRecipesInAssemblies()
    {
        // var results = new List<(AbsolutePath Path, RecipeDescriptor Recipie)>();
        var results = new List<RecipeDescriptor>();
        // var resolver = new PathAssemblyResolver(assembliesInRecipePackage.Select(x => x.Name.ToString()));
        // var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        // var currentDirAssemblies = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "*.dll");
        // var allAssemblies = runtimeAssemblies.Union(currentDirAssemblies);
        // var resolver = new PathAssemblyResolver(runtimeAssemblies);
        var resolver = new RecipieAssemblyResolver();
        using var mlc = new MetadataLoadContext(resolver);
        var mlcRecipeAttributeAssembly = mlc.LoadFromAssemblyPath(typeof(RecipesAttribute).Assembly.Location);
        var mlcRecipeAttributeType = mlcRecipeAttributeAssembly.GetType(typeof(RecipesAttribute).FullName!)!;
        var mlcDiagnosticAnalyzerAssembly = mlc.LoadFromAssemblyPath(typeof(DiagnosticAnalyzer).Assembly.Location);
        var mlcDiagnosticAnalyzerAttributeType = mlcDiagnosticAnalyzerAssembly.GetType(typeof(DiagnosticAnalyzerAttribute).FullName!)!;
        var mlcCodeFixupAssembly = mlc.LoadFromAssemblyPath(typeof(ExportCodeFixProviderAttribute).Assembly.Location);
        var mlcCodeFixupAttributeType = mlcCodeFixupAssembly.GetType(typeof(ExportCodeFixProviderAttribute).FullName!)!;
        // var loadedDiagnosticAnalyzerType = LoadFromAssemblyPath(typeof(DiagnosticAnalyzerAttribute).Assembly.Location);
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
            .SelectMany(x => x.ExportedTypes)
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
                Tags = ["roslyn"],
                Options = OptionDescriptor.FromRecipeType<RoslynRecipe>() 
            })
            .ToList();
        
        
        var fixersById = metadataAssemblies
            .SelectMany(x => x.ExportedTypes)
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
            })
            .ToList();

        return openRewriteRecipes.Union(fixupRecipes).ToList();
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
    
    
    
    
}