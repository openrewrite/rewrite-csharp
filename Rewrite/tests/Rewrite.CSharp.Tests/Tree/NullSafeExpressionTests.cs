using Rewrite.Test.CSharp;
using Rewrite.Test;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteJava;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NullSafeExpressionTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void Space()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public object M(int? i)
                    {
                        return /*0*/ i /*1*/ ?. /*2*/ ToString() /*3*/ ;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void NestedMethodCall()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public object M(int? i)
                    {
                        return i.GetHashCode()?.ToString();
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void FieldAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    A a = new A(new B(new C()));
                    public object M()
                    {
                        a?.b?.c().ToString();
                    }
                }
                record C();
                record B(C c);
                record A(B b);
                """,
                a => a.AfterRecipe = c =>
                {
                    c.ToString();
                })
        );
    }

    [Fact]
    public void NullSafeArrayAccess()
    {
        var src = CSharp(
            """
            public class Foo
            {
                public object M()
                {
                    a?.b?[0]?.c
                }
            }
            """);
    }

    [Fact]
    public void SequentialFieldAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    Foo? foo_;
                    Foo? baz_;
                    Foo? bar_;
                    public object M()
                    {
                        return this.foo_?.baz_?.bar_;
                    }
                }
                """
            )
        );
    }


    [Fact]
    public void ArrayAccess()
    {
        RewriteRun(
            CSharp(
                """
                public static class A
                {
                    public static void Method(string p)
                    {
                         A.Method("hi");
                    }
                }

                """
            )
        );
    }

    [Fact]
    public void MultiLevelWithSpaces()
    {
        RewriteRun(
            CSharp(
            """
            /*1*/a/*2*/?/*3*/.
            b()/*4*/?/*5*/.
            c()/*6*/./*7*/
            d;

            """));
    }

    [Fact]
    public void MultiLevelWithSpaces2()
    {
        RewriteRun(
            CSharp(
                """
                /*1*/a/*2*/?/*3*/.b()/*4*/?/*5*/.c();
                """));
    }

    [Fact]
    public void FieldWithSpace()
    {
        RewriteRun(
            CSharp(
                """
                providerPlans.FirstOrDefault()?/*1*/.SeatMinimum ?? 0;
                """));
    }
}
