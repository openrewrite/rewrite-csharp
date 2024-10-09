using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ArrayTests : RewriteTest
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new int[3];
                }
                """
            )
        );
    }

    [Fact]
    public void MultiDimensionalSpace()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = /*0*/ new /*1*/ int /*2*/ [ /*3*/ 1 /*4*/ , /*5*/ 1 /*6*/ ] /*7*/ [ /*8*/ ] /*9*/
                        { /*10*/ { /*11*/ { /*12*/ 1 /*13*/ , /*14*/ } /*15*/ } /*16*/ } /*17*/ ; /*18*/
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
                public class Foo
                {
                    object arr = new int[3] { 1, 2, 3 };
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
                    object arr = new int[3][];
                }
                """
            )
        );
    }
}
