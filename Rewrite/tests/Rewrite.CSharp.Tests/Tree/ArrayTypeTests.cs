using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ArrayTypeTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                int [] _arr;
                """
            )
        );
    }

    [Fact]
    public void TwoDimensional()
    {
        RewriteRun(
            CSharp(
                """
                int [,] _arr = new int [0,0];
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
                int[][] _arr;
                """
            )
        );
    }

    [Fact]
    public void Space()
    {
        RewriteRun(
            CSharp(
                """
                /*0*/ int /*1*/ [ /*2*/ ] /*3*/ [ /*4*/ ] /*5*/ _arr = null;
                """
            )
        );
    }
}
