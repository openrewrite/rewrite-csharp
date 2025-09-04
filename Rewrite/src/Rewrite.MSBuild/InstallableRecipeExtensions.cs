using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Rewrite.Core;

namespace Rewrite.MSBuild;

public static class InstallableRecipeExtensions
{
    public static LibraryRange GetLibraryRange(this InstallableRecipe recipeId)
    {
        return GetLibraryRange(recipeId.NugetPackageName, recipeId.NugetPackageVersion);
    }

    public static LibraryRange GetLibraryRange(string package)
    {
        var parts = package.Split(":");
        if(parts.Length > 2)
            throw new InvalidOperationException($"Package name {package} is invalid");
        var name = parts[0];
        
        var version = parts.Length > 2 ? parts[1] : PackageVersionMoniker.Release;
        return GetLibraryRange(name, version);
    }
    public static LibraryRange GetLibraryRange(string package, string version)
    {
        var versionRange = version switch
        {
            PackageVersionMoniker.Snapshot => VersionRange.All,
            PackageVersionMoniker.Release => VersionRange.AllStable,
            _ => VersionRange.Combine([NuGetVersion.Parse(version)]),
        };
        return new LibraryRange(package, versionRange, LibraryDependencyTarget.Package);
    }
}

public static class PackageVersionMoniker
{
    public const string Snapshot = "SNAPSHOT";
    public const string Release = "RELEASE";
    
}

