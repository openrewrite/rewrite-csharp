using Rewrite.Test;
using Rewrite.Test.CSharp;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class YieldTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void YieldReturn()
    {
        RewriteRun(
            CSharp(
                @"
                IEnumerable<int> M()
                {
                    yield return 1;
                }
                "
            )
        );
    }
    [Fact]
    public void YieldBreak()
    {
        RewriteRun(
            CSharp(
                @"
                IEnumerable<int> M()
                {
                    yield break;
                }
                "
            )
        );
    }
}
