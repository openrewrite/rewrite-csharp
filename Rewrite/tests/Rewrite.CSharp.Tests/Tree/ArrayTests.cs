using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;


public class ArrayTests : RewriteTest
{
    [Fact]
    public void OneDimensional()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new int[3];
                }
                """
            )
        );
    }

    [Fact]
    public void ImplicitArray()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new [] { 1 };
                }
                """
            )
        );
    }

    [Fact]
    public void Initializer()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new int[3] { 1, 2, 3 };
                }
                """
            )
        );
    }

    [Fact]
    public void Jagged()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    object arr = new int[3][];
                }
                """
            )
        );
    }
}
