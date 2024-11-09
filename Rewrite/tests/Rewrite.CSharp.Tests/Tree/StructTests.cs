using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class StructTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void Empty()
    {
        RewriteRun(
            CSharp(
                """
                public struct Empty
                {
                }
                """
            )
        );
    }
}
