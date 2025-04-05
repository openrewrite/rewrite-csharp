using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
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
    private Dictionary<AssemblyName, AbsolutePath> _recipeAssemblies;
    private Dictionary<RecipeDescriptor, AbsolutePath> _recipeAssemblyMap;
    public IReadOnlyList<RecipeDescriptor> Recipes { get; private set; }

    private static IEqualityComparer<AssemblyName> _assemblyNameEqualityComparer = EqualityComparer<AssemblyName>.Create((a, b) => a?.Name == b?.Name, a => a.Name!.GetHashCode());

    public RecipeExecutionContext(PackageIdentity recipePackageName, List<AbsolutePath> assemblies) : base(recipePackageName.ToString())
    {
        _assemblies = assemblies.ToDictionary(x => AssemblyName.GetAssemblyName(x), x => x, _assemblyNameEqualityComparer);
        var recipes = FindRecipeAssemblies();
        _recipeAssemblies = recipes.Select(x => x.Key).ToDictionary(x => AssemblyName.GetAssemblyName(x), x => x);
        _recipeAssemblyMap = recipes.SelectMany(x => x.Select(y => (Path: x.Key, Recipe: y))).ToDictionary(x => x.Recipe, x => x.Path);
        Recipes = recipes.SelectMany(x => x).ToList();
    }

    public Recipe CreateRecipe(RecipeDescriptor descriptor, RecipeStartInfo recipeArguments)
    {
        // if (!_recipeAssemblyMap.TryGetValue(descriptor, out var assemblyPath))
        //     throw new InvalidOperationException($"The provided recipe descriptor for recipe {descriptor} is not part of execution context {Name}");
        // var assembly = LoadFromAssemblyPath(assemblyPath);
        var assembly = LoadFromAssemblyName(new AssemblyName(descriptor.TypeName.AssemblyName));
        var recipeType = assembly.GetType(descriptor.TypeName.FullName) ?? throw new InvalidOperationException($"Cannot resolve recipe type {descriptor.TypeName.FullName} from {assembly.FullName}");
        var recipe = (Recipe)Activator.CreateInstance(recipeType)!;
        // todo: add validation to ensure everything is satisfied
        var recipeProperties = recipeType.GetProperties().Join(recipeArguments.Arguments.Values, x => x.Name, x => x.Name, (property, argument) => (property, argument)).ToList();
        foreach(var (propertyInfo, argument) in recipeProperties)
        {
            propertyInfo.SetValue(recipe, argument.Value);
        }
        return recipe;
    }
    
    private List<Recipe> FindAllRecipes(Assembly assembly)
    {
        var recipes = new List<Recipe>();
        foreach (var type in assembly.GetExportedTypes())
        {
            if (!typeof(Recipe).IsAssignableFrom(type)) continue;

            var constructorInfo = type.GetConstructors()[0];
            var parameters = new object?[constructorInfo.GetParameters().Length];
            for (var index = 0; index < constructorInfo.GetParameters().Length; index++)
            {
                var parameterInfo = constructorInfo.GetParameters()[index];
                var parameterInfoParameterType = parameterInfo.ParameterType;

                parameters[index] = parameterInfoParameterType.IsValueType
                    ? Activator.CreateInstance(parameterInfoParameterType)
                    : null;
            }

            recipes.Add((Recipe)constructorInfo.Invoke(parameters));
        }

        return recipes;
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

    
    private ILookup<AbsolutePath, RecipeDescriptor> FindRecipeAssemblies()
    {
        var results = new List<(AbsolutePath Path, RecipeDescriptor Recipie)>();
        // var resolver = new PathAssemblyResolver(assembliesInRecipePackage.Select(x => x.Name.ToString()));
        // var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        // var currentDirAssemblies = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "*.dll");
        // var allAssemblies = runtimeAssemblies.Union(currentDirAssemblies);
        // var resolver = new PathAssemblyResolver(runtimeAssemblies);
        var resolver = new RecipieAssemblyResolver();
        using var mlc = new MetadataLoadContext(resolver);
        
        var recipeAttributeAssembly = mlc.LoadFromAssemblyPath(typeof(RecipesAttribute).Assembly.Location);
        var recipieAttributeType = recipeAttributeAssembly.GetType(typeof(RecipesAttribute).FullName!)!;
        // Load assembly into MetadataLoadContext.
        foreach (var assemblyPath in _assemblies.Values)
        {
            
            var assembly = mlc.LoadFromAssemblyPath(assemblyPath);
            var recipeAttribute = assembly.GetCustomAttributesData()
                .FirstOrDefault(x => x.AttributeType == recipieAttributeType);

            if (recipeAttribute != null)
            {
                IReadOnlyCollection<CustomAttributeTypedArgument> recipeAttributeArguments = recipeAttribute.ConstructorArguments[0].Value as IReadOnlyCollection<CustomAttributeTypedArgument> ?? new List<CustomAttributeTypedArgument>();
                var recipeDescriptorsJson = recipeAttributeArguments
                    .Where(x => x.Value != null)
                    .Select(x => (string)x.Value!)
                    .ToList();
                var recipeDescriptors = recipeDescriptorsJson.Select(x => JsonConvert.DeserializeObject<RecipeDescriptor>(x)).ToList();
                results.AddRange(recipeDescriptors.Select(x => (assemblyPath, x!)));
            }
            // if (recipeAttribute != null)
            // {
            //     // JsonConvert.DeserializeObject<>()
            //     results.Add(assembly.GetName(), assemblyPath);
            // }
        }

        return results.ToLookup(x => x.Path, x => x.Recipie);
    }
    
    
    
    
}