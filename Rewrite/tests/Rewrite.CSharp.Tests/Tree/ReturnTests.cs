using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ReturnTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public int M()
                    {
                        return 1;
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void Void()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M()
                    {
                        return;
                    }
                }
                "
            )
        );
    }
}
