using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class BlockTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void MultipleStatementBlock()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public override string ToString()
                    {
                        var s = "Foo";
                        return s;
                    }
                }
                """
            )
        );
    }
}
