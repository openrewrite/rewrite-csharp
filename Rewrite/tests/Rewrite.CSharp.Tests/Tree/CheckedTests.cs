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
    [Fact]
    private void UnCheckedStatement()
    {
        RewriteRun(
            CSharp(
                """
                unchecked
                {

                }
                """
            )
        );
    }

    [Fact]
    private void CheckedExpression()
    {
        RewriteRun(
            CSharp(
                """
                var a = checked(1);
                """
            )
        );
    }
    [Fact]
    private void UnCheckedExpression()
    {
        RewriteRun(
            CSharp(
                """
                var a = unchecked(1);
                """
            )
        );
    }
}
