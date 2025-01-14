using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class RefTypeTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void SimpleRefParameter()
    {
        RewriteRun(
            CSharp(
                """
                void Method(ref int x)
                {
                    x = 42;
                }
                """
            )
        );
    }

    [Fact]
    public void RefReturnType()
    {
        RewriteRun(
            CSharp(
                """
                ref int Method(ref int x)
                {
                    return ref x;
                }
                """
            )
        );
    }

    [Fact]
    public void RefLocalVariable()
    {
        RewriteRun(
            CSharp(
                """
                void Method()
                {
                    int value = 10;
                    ref int refValue = ref value;
                }
                """
            )
        );
    }

    [Fact]
    public void RefReadonlyParameter()
    {
        RewriteRun(
            CSharp(
                """
                void Method(ref readonly int x)
                {
                    int y = x;
                }
                """
            )
        );
    }

    [Fact]
    public void RefReadonlyReturn()
    {
        RewriteRun(
            CSharp(
                """
                ref readonly int Method(in int x)
                {
                    return ref x;
                }
                """
            )
        );
    }

    [Fact]
    public void RefStructType()
    {
        RewriteRun(
            CSharp(
                """
                ref struct RefStruct
                {
                    public int Value;
                }
                """
            )
        );
    }

    [Fact]
    public void MultipleRefParameters()
    {
        RewriteRun(
            CSharp(
                """
                void Method(ref int x, ref string y, ref double z)
                {
                    x = 42;
                    y = "modified";
                    z = 3.14;
                }
                """
            )
        );
    }

    [Fact]
    public void RefPropertyReturn()
    {
        RewriteRun(
            CSharp(
                """
                class Container
                {
                    private int _value;
                    public ref int Value => ref _value;
                }
                """
            )
        );
    }

    [Fact]
    public void RefExtensionMethod()
    {
        RewriteRun(
            CSharp(
                """
                static class Extensions
                {
                    public static void Modify(this ref int value)
                    {
                        value *= 2;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void RefFieldInRefStruct()
    {
        RewriteRun(
            CSharp(
                """
                ref struct Container
                {
                    private ref int _value;

                    public Container(ref int value)
                    {
                        _value = ref value;
                    }
                }
                """
            )
        );
    }
}
