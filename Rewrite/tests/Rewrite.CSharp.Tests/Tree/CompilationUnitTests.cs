using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
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
}