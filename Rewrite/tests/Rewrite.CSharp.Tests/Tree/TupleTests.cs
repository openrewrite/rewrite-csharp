using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TupleTests : RewriteTest
{
    [Test]
    public void TupleDeconstruction()
    {
        var src = CSharp(
            """
            public class T
            {
                void M()
                {
                    var (x, y) = (1,2);
                }
            }
            """
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }
    [Test]
    public void TupleExpression()
    {
        var src = CSharp(
            """
            public class T
            {
                void M()
                {
                    (var x, var y) = (1,2);
                }
            }
            """
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void NamedTupleType()
    {
        var src = CSharp(
            """
            public class T
            {
                void M()
                {
                    (int x, int y) myTuple;
                }
            }
            """
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void BasicTupleType()
    {
        var src = CSharp(
            """
            public class T
            {
                void M()
                {
                    (int, int) myTuple;
                }
            }
            """
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void TupleWithNullables()
    {
        RewriteRun(CSharp(
            """
            public class T
            {
                void M()
                {
                    (int? x, int y) myTuple;
                }
            }
            """
        ));
    }
}
