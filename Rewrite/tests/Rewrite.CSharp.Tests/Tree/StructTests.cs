using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class StructTests : RewriteTest
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