using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EnumDeclarationTests : RewriteTest
{
    [Test]
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

    [Test]
    public void EmptyEnum()
    {
        RewriteRun(
            CSharp(
                """
                public enum A
                {

                }
                """
            )
        );
    }

    [Test]
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

    [Test]
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


    [Test]
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
