using NuGet.Versioning;

namespace Rewrite.MSBuild;

public record Dependency(string Package, string Version)
{
}