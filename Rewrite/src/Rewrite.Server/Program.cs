using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Rewrite.Server.Commands;
using Rewrite.Server.Commands.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Parser = CommandLine.Parser;


var builder = Host.CreateApplicationBuilder(args);
// builder.Logging.ClearProviders();
var commandApp = new CommandApp();
// we want to modify the buildup of the service collection so we need to build command app that doesn't really do anything
// but lets us tweak each  
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
    c.SetExceptionHandler((ex, resolver) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    });
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