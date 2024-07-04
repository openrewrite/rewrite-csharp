using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class InstanceOfTests : RewriteTest
{

    [Fact]
    private void Is()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    bool M(object? o)
                    {
                        return o is int;
                    }
                }
                """
            )
        );
    }

    [Fact]
    private void IsVariablePattern()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    bool M(object? o)
                    {
                        return o is int i;
                    }
                }
                """
            )
        );
    }

    [Fact]
    private void IsDiscardPattern()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    bool M(object? o)
                    {
                        return o is int _;
                    }
                }
                """
            )
        );
    }
}