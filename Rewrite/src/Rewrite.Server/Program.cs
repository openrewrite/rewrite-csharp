using Microsoft.CodeAnalysis;
using Rewrite.Remote.Server;
using Parser = CommandLine.Parser;

var result = new Parser(ps =>
    {
        ps.AllowMultiInstance = true;
    })
    .ParseArguments<Options>(args);

if (result.Errors.Any())
{
    foreach (var resultError in result.Errors)
    {
        Console.WriteLine(resultError);
    }
    throw new AggregateException(result.Errors.Select(e => new ArgumentException(e.ToString())));
}

var options = result.Value;

var server = new Rewrite.Remote.Server.Server(options);

server.Listen().Wait();
