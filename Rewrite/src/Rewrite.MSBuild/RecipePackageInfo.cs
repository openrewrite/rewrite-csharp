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

   
}
