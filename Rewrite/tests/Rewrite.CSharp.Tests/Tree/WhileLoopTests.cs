using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class WhileLoopTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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
