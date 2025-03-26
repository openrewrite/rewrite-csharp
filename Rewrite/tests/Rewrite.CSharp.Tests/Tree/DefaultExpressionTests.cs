using Rewrite.Test;
using Rewrite.Test.CSharp;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DefaultExpressionTests : RewriteTest
{
    [Test]
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
    [Test]
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
