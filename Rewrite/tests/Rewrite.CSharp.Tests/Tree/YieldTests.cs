using Rewrite.Test;
using Rewrite.Test.CSharp;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class YieldTests : RewriteTest
{
    [Test]
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
    [Test]
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
