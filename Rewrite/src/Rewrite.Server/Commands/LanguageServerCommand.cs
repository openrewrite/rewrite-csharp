using System.ComponentModel;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

public class LanguageServerCommand(IHost host) : AsyncCommand<LanguageServerCommand.Settings>
{
    private readonly IHost _host = host;

    public class Settings : BaseSettings
    {
        [CommandOption("-p|--port <PORT>")]
        [Description("Port the server will listen on. Default: 54321")]
        public int Port { get; set; } = 54321;

        [CommandOption("-t|--timeout <TIMEOUT>")]
        [Description("Connection timeout in milliseconds. Default: infinite")]
        public int TimeoutInMilliseconds { get; set; } = int.MaxValue / 1000;
        public TimeSpan Timeout => TimeSpan.FromMilliseconds(TimeoutInMilliseconds);

        
    }

    // public class ConfigureHost : HostConfigurationCommand<Settings>
    // {
    //     protected override void ConfigureHost(ServiceCollection services) => services.AddHostedService<Server>();
    // }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await _host.RunAsync();
        return 0;
    }
}