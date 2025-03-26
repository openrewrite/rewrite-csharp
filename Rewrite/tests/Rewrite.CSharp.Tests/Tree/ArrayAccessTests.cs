using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ArrayAccessTests : RewriteTest
{
    [Test]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                int[] a;
                return a[0];
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
