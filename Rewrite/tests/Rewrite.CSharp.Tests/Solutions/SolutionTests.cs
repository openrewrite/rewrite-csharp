using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using FluentAssertions;
using Nuke.Common;
using Nuke.Common.IO;
using Rewrite.Core;
using Rewrite.MSBuild;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Test;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace Rewrite.CSharp.Tests.Solutions;

using static Assertions;

[Collection("C# remoting")]
public class SolutionTests// : RewriteTest
{
    private readonly ITestOutputHelper _output;

    public SolutionTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory(Skip = "Run manually")]
    [MemberData(nameof(Fixtures))]
    public async Task ParseSolution(AbsolutePath solutionPath)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1000)).Token;
        var solutionParser = new SolutionParser();

        var solution = await solutionParser.LoadSolutionAsync(solutionPath, cancellationToken);
        var executionContext = new InMemoryExecutionContext(exception => _output.WriteLine(exception.ToString()));
        var root = solutionPath.Parent;
        var projectPaths = solution.Projects.Select(x => x.FilePath).ToList();
        foreach (var projectPath in projectPaths.Where(x => x != null).Cast<string>())
        {
            var sources = solutionParser.ParseProjectSources(solution, projectPath, root, executionContext).ToList();
            foreach (var source in sources)
            {

                var filePath = Path.Combine(root, source.SourcePath);
                var parseError = source.Descendents().OfType<ParseError>().FirstOrDefault();
                if (parseError != null)
                {
                    var parseExceptionResult = (ParseExceptionResult)parseError.Markers.First();
                    _output.WriteLine("=======Parser Error==========");
                    _output.WriteLine(parseExceptionResult.Message);
                    // parseError.
                    Debugger.Break();
                }
                var originalSourceText = await File.ReadAllTextAsync(filePath, cancellationToken);
                try
                {
                    var printedSourceText = source.PrintAll(executionContext);
                    try
                    {
                        printedSourceText.Should().Be(originalSourceText);
                    }
                    catch (Exception e)
                    {
                        _output.WriteLine($"----{filePath}---");
                        var message = Regex.Replace(e.Message, @"^\s+at .+", "", RegexOptions.Multiline).Trim();
                        _output.WriteLine(message);
                    }
                }
                catch (Exception e)
                {

                    var exception = ExceptionDispatchInfo.SetRemoteStackTrace(
                        new Exception($"Failure parsing {source.SourcePath}\n{e.Message}"), e.StackTrace!);
                    ExceptionDispatchInfo.Throw(exception);
                }
            }
        }
    }

    public static IEnumerable<object[]> Fixtures
    {
        get
        {
            var fixturesDirectory = NukeBuild.RootDirectory / "Rewrite" / "tests" / "fixtures";
            if (!fixturesDirectory.Exists())
                return Array.Empty<object[]>();
            return fixturesDirectory
                .GetDirectories()
                .SelectMany(x => x.GlobFiles("*.sln"))
                .Select(x => new object[] { x });
        }
    }
}
