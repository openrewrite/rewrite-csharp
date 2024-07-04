using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
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
}