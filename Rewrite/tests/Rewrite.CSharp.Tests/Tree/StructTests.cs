using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class StructTests : RewriteTest
{
    [Test]
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
