using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

public class InstallableRemotingRecipe(string repository, string version, List<RecipeDescriptor> recipes)
{
    public string Repository => repository;
    public string Version => version;
    public List<RecipeDescriptor> Recipes => recipes;
}
