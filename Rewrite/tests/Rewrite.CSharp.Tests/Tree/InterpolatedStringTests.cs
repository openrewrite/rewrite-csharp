using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class InterpolatedStringTests(ITestOutputHelper output) : RewriteTest(output)
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

    [Fact]
    public void Alignment()
    {
        RewriteRun(
            CSharp(
                """"
                public class Foo
                {
                    string M(string s) => $"""Hello { s, 10  }""";
                }
                """"
            )
        );
    }

    [Fact]
    public void AlignmentAndFormat()
    {
        RewriteRun(
            CSharp(
                """"
                public class Foo
                {
                    string M(string s) => $"""Hello { s, 10  :C   }""";
                }
                """"
            )
        );
    }
}
