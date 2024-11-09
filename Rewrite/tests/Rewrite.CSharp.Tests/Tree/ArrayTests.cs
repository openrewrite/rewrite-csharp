using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;


public class ArrayTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                new int[3];
                }
                """
            )
        );
    }

    [Fact]
    public void RangeExpression()
    {
        RewriteRun(
            CSharp(
                """
                a[1..3];
                """
            )
        );
    }

    [Fact]
    public void ImplicitArray()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new [] { 1 };
                }
                """
            )
        );
    }

    [Fact]
    public void Initializer()
    {
        RewriteRun(
            CSharp(
                """
                new int[3] { 1, 2, 3 };
                }
                """
            )
        );
    }

    [Fact]
    public void InitializerWithTrailingComma()
    {
        RewriteRun(
            CSharp(
                """
                new [] { 1, };
                """
            )
        );
    }

    [Fact]
    public void ArrayWithEmptyInitializer()
    {
        RewriteRun(
            CSharp(
                """
                var a = new int[] { /*1*/ };
                """
            )
        );

    }

    [Fact]
    public void Jagged()
    {
        RewriteRun(
            CSharp(
                """
                new int[3][];
                """
            )
        );
    }
}
