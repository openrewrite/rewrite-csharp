using Microsoft.CodeAnalysis;

namespace Rewrite.MSBuild.Restore;

public class CompilerUtils
{
    public static string GetCompilerApiVersion()
    {
        var assemblyName = typeof(Compilation).Assembly.GetName();
        var version = assemblyName.Version;

        if (version == null)
            throw new InvalidOperationException("Could not determine version from Roslyn tasks assembly.");

        string roslynApiVersion = $"{version.Major}.{version.Minor}";
        return $"roslyn{roslynApiVersion}";
    }
}