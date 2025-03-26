using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class RefTypeTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
