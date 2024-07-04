using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class NullSafeExpressionTests : RewriteTest
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

    [Fact]
    public void ArrayAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public object? M(int[]? i)
                    {
                        return i?[0].ToString();
                    }
                }
                """
            )
        );
    }
}