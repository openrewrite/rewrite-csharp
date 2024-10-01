using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class MemberAccessTests : RewriteTest
{
    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void MultilineLinq()
    {
        var src = CSharp(
            """
            public class Foo
            {
                void Test()
                {
                    "blah".Skip()
                        .ToList();
                }
            }
            """);
        var cu = src.Parse().First();
        var result = cu.Print();
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
}
