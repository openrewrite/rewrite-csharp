using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class InterpolatedStringTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
