using System.ComponentModel;
using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using Rewrite.Core;
using Rewrite.Diagnostics;
using Rewrite.MSBuild;
using Rewrite.Remote.Server;
using Rewrite.Remote.Server.Commands;
using Rewrite.Remote.Server.Commands.Infrastructure;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Parser = CommandLine.Parser;

// var parseResult = new Parser().ParseArguments<Options>(args);
// parseResult.WithNotParsed(x =>
// {
//     var helpText = HelpText.AutoBuild(parseResult, h => HelpText.DefaultParsingErrorsHandler(parseResult, h), e => e);
//     Console.Error.WriteLine(helpText);
//     Environment.Exit(1);
// });
var builder = Host.CreateApplicationBuilder(args);
// builder.Logging.ClearProviders();
var commandApp = new CommandApp();
commandApp.Configure(c =>
{
    c.AddCommand<NoOpCommand<LanguageServerCommand.Settings>>("server");
    c.AddCommand<NoOpCommand<RunRecipeCommand.Settings>>("run-recipe");
    c.SetInterceptor(new ConfigureInterceptor(builder));
});
var result = await commandApp.RunAsync(args);
if (result != 0)
{
    Environment.Exit(result);
}

var registrar = new HostRegistrar(builder);

// actual command app that is powered by DI
commandApp = new CommandApp(registrar);
commandApp.Configure(c =>
{
    c.AddCommand<LanguageServerCommand>("server");
    c.AddCommand<RunRecipeCommand>("run-recipe");
});
await commandApp.RunAsync(args);

//
// Logging.ConfigureLogging(options.LogFilePath);
//
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog();
// builder.Services.AddHostedService<Server>();
// builder.Services.AddSingleton<RecipeManager>();
// builder.Services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
// builder.Services.AddSingleton(options);
// var app = builder.Build();
// app.Run();
public class NoOpCommand<T> : Command<T> where T : CommandSettings
{
    public override int Execute(CommandContext context, T settings) => 0;
}
public class ConfigureInterceptor(HostApplicationBuilder builder) : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        var commonSettings = (BaseSettings)settings;
        Logging.ConfigureLogging(commonSettings.LogFilePath);
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Services.AddSingleton<RecipeManager>();
        builder.Services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
        builder.Services.AddSingleton(commonSettings);
        if (context.Name == "server")
        {
            builder.Services.AddHostedService<Server>();
        }
    }
}

public class BaseSettings : CommandSettings
{
    [CommandOption("-l|--logfile")]
    [Description("Log file path. Default: disabled")]
    public string? LogFilePath { get; set; }
}

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


public class RunRecipeCommand(RecipeManager recipeManager) : AsyncCommand<RunRecipeCommand.Settings>
{
    public class Settings : BaseSettings
    {
        [CommandOption("-s|--solution <SOLUTION>")]
        [Description("Path to solution file")]
        public required string SolutionPath { get; set; }

        [CommandOption("-p|--package <NAME>")]
        [Description("Nuget Package Name")]
        public required string PackageName { get; set; }

        [CommandOption("-v|--version <VERSION>")]
        [Description("Nuget Package Version")]
        public required string PackageVersion { get; set; }

        [CommandOption("-i|--id <VERSION>")]
        [Description("Recipe ID. For Open Rewrite recipes this is namespace qualified type name. For Roslyn recipies this is the diagnostic ID (ex. CS1123)")]
        public required string Id { get; set; }
        
        public override ValidationResult Validate()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (SolutionPath == null)
            {
                return ValidationResult.Error("Solution path must be specified");
            }
            if (Id == null)
            {
                return ValidationResult.Error("Id must be specified");
            }
            if (PackageVersion == null)
            {
                return ValidationResult.Error("Recipe Id must be specified");
            }
            if (PackageName == null)
            {
                return ValidationResult.Error("Package Version must be specified");
            }
            // ReSharper enable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

            return base.Validate();
        }
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        string[] feeds =
        [
            "https://api.nuget.org/v3/index.json"
        ];
        
        var packageSources = feeds.Select(x => new PackageSource(x)).ToList();
        // var recipeManager = new RecipeManager();
        // CA1802: Use Literals Where Appropriate
        // CA1861: Avoid constant arrays as arguments
        var installableRecipe = new InstallableRecipe(settings.Id, settings.PackageName, settings.PackageVersion);
        var recipesPackage = await recipeManager.InstallRecipePackage(installableRecipe, packageSources);
        var recipeDescriptor = recipesPackage.GetRecipeDescriptor(installableRecipe.Id);
        var recipeStartInfo = recipeDescriptor.CreateRecipeStartInfo();
        recipeStartInfo.WithOption(nameof(RoslynRecipe.SolutionFilePath), settings.SolutionPath);
        var recipe = (RoslynRecipe)recipeManager.CreateRecipe(installableRecipe, recipeStartInfo);
        var affectedDocuments = await recipe.Execute(CancellationToken.None);
        foreach (var affectedDocument in affectedDocuments)
        {
            Console.WriteLine(affectedDocument.FilePath);
        }

        return 0;
    }
}