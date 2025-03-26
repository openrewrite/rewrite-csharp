using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CheckedTests : RewriteTest
{
    [Test]
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
    [Test]
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

    [Test]
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
    [Test]
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
