using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using Rewrite.Diagnostics;
using Rewrite.MSBuild;
using Serilog;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

public class ConfigureInterceptor(HostApplicationBuilder builder) : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    { 
        var commonSettings = (BaseSettings)settings;
        Logging.ConfigureLogging(commonSettings.LogFilePath);
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Configuration
            .AddYamlFile("appsettings.yaml", optional: true)
            .AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yaml", optional: true)
            .AddEnvironmentVariables();
        builder.Services.AddSingleton<ISettings>(serviceProvider =>
        {
            var settingsLocal = serviceProvider.GetRequiredService<BaseSettings>();
            var nugetSettings = Settings.LoadDefaultSettings(settingsLocal.NugetConfigRoot);
            return nugetSettings;
        });
        builder.Services.AddSingleton<RecipeManager>();
        builder.Services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
        builder.Services.AddSingleton(commonSettings);
        if (context.Name == "server")
        {
            builder.Services.AddHostedService<Remote.Server.Server>();
        }
    }
}