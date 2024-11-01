using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EnumDeclarationTests : RewriteTest
{
    [Fact]
    public void BasicEnum()
    {
        RewriteRun(
            CSharp(
                """
                public enum A
                {
                    a,
                    b
                }
                """
            )
        );
    }

    [Fact]
    public void EnumWithBaseType()
    {
        RewriteRun(
            CSharp(
                """
                [Test]
                public enum A : int
                {
                    a,
                    b
                }
                """
            )
        );
    }
}
