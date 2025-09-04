using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CheckedTests : RewriteTest
{
    [Test]
    public void CheckedStatement()
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
    public void UnCheckedStatement()
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
    public void CheckedExpression()
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
    public void UnCheckedExpression()
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
