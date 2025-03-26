using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;


public class ArrayTests : RewriteTest
{
    [Test]
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
    [Test]
    public void MultiDimensional()
    {
        RewriteRun(
            CSharp(
                """
                int[,] a = new int[1,1];
                return a[0, 0];
                """
            )
        );
    }

    [Test]
    public void Jagged()
    {
        RewriteRun(
            CSharp(
                """
                int[][] a = new int[3][0];
                a[0][0];
                """
            )
        );
    }

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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


}
