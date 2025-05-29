using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class MethodInvocationTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
    public void InvocationSpacing()
    {
        RewriteRun(
            CSharp(
                @"
                a.GetField . GetCustomAttribute<EnumMemberAttribute>();
                "
            )
        );
    }

    [Test]
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
        Console.WriteLine(lst.RenderLstTree());
    }

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
