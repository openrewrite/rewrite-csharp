#nullable disable

using NuGet.ProjectModel;
using NuGet.Versioning;

namespace Rewrite.MSBuild.Restore;

internal interface IPackageResolver
{
    string GetPackageDirectory(string packageId, NuGetVersion version);
    string GetPackageDirectory(string packageId, NuGetVersion version, out string packageRoot);
    string ResolvePackageAssetPath(LockFileTargetLibrary package, string relativePath);
}