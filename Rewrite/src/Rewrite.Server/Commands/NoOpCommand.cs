using Spectre.Console.Cli;

namespace Rewrite.Server.Commands;

public class NoOpCommand<T> : Command<T> where T : CommandSettings
{
    public override int Execute(CommandContext context, T settings, CancellationToken cancellationToken) => 0;
}