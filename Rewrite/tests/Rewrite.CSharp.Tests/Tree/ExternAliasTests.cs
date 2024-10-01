using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
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
