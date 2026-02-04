using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class RangeExpressionTests : RewriteTest
{
    [Test]
    public void BothBounds()
    {
        RewriteRun(
            CSharp(
                """
                a[1..3];
                """
            )
        );
    }

    [Test]
    public void StartOnly()
    {
        RewriteRun(
            CSharp(
                """
                a[1..];
                """
            )
        );
    }

    [Test]
    public void EndOnly()
    {
        RewriteRun(
            CSharp(
                """
                a[..3];
                """
            )
        );
    }

    [Test]
    public void FullRange()
    {
        RewriteRun(
            CSharp(
                """
                a[..];
                """
            )
        );
    }

    [Test]
    public void EndRelativeStart()
    {
        RewriteRun(
            CSharp(
                """
                a[^2..];
                """
            )
        );
    }

    [Test]
    public void EndRelativeEnd()
    {
        RewriteRun(
            CSharp(
                """
                a[..^1];
                """
            )
        );
    }

    [Test]
    public void BothEndRelative()
    {
        RewriteRun(
            CSharp(
                """
                a[^2..^1];
                """
            )
        );
    }

    [Test]
    public void StandaloneRangeExpression()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        Range r = 1..4;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void StandalonePartialRange()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        Range r = ..^1;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void WhitespacePreserved()
    {
        RewriteRun(
            CSharp(
                """
                a[ 1 .. 3 ];
                """
            )
        );
    }

    [Test]
    public void WhitespacePreservedPartial()
    {
        RewriteRun(
            CSharp(
                """
                a[ .. 3 ];
                """
            )
        );
    }

    [Test]
    public void RangeWithExpressions()
    {
        RewriteRun(
            CSharp(
                """
                a[(start + 1)..(end - 1)];
                """
            )
        );
    }

    [Test]
    public void RangeInMethodCall()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(string s)
                    {
                        var sub = s[1..^1];
                    }
                }
                """
            )
        );
    }
}
