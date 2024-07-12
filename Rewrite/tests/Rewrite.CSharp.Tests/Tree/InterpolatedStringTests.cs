using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class InterpolatedStringTests : RewriteTest
{
    [Fact]
    public void SingleLine()
    {
        RewriteRun(
            CSharp(
                """"
                public class Foo
                {
                    string s = $"""Hello""";
                }
                """"
            )
        );
    }

    [Fact]
    public void Interpolated()
    {
        RewriteRun(
            CSharp(
                """"
                public class Foo
                {
                    string M(string s) => $"""Hello { s }""";
                }
                """"
            )
        );
    }

    [Fact]
    public void MultiLine()
    {
        RewriteRun(
            CSharp(
                """"
                public class Foo
                {
                    string s = $"""
                        Hello
                        """;
                }
                """"
            )
        );
    }
}