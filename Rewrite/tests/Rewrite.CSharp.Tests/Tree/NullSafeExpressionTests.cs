using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class NullSafeExpressionTests : RewriteTest
{
    [Fact(Skip = "NullSafeExpression parsing was disabled due to infinite recursion issue")]
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

    [Fact(Skip = "NullSafeExpression parsing was disabled due to infiti recursion issue")]
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

    [Fact(Skip = "NullSafeExpression parsing was disabled due to infiti recursion issue")]
    public void FieldAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    Foo? foo_;
                    public object M()
                    {
                        return this.foo_?.foo_;
                    }
                }
                """
            )
        );
    }

    [Fact(Skip = "NullSafeExpression parsing was disabled due to infiti recursion issue")]
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


    [Fact(Skip = "NullSafeExpression parsing was disabled due to infiti recursion issue")]
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
}
