#nullable disable

namespace Rewrite.MSBuild.Restore;

public sealed class ExecutableExtension
{
    public static string ForRuntimeIdentifier(string runtimeIdentifier)
    {
        if (runtimeIdentifier.StartsWith("win", StringComparison.OrdinalIgnoreCase))
        {
            return ".exe";
        }
        return string.Empty;
    }
}