using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class MethodInvocationTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void NoReceiver()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    string s = ToString();
                }
                "
            )
        );
    }

    [Fact]
    public void InvocationOnThis()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    string s = this.ToString();
                }
                "
            )
        );
    }

    [Fact]
    public void InvocationWithNamedParameters()
    {
        var src = CSharp(
            @"
                public class T
                {
                    void M()
                    {
                        string s = this.Equals(obj: null, 1);
                    }
                }
                "
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Fact]
    public void InvocationWithOutParameter()
    {
        var src = CSharp(
            """
                public class T
                {
                    void M()
                    {
                        int.TryParse("1", out var one);
                    }
                }
                """
        );

        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Fact]
    public void InvocationWithRefParameter()
    {
        var src = CSharp(
            """
            public class T
            {
                void M()
                {
                    int.TryParse("1", ref one);
                }
            }
            """
        );

        var lst = src.Parse();
        var statement = lst.Descendents().OfType<J.MethodInvocation>().First();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Fact]
    public void InvocationWithGenerics()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    string _s = M<string>();

                    static string M<T>()
                    {
                        return ""Foo"";
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void QualifiedInvocationWithGenerics()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    string _s = T.M<string>();

                    static string M<T>()
                    {
                        return ""Foo"";
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void InvocationOfDelegateReturnedByArrayAccess()
    {
        RewriteRun(
            CSharp(
                """
                a[0]();
                """
            )
        );

    }

    [Fact]
    public void InvocationOfDelegateReturnedByMethod()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    void Main()
                    {
                        Something()();
                    }
                    public static Func<string> Something()
                    {
                        return () => "hello";
                    }
                }
                """
            )
        );

    }

    [Fact]
    public void ArgumentOnNewLine()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    void Main()
                    {
                      /*1*/  Something(
                      /*2*/      there);
                    }
                }
                """)
        );
    }
}
