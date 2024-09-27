using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class MemberAccessTests : RewriteTest
{
    [Fact]
    public void MultilineLinq()
    {

        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    void Test()
                    {
                        "blah".Skip(1)
                            .Take(1)
                            .ToList();
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
