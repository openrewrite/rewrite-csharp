using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TupleTests : RewriteTest
{
    [Fact]
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
    [Fact]
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
        var statement = lst.Descendents().OfType<J.MethodDeclaration>().First().Body!.Statements.First();
        lst.ToString().ShouldBeSameAs(src.Before);
    }
}
