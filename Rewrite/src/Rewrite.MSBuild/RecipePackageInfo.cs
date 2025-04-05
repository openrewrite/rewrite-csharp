using NuGet.Packaging.Core;
using Rewrite.Core;
using Rewrite.Core.Config;

namespace Rewrite.MSBuild;

public class RecipePackageInfo(PackageIdentity packageIdentity, IReadOnlyList<RecipeDescriptor> recipes)
{
    public PackageIdentity Package => packageIdentity;
    public IReadOnlyList<RecipeDescriptor> Recipes => recipes;
}
