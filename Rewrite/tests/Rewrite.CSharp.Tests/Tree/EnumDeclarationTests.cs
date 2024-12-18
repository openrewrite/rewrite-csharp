using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EnumDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
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

    [Fact]
    public void EnumWithValues()
    {
        RewriteRun(
            CSharp(
                """
                [Test]
                public enum A
                {
                    a = 1,
                    b
                }
                """
            )
        );
    }


    [Fact]
    public void EnumWithAttributes()
    {
        RewriteRun(
            CSharp(
                """
                [Test]
                public enum A
                {
                    [Value(1)]
                    a,
                    [Value(2)]
                    b
                }
                """
            )
        );
    }
}
