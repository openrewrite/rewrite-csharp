using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Socolin.ANSITerminalColor;

namespace Rewrite.CSharp.Tests.RoslynTestCases;

public class ParseTests : RewriteTest
{
    private readonly CSharpParser _parser = new CSharpParser.Builder().Build();


    [Test]
    [Explicit]
    public void TestReport()
    {
        var badTestCases = new List<(SourceTestCase, List<Diagnostic>)>();
        var testCases = CSharpSyntaxFragments.GetData().ToList();
        List<TestResult> report = new();
        foreach (var testCase in testCases)
        {
            var src = this.AsValidCompilationRoot(testCase.SourceText);
            var roslynDiagnostics = CSharpSyntaxTree.ParseText(src).GetDiagnostics().ToList();
            if (roslynDiagnostics.Any())
            {
                badTestCases.Add((testCase,roslynDiagnostics));
                continue;
            }

            var lst = _parser.Parse(src);

            if (lst is ParseError)
            {
                report.Add(testCase.Fail("LstParseError"));
                continue;
            }

            var lstPrint = lst.ToString()!;

            string StripWhiteSpace(string s) => Regex.Replace(s, @"[\r\n]", "");
            var equalBeforeTrim = lstPrint == src;
            var equalAfterTrim = StripWhiteSpace(lstPrint) == StripWhiteSpace(src);
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

        _output.WriteLine("The following input testcases were skipped because they are not a fully valid syntax");
        foreach (var (testCase, diagnostics) in badTestCases)
        {
            _output.WriteLine($"\"{testCase.Name}\",");
            _output.WriteLine(diagnostics.Select(x => x.GetMessage()).FirstOrDefault());
        }
    }

    private string AsValidCompilationRoot(string src)
    {
        var expectsSemicolon = SyntaxFactory.ParseCompilationUnit(src).GetDiagnostics().Any(x => x.Id == "CS1002");
        src += ";";
        return src;
    }


    [Test]
    [MethodDataSource<CSharpSyntaxFragments>(nameof(CSharpSyntaxFragments.GetData))]
    [Explicit]
    public void ParseAndPrint(SourceTestCase syntax)
    {
        var src = AsValidCompilationRoot(syntax.SourceText);
        _output.WriteLine(src);
        var lst = _parser.Parse(src);
        lst.Should().NotBeOfType<ParseError>();

        var lstPrint = lst.ToString()!;
        // lstPrint.Should().Be(syntax.SourceText);
        lstPrint.ShouldBeSameAs(src);

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

