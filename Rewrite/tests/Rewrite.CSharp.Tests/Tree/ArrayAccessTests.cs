using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ArrayAccessTests : RewriteTest
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    int M(int[] a)
                    {
                        return a[0];
                    }
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
                    int M(int[] a)
                    {
                        return /*0*/ a /*1*/ [ /*2*/ 0 /*3*/ ] /*4*/ ; /*5*/
                    }
                }
                """
            )
        );
    }
}
