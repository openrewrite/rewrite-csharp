using System.Collections;
using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.RoslynTestCases;

using static Assertions;

public class ParseTests : RewriteTest
{
    private readonly ITestOutputHelper _output;

    public ParseTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [ClassData(typeof(CSharpSyntaxFragments))]
    [Category("Roslyn")]
    public void ParseAndPrint(CSharpSyntaxFragment syntax)
    {
        RewriteRun(CSharp(syntax.Content));
    }
}
