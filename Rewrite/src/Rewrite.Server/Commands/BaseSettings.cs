using System.ComponentModel;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

public class BaseSettings : CommandSettings
{
    [CommandOption("-l|--logfile")]
    [Description("Log file path. Default: disabled")]
    public string? LogFilePath { get; set; }
}