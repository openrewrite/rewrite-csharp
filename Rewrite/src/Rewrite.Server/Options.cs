using CommandLine;
using NuGet.Configuration;

namespace Rewrite.Remote.Server;

public class Options
{
    [Option('p', "port", Required = false, HelpText = "Set port on which server is running.")]
    public int Port { get; set; } = 54321;

    [Option('t', "timeout", Required = false,
        HelpText = "Set timeout for each request in milliseconds.")]
    public int TimeoutInMilliseconds { get; set; } = int.MaxValue / 1000;

    public TimeSpan Timeout => TimeSpan.FromMilliseconds(TimeoutInMilliseconds);

    [Option('l', "logfile", Required = false, HelpText = "Set log file path.")]
    public string? LogFilePath { get; set; }

    [Option('f', "nupkgfolder", Required = false, HelpText = "Set nuget packages folder path.")]
    public string NugetPackagesFolder { get; set; } = SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(null));

}
