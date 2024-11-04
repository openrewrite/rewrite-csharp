using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CompilationUnitTests : RewriteTest
{
    [Fact]
    public void Empty()
    {
        RewriteRun(
            CSharp(
                @"
                ",
                spec => spec.SourcePath = "foo.cs"
            )
        );
    }

    [Fact]
    public void OnlyComment()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                "
            )
        );
    }

    [Fact]
    public void EndsWithCrlf()
    {
        RewriteRun(CSharp("class A{} \r\n"));
    }


}
