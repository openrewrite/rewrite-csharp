using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}");
if (options.LogFilePath != null)
{
    loggerConfig = loggerConfig.WriteTo.File(options.LogFilePath);
}

Log.Logger = loggerConfig.CreateLogger();

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.AddLogging(log => log.AddSerilog(Log.Logger));
builder.Services.AddHostedService<Server>();
builder.Services.AddSingleton<RecipeManager>();
builder.Services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
builder.Services.AddSingleton(options);
var app = builder.Build();
app.Run();
