using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Nuke.Common.IO;
// using NMica.Utils.IO;
using Rewrite.MSBuild;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.Test.CSharp;
using Rewrite.Tests;

namespace Rewrite.CSharp.Tests.Solutions;

using static Assertions;

public class SolutionTests : RewriteTest
{


    [Test]
    [Explicit]
    public void PlayTest()
    {
        var actual = """
                     hello
                     there
                     """;
        var expected = "hello there";
        actual.ShouldBeSameAs(expected);
    }

    [Test]
    [Explicit]
    public void RewriteTest()
    {
        this.RewriteRun(CSharp("""
                     var newElements = combinatorSelector.Transform!(element);
                     """));

    }

    [Test]
    [Explicit]
    public async Task ParseSingleFile()
    {
        var src = await File.ReadAllTextAsync(@"C:\projects\openrewrite\rewrite-csharp\Rewrite\tests\fixtures\Bogus\Source\Bogus.Tests\SchemaTests\LocaleSchemaTests.cs");
        RewriteRun(CSharp(src));
    }

    [Test]
    [Explicit]
    public async Task UnknownSyntaxReport()
    {
        // Type.GetType("Nuke.Common.ParameterService")
        // AbsolutePath path = NukeBuild.RootDirectory;
        var sourceFiles = (await Task.WhenAll(Fixtures().Select(async x =>
            {
                var (solutionOrProjectPath, root) = x();
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
                        .SelectMany(projectPath => solutionParser.ParseProjectSources(solution, projectPath, root, executionContext))
                        .ToList();
                }
                else
                {
                    var projectParser = new ProjectParser(solutionOrProjectPath, root);
                    sourceFiles = projectParser.ParseSourceFiles();
                }

                return sourceFiles.Select(x => (ProjectPath: root, SourceTree: x)).ToList();
            })))
            .SelectMany(x => x)
            .ToList();

        // var items = sourceFiles.Where(x => x.SourceTree is null);

        var brokenIdp = sourceFiles
            .Select(x =>
            {
                var file = x.ProjectPath / x.SourceTree.SourcePath;
                var before = File.ReadAllText(file);
                var after = x.SourceTree.ToString();
                return (file, before, after, x.SourceTree);
            })
            .Where(x => x.before != x.after)
            .Take(20)
            .ToList();

        var report = sourceFiles.Select(x => x.SourceTree)
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
        _output.WriteLine("=============");
        _output.WriteLine($"Broken {brokenIdp.Count} / {sourceFiles.Count} Total ({brokenIdp.Count * 100 / sourceFiles.Count }%).");
        foreach (var (file, before,after, tree) in brokenIdp.Where(x => x.after != null))
        {

            _output.WriteLine(file);
            if (tree is ParseError)
            {
                _output.WriteLine(after);
            }
            else
            {
                _output.WriteLine(before.GetDifferences(after!));
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(Fixtures))]
    [Explicit]
    public async Task ParseSolution(AbsolutePath solutionPath, AbsolutePath root)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1000)).Token;
        var solutionParser = new SolutionParser();

        var solution = await solutionParser.LoadSolutionAsync(solutionPath, cancellationToken);
        var executionContext = new InMemoryExecutionContext(exception => _output.WriteLine(exception.ToString()));
        // var root = solutionPath.Parent;
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

    public IEnumerable<Func<(AbsolutePath SolutionOrProject, AbsolutePath RootDir)>> Fixtures()
    {
        var fixturesDirectory = DirectoryHelper.RepositoryRoot / "Rewrite" / "tests" / "fixtures";
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
                yield return () => (slnOrProj, root);
            }
        }
    }
}

