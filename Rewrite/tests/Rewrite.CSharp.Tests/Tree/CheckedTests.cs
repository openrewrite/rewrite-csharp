using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CheckedTests : RewriteTest
{
    [Test]`n    public void CheckedStatement()
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
    [Test]`n    public void UnCheckedStatement()
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

    [Test]`n    public void CheckedExpression()
    {
        RewriteRun(
            CSharp(
                """
                var a = checked(1);
                """
            )
        );
    }
    [Test]`n    public void UnCheckedExpression()
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
