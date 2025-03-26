using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ArrayTypeTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
