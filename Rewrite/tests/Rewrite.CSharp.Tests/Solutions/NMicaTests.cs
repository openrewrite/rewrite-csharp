using FluentAssertions;
using Rewrite.Core;
using Rewrite.MSBuild;
using Rewrite.RewriteCSharp.Test;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;
using Xunit.Abstractions;

namespace Rewrite.CSharp.Tests.Solutions;

using static Assertions;

[Collection("C# remoting")]
public class NMicaTests : RewriteTest
{
    private readonly RemotingFixture _fixture;
    private readonly ITestOutputHelper _output;

    public NMicaTests(RemotingFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task ParseSolution()
    {
        var cancellationToken = new CancellationTokenSource(30_000).Token;
        var solutionParser = new SolutionParser();
        var solution = await solutionParser.LoadSolutionAsync(@"C:\projects\nmica\NMica.Utils\NMica.Utils.sln", cancellationToken);
        var executionContext = new InMemoryExecutionContext(exception => _output.WriteLine(exception.ToString()));
        var root = @"C:\projects\nmica\NMica.Utils";
        var sources = solutionParser.ParseProjectSources(solution, @"C:\projects\nmica\NMica.Utils\src\NMica.Utils\NMica.Utils.csproj", root, executionContext)
            .ToList();

        sources.Should().NotContain(x => x is ParseError);
        foreach (var source in sources)
        {
            var outputSrc = source.PrintAll();
        }
    }

    [Fact]
    public void AssemblyAssertions()
    {
        RewriteRun(
            CSharp(
                """
                using System.Reflection;

                namespace NMica.Utils
                {
                    public static class AssemblyExtensions
                    {
                        public static string GetInformationalText(this Assembly assembly)
                        {
                            return $"version {assembly.GetVersionText()} ({EnvironmentInfo.Platform},{EnvironmentInfo.Framework})";
                        }

                        public static string GetVersionText(this Assembly assembly)
                        {
                            var informationalVersion = assembly.GetAssemblyInformationalVersion();
                            var plusIndex = informationalVersion.IndexOf(value: '+');
                            return plusIndex == -1 ? "LOCAL" : informationalVersion.Substring(startIndex: 0, length: plusIndex);
                        }

                        private static string GetAssemblyInformationalVersion(this Assembly assembly)
                        {
                            return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                        }
                    }
                }

                """
            ));
    }
}
