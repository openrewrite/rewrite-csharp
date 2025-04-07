// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly

using NuGet.Configuration;
using Nuke.Common.IO;
using Rewrite.CSharp.Tests;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Rewrite.MSBuild.Tests;

public class GlobalHooks
{
    [Before(TestSession)]
    public static void Initialize()
    {
        CommonTestHooks.Initialize();
    }
    [Before(TestDiscovery)]
    public static void CleanNugetDirectory()
    {
        var globalPackagesFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
        var rewritePackagesInGlobalCache = Directory.EnumerateDirectories(globalPackagesFolder, "Rewrite.*", SearchOption.TopDirectoryOnly)
            .Select(x => (AbsolutePath)x)
            .ToList();
        foreach (var rewritePackagePath in rewritePackagesInGlobalCache)
        {
            rewritePackagePath.DeleteDirectory();
        }
    }

    [After(TestSession)]
    public static void CleanUp()
    {
    }
}