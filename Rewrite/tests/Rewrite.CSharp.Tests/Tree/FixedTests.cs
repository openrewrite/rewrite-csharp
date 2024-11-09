using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class FixedTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    private void FixedStatement()
    {
        RewriteRun(
            CSharp(
                """
                fixed(int* p1 = 0)
                {

                }
                """
            )
        );
    }

}
