namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class GotoStatementTests : RewriteTest
{
    [Test]
    public void SimpleGoto()
    {
        RewriteRun(
            CSharp(
                """
                void Method()
                {
                    goto Label;
                    Label:
                    return;
                }
                """
            )
        );
    }

    [Test]
    public void GotoInLoop()
    {
        RewriteRun(
            CSharp(
                """
                void Method()
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (i == 5)
                            goto ExitLoop;
                    }
                    ExitLoop:
                    return;
                }
                """
            )
        );
    }

    [Test]
    public void GotoWithSwitch()
    {
        RewriteRun(
            CSharp(
                """
                void Method(int x)
                {
                    switch (x)
                    {
                        case 1:
                            goto case 2;
                        case 2:
                            return;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void GotoInNestedBlocks()
    {
        RewriteRun(
            CSharp(
                """
                void Method()
                {
                    {
                        if (true)
                            goto OuterLabel;
                    }
                    OuterLabel:
                    return;
                }
                """
            )
        );
    }

    [Test]
    public void GotoWithTryFinally()
    {
        RewriteRun(
            CSharp(
                """
                void Method()
                {
                    try
                    {
                        goto Finish;
                    }
                    finally
                    {
                        // Cleanup code
                    }
                    Finish:
                    return;
                }
                """
            )
        );
    }

    [Test]
    public void MultipleGotoSameLabel()
    {
        RewriteRun(
            CSharp(
                """
                void Method(int x)
                {
                    if (x < 0)
                        goto Exit;
                    if (x > 100)
                        goto Exit;
                    Exit:
                    return;
                }
                """
            )
        );
    }

    [Test]
    public void GotoDefaultCase()
    {
        RewriteRun(
            CSharp(
                """
                void Method(int x)
                {
                    switch (x)
                    {
                        case 1:
                            goto default;
                        default:
                            return;
                    }
                }
                """
            )
        );
    }
}
