using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ExternAliasTests : RewriteTest
{
    [Fact]
    public void CompilationUnit()
    {
        RewriteRun(
            CSharp(
                """
                extern /*0*/ alias /*1*/ Foo /*2*/ ;
                """
            )
        );
    }

    [Fact]
    public void FileNamespace()
    {
        RewriteRun(
            CSharp(
                """
                namespace Foo;

                extern /*0*/ alias /*1*/ Foo /*2*/ ;
                """
            )
        );
    }
}
