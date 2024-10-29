using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Rewrite.MSBuild;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteJson.Tree;
using Rewrite.Test.CSharp;

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

    [Fact]
    // [MemberData(nameof(Fixtures))]
    [Exploratory]
    // public async Task UnknownSyntaxReport(AbsolutePath solutionOrProjectPath, AbsolutePath root)
    public async Task UnknownSyntaxReport()
    {
        var fixtures = Fixtures.Select(x => (solutionOrProjectPath: (AbsolutePath)x[0], root: (AbsolutePath)x[1]));

        var sourceFiles = (await Task.WhenAll(fixtures.Select(async x =>
        {
            var (solutionOrProjectPath, root) = x;
            _output.WriteLine(solutionOrProjectPath);
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1000)).Token;
            var solutionParser = new SolutionParser();
            var executionContext = new InMemoryExecutionContext(exception => _output.WriteLine(exception.ToString()));
            IEnumerable<SourceFile> sourceFiles;
            if (solutionOrProjectPath.Extension == ".sln")
            {
                var solution = await solutionParser.LoadSolutionAsync(solutionOrProjectPath, cancellationToken);

                var projectPaths = solution.Projects.Select(x => x.FilePath!).ToList();
                sourceFiles = projectPaths
                    .SelectMany(projectPath => solutionParser.ParseProjectSources(solution, projectPath, root, executionContext));
            }
            else
            {
                var projectParser = new ProjectParser(solutionOrProjectPath, root);
                sourceFiles = projectParser.ParseSourceFiles();
            }

            return sourceFiles;
        })))
            .SelectMany(x => x);



        var report = sourceFiles
            .SelectMany(x => x.Descendents().OfType<J.Unknown>())
            .Select(x => x.UnknownSource.Markers.OfType<ParseExceptionResult>().Select(y => y.TreeType).First())
            .GroupBy(x => x)
            .Select(x => new
            {
                Kind = x.Key,
                Count = x.Count(),
            })
            .OrderByDescending(x => x.Count)
            .ToList();
        foreach (var unknownSyntax in report)
        {
            _output.WriteLine($"{unknownSyntax.Kind}: {unknownSyntax.Count}");
        }

    }

    [Theory]
    [MemberData(nameof(Fixtures))]
    [Exploratory]
    public async Task ParseSolution(AbsolutePath solutionPath)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1000)).Token;
        var solutionParser = new SolutionParser();

        var solution = await solutionParser.LoadSolutionAsync(solutionPath, cancellationToken);
        var executionContext = new InMemoryExecutionContext(exception => _output.WriteLine(exception.ToString()));
        var root = solutionPath.Parent;
        var projectPaths = solution.Projects.Select(x => x.FilePath).ToList();
        List<J.Unknown> jUnknowns = new();
        foreach (var projectPath in projectPaths.Where(x => x != null).Cast<string>())
        {
            var sources = solutionParser.ParseProjectSources(solution, projectPath, root, executionContext);
            foreach (var source in sources)
            {
                jUnknowns.AddRange(source.Descendents().OfType<J.Unknown>().ToList());
                var filePath = Path.Combine(root, source.SourcePath);
                var parseError = source.Descendents().OfType<ParseError>().FirstOrDefault();
                if (parseError != null)
                {
                    var parseExceptionResult = (ParseExceptionResult)parseError.Markers.First();
                    _output.WriteLine("=======Parser Error==========");
                    _output.WriteLine(parseExceptionResult.Message);
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
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
    }

    public static IEnumerable<object[]> Fixtures
    {
        get
        {
            var fixturesDirectory = NukeBuild.RootDirectory / "Rewrite" / "tests" / "fixtures";
            if (!fixturesDirectory.Exists())
                yield break;
            foreach (var fixture in fixturesDirectory.GetDirectories())
            {
                var result = fixture.GlobFiles("**/*.sln").ToList();
                if (result.Count == 0)
                {
                    result = fixture.GlobFiles("**/*.csproj").ToList();
                }

                foreach (var slnOrProj in result)
                {
                    var root = fixturesDirectory / fixturesDirectory.GetUnixRelativePathTo(slnOrProj).ToString().Split("/")[0];
                    yield return new[] { slnOrProj, root  };
                }
            }
            // return fixturesDirectory
            //     .GetDirectories()
            //     .SelectMany(x => x.GlobFiles("*.sln"))
            //     .Select(x => new object[] { x, (AbsolutePath)fixturesDirectory.GetUnixRelativePathTo(x).ToString().Split("/")[0] });
        }
    }
}
