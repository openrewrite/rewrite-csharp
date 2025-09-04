using Nuke.Common.Utilities.Collections;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Rewrite.MSBuild.Tests;

public static class CommandExtensions
{

    public static async Task<int> ExecuteAsync<TSettings>(this AsyncCommand<TSettings> command, TSettings settings) where TSettings : CommandSettings
    {
        command.Validate(CreateContext(), settings);
        return await command.ExecuteAsync(CreateContext(), settings);
    }

    private static CommandContext CreateContext()
    {
       return new CommandContext([], new RemainingArguments(), "test", null);
    }
    private sealed class RemainingArguments : IRemainingArguments
    {
        public IReadOnlyList<string> Raw { get; } = [];
        public ILookup<string, string?> Parsed { get; } = new LookupTable<string, string?>();

    }
}