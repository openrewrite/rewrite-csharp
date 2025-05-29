using System.Diagnostics.CodeAnalysis;
using CommandLine;
using NuGet.Configuration;

namespace Rewrite.Remote.Server;

[UsedImplicitly]
public class Options
{
    [Option('p', "port", Required = false, HelpText = "Set port on which server is running.")]
    public int Port { get; set; } = 54321;

    [Option('t', "timeout", Required = false, HelpText = "Set timeout for each request in milliseconds.")]
    public int TimeoutInMilliseconds { get; set; } = int.MaxValue / 1000;

    public TimeSpan Timeout => TimeSpan.FromMilliseconds(TimeoutInMilliseconds);

    [Option('l', "logfile", Required = false, HelpText = "Set log file path.")]
    public string? LogFilePath { get; set; }

    
    [Option('z', "dummy", Required = false, HelpText = "Spawns a process that doesn't do anything. Useful for launching tests when a real server is already running under IDE debug.")]
    public bool IsDummy { get; set; }

    

}
