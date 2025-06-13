using Newtonsoft.Json.Linq;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Rewrite.Core;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

/// <summary>
/// Nuget package with recipes that are available inside it
/// </summary>
/// <param name="packageIdentity"></param>
/// <param name="recipes"></param>
public class RecipePackageInfo(PackageIdentity packageIdentity, IReadOnlyList<RecipeDescriptor> recipes)
{
    private readonly IReadOnlyDictionary<string, RecipeDescriptor> _recipeDescriptors = recipes.ToDictionary(x => x.Id, x => x);
    public PackageIdentity Package => packageIdentity;
    public IReadOnlyList<RecipeDescriptor> Recipes => recipes;

    public RecipeDescriptor GetRecipeDescriptor(InstallableRecipe installableRecipe) => _recipeDescriptors[installableRecipe.Id];
    public RecipeDescriptor GetRecipeDescriptor(string id)
    {
        if (!_recipeDescriptors.TryGetValue(id, out var recipeDescriptor))
        {
            throw new InvalidOperationException($"Recipe ID {id} not found in package {Package}");
        }
        return recipeDescriptor!;
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
            NugetPackageId = Package,
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
}
