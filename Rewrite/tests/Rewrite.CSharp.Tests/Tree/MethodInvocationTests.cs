using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class MethodInvocationTests : RewriteTest
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
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    string s = this.Equals(obj: null);
                }
                "
            )
        );
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
