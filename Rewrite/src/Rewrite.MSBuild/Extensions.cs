using NuGet.Packaging.Core;
using NuGet.Versioning;
using Rewrite.Core;

namespace Rewrite.MSBuild;

public static class Extensions
{
    public static PackageIdentity GetPackageIdentity(this InstallableRecipe installableRecipe) => new PackageIdentity(installableRecipe.NugetPackageName, NuGetVersion.Parse(installableRecipe.NugetPackageVersion));
}