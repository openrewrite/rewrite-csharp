using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

public abstract class HostConfigurationCommand<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected abstract void ConfigureHost(HostApplicationBuilder host);
    public sealed override int Execute(CommandContext context, TSettings settings)
    {
        var hostBuilder = context.Data as HostApplicationBuilder ?? throw new InvalidOperationException("Context data is not set to HostApplicationBuilder");
        ConfigureHost(hostBuilder);
        return 0;
    }
}