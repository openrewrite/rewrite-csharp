// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly

using NuGet.Configuration;
using Nuke.Common.IO;
using Rewrite.CSharp.Tests;
using Rewrite.Tests;
using Spectre.Console;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Rewrite.MSBuild.Tests;

public class GlobalHooks
{
    [Before(TestSession)]
    public static void BeforeTestSession()
    {
// #pragma warning disable CA1416
//         Console.WindowWidth = 200;
// #pragma warning restore CA1416
        AnsiConsole.Console.Profile.Width = 220;
        AnsiConsole.Profile.Capabilities.ColorSystem = ColorSystem.TrueColor;
        AnsiConsole.Profile.Capabilities.Ansi = true;
        CommonTestHooks.BeforeTestSession();
    }
    [Before(TestDiscovery)]
    public static void CleanNugetDirectory()
    {
        var globalPackagesFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
        var rewritePackagesInGlobalCache = Directory.EnumerateDirectories(globalPackagesFolder, "Rewrite.*", SearchOption.TopDirectoryOnly)
            .Select(x => (AbsolutePath)x / ThisAssembly.AssemblyInformationalVersion.Replace("+","-"))
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