using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
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
