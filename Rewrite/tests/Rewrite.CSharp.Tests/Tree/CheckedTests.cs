using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CheckedTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    private void CheckedStatement()
    {
        RewriteRun(
            CSharp(
                """
                checked
                {

                }
                """
            )
        );
    }

}
