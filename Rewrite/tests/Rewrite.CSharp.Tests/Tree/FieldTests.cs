using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class FieldTests : RewriteTest
{
    [Fact]
    void Simple()
    {
        RewriteRun(
            CSharp(
                """
                public class B
                {
                    public B Field = new B();
                    B B = new B()/*1*/ .     Field      . /*asda*/  Field;
                }
                """
            )
        );
    }

    [Fact]
    void Modifiers()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    const int I1 = 0;
                    readonly int _i2 = 0;
                    volatile int _i3 = 0;
                    static int _i4 = 0;
                }
                """
            )
        );
    }

    [Fact]
    void Visibility()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    public int _i1;
                    private int _i2;
                    protected int _i3;
                    internal int _i4;
                    protected internal int _i5;
                    private protected int _i6;
                }
                """
            )
        );
    }

    [Fact]
    void ComplexNestedAccess()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    public class TestAction {
                        public class InnerTestAction<T1, T2>
                        {
                        }
                    }
                    Test.TestAction.InnerTestAction<string, string>? square  =  null;
                }
                """
            )
        );
    }
}
