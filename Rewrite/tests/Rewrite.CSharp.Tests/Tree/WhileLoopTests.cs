using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class WhileLoopTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void EmptyBody()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M()
                    {
                        while (true)
                        {
                        }
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void WithBreak()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M()
                    {
                        while (true)
                            break;
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void WithContinue()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M()
                    {
                        while (true)
                            continue;
                    }
                }
                "
            )
        );
    }
}
