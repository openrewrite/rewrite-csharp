using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class ArrayTypeTests : RewriteTest
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    int [] _arr = null;
                }
                """
            )
        );
    }

    [Fact(Skip = "Still work in progress")]
    public void TwoDimensional()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    int [,] _arr = null;
                }
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
                public class Foo
                {
                    int[][] _arr = null;
                }
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
                public class Foo
                {
                    /*0*/ int /*1*/ [ /*2*/ ] /*3*/ [ /*4*/ ] /*5*/ _arr = null;
                }
                """
            )
        );
    }
}