using Rewrite.Test;
using Rewrite.Test.CSharp;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DefaultExpressionTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void DefaultLiteral()
    {
        RewriteRun(
            CSharp(
                @"
                int i = default;
                "
            )
        );
    }
    [Fact]
    public void DefaultOperator()
    {
        RewriteRun(
            CSharp(
                @"
                int i = default (  int?   );
                "
            )
        );
    }
}
