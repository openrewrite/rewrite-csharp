using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rewrite.Diagnostics;
using Rewrite.MSBuild;
using Rewrite.Remote.Server;
using Serilog;
using Parser = CommandLine.Parser;



var parseResult = new Parser().ParseArguments<Options>(args);
parseResult.WithNotParsed(x =>
{
    var helpText = HelpText.AutoBuild(parseResult, h => HelpText.DefaultParsingErrorsHandler(parseResult, h), e => e);
    Console.Error.WriteLine(helpText);
    Environment.Exit(1);
});

var options = parseResult.Value;

Logging.ConfigureLogging(options.LogFilePath);

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.AddHostedService<Server>();
builder.Services.AddSingleton<RecipeManager>();
builder.Services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
builder.Services.AddSingleton(options);
var app = builder.Build();
app.Run();
