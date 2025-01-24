using Microsoft.CodeAnalysis;
using Rewrite.Remote.Server;
using Parser = CommandLine.Parser;

namespace Rewrite.Server;

public class Program
{
    public static void Main(string[] args)
    {
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

        var server = new Remote.Server.Server(options);

        server.Listen().Wait();
    }
}
