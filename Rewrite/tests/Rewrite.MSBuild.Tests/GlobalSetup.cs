// Here you could define global logic that would affect all tests

// You can use attributes at the assembly level to apply to all tests in the assembly

using NuGet.Configuration;
using Nuke.Common.IO;
using Rewrite.CSharp.Tests;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Rewrite.MSBuild.Tests;

public class GlobalHooks
{
    public static int BeforeHookCallsCount = 0;
    [Before(Assembly)]
    public static void AssemblySetUp()
    {
        TestSetup.BeforeAssembly();
    }
    [Before(TestDiscovery)]
    public static void SetUp()
    {
        BeforeHookCallsCount++;
        // var loggerConfig = new LoggerConfiguration()
        //     .MinimumLevel.Debug()
        //     .Destructure.With<PrettyJsonDestructuringPolicy>()
        //     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}", applyThemeToRedirectedOutput: true, theme: AnsiConsoleTheme.Literate);
        // var log = loggerConfig.CreateLogger();
        var globalPackagesFolder = (AbsolutePath)SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));
        var rewritePackagesInGlobalCache = Directory.EnumerateDirectories(globalPackagesFolder, "Rewrite.*", SearchOption.TopDirectoryOnly)
            .Select(x => (AbsolutePath)x)
            .ToList();
        foreach (var rewritePackagePath in rewritePackagesInGlobalCache)
        {
            rewritePackagePath.DeleteDirectory();
            // for (int i = 0; i < 5; i++)
            // {
            //     try
            //     {
            //         if(Directory.Exists(rewritePackagePath))
            //             Directory.Delete(rewritePackagePath, recursive: true);
            //         break;
            //     }
            //     catch (UnauthorizedAccessException)
            //     {
            //         Console.WriteLine($"Skipping rewrite package {rewritePackagePath}");
            //         // log.Information($"Skipping rewrite package {rewritePackagePath}");
            //         Thread.Sleep(1000);
            //         continue;
            //     }
            // }
        }
    }

    [After(TestSession)]
    public static void CleanUp()
    {
    }
}