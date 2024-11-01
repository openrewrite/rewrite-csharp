using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class MemberAccessTests : RewriteTest
{
    [Fact]
    public void MultilineLinq()
    {
        var src = CSharp(
            """
            public class Foo
            {
                void Test()
                {
                 /*1*/   "blah".Skip()/*2*/
                        .ToList();
                }
            }
            """);
        RewriteRun(src);
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

    [Fact]
    public void SimpleFieldAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    int M()
                    {
                        var a = a.b.c;
                    }
                }
                """
            , s => s.AfterRecipe = c => Console.WriteLine(c))
        );
    }
}
