using NuGet.Packaging.Core;
using Rewrite.Core;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

public class RecipePackageInfo(PackageIdentity packageIdentity, IReadOnlyList<RecipeDescriptor> recipes)
{
    private readonly IReadOnlyDictionary<string, RecipeDescriptor> _recipeDescriptors = recipes.ToDictionary(x => x.Id, x => x);
    public PackageIdentity Package => packageIdentity;
    public IReadOnlyList<RecipeDescriptor> Recipes => recipes;

    public RecipeDescriptor GetRecipeDescriptor(InstallableRecipe installableRecipe) => _recipeDescriptors[installableRecipe.Id];
    public RecipeDescriptor GetRecipeDescriptor(string id) => _recipeDescriptors[id];
}
