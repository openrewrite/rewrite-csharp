
using System;
using System.IO;
using JetBrains.Annotations;
using Nuke.Common.CI.GitHubActions;

public static class Extensions
{
    public static string Append(this string original, [CanBeNull] string toAppend)
    {
        return $"{original}{toAppend}";
    }

    public static void SetVariable(this GitHubActions gitHubActions, string key, bool value) => SetVariable(gitHubActions, key, value.ToString().ToLower());
    public static void SetVariable(this GitHubActions gitHubActions, string key, string value)
    {
        var outputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT");
        if (string.IsNullOrEmpty(outputFile))
            throw new InvalidOperationException("GITHUB_OUTPUT environment variable is not set.");

        using var writer = new StreamWriter(outputFile, append: true);
        writer.WriteLine($"{key}={value}");
    }
}
