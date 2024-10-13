using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rewrite.Test.CSharp;
using Rewrite.Test;
using Socolin.ANSITerminalColor;

namespace Rewrite.CSharp.Tests.RoslynTestCases;

using static Assertions;

public class ParseTests : RewriteTest
{
    private readonly ITestOutputHelper _output;
    private readonly CSharpParser _parser;

    public ParseTests(ITestOutputHelper output)
    {
        _output = output;
        _parser = new CSharpParser.Builder().Build();
    }

    [Theory]
    [Exploratory]
    [InlineData("abc\n123","abc123")]
    [InlineData("abc123","abc134")]
    [InlineData("""
                one
                two
                three
                four
                NOT five
                """,
                """
                one
                two
                four
                five
                six
                """)]
    public void Delta(string before, string after)
    {
        after.ShouldBeSameAs(before);
    }

    [Fact]
    [Exploratory]
    public void TestReport()
    {
        var badTestCases = new List<(SourceTestCase, List<Diagnostic>)>();
        var testCases = new CSharpSyntaxFragments().Select(x => (SourceTestCase)x[0]).ToList();
        List<TestResult> report = new();
        foreach (var testCase in testCases)
        {
            var roslynDiagnostics = CSharpSyntaxTree.ParseText(testCase.SourceText).GetDiagnostics().ToList();
            if (roslynDiagnostics.Any())
            {
                badTestCases.Add((testCase,roslynDiagnostics));
                // report.Add(testCase.Fail("Original.Parse.RoslynIssues"));
                continue;
            }

            var lst = _parser.Parse(testCase.SourceText);

            if (lst is ParseError)
            {
                report.Add(testCase.Fail("LstParseError"));
                continue;
            }

            var lstPrint = lst.ToString()!;

            string StripWhiteSpace(string s) => Regex.Replace(s, @"[\r\n]", "");
            var equalBeforeTrim = lstPrint == testCase.SourceText;
            var equalAfterTrim = StripWhiteSpace(lstPrint) == StripWhiteSpace(testCase.SourceText);
            if (!equalBeforeTrim && equalAfterTrim)
            {
                report.Add(testCase.Fail("DifferentWhitespace"));
            }
            else if(!equalAfterTrim)
            {
                var roslynLstReParse = CSharpSyntaxTree.ParseText(lstPrint);
                if (roslynLstReParse.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).Any())
                {
                    report.Add(testCase.Fail("DifferentUncompilable"));
                }
                else
                {
                    report.Add(testCase.Fail("DifferentCompilable"));
                }


            }
            else
            {
                report.Add(testCase.Pass());
            }
        }

        _output.WriteLine(AnsiColor.ColorizeText($"Total: {report.Count}", AnsiColor.Foreground(Terminal256ColorCodes.Red1C196)));
        _output.WriteLine($"  Successful: {report.Count(x => x.Success)}");
        var failed = report.Where(x => !x.Success).ToList();
        _output.WriteLine($"  Failed: {failed.Count}");
        foreach (var (failureReason, count) in failed
                     .GroupBy(x => x.FailureReason)
                     .Select(x => (Reason: x.Key, count: x.Count()))
                     .OrderBy(x => x.Reason))
        {
            _output.WriteLine($"    {failureReason}: {count}");
        }

        foreach (var (testCase, diagnostics) in badTestCases)
        {
            _output.WriteLine($"\"{testCase.Name}\",");
            // _output.WriteLine(testCase.SourceText);
            // _output.WriteLine(string.Join("\n", diagnostics.Select(x => x.ToString())));
        }
    }




    [Theory]
    [ClassData(typeof(CSharpSyntaxFragments))]
    [Category("Roslyn")]
    public void ParseAndPrint(SourceTestCase syntax)
    {
        var lst = _parser.Parse(syntax.SourceText);
        lst.Should().NotBeOfType<ParseError>();

        var lstPrint = lst.ToString()!;
        // lstPrint.Should().Be(syntax.SourceText);
        lstPrint.ShouldBeSameAs(syntax.SourceText);

    }
}

public class TestResult
{
    public TestResult(SourceTestCase testCase, string? failureReason = null)
    {
        TestCase = testCase.Name;
        FailureReason = failureReason;
    }

    public bool Success => FailureReason == null;
    public string TestCase { get; }
    public string? FailureReason { get; set; }
}

public static class Extensions
{
    public static TestResult Pass(this SourceTestCase testCase) => new TestResult(testCase);
    public static TestResult Fail(this SourceTestCase testCase, string reason) => new TestResult(testCase, reason);
}

