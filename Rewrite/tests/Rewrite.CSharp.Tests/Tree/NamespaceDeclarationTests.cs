using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class NamespaceDeclarationTests : RewriteTest
{
    [Fact]
    public void EmptyBlockScopeNamespace()
    {
        RewriteRun(
            CSharp(
                @"
                namespace Foo {
                }
                "
            )
        );
    }

    [Fact]
    public void EmptyBlockScopeNamespaceWithTrailingSemicolon()
    {
        RewriteRun(
            CSharp(
                @"
                namespace Foo {
                };
                "
            )
        );
    }

    [Fact]
    public void EmptyFileScopeNamespaceWithTrailingSemicolon()
    {
        RewriteRun(
            CSharp(
                @"
                namespace Foo ;
                "
            )
        );
    }

    [Fact]
    public void QualifiedFileScopeNamespace()
    {
        RewriteRun(
            CSharp(
                @"
                namespace Foo.Bar;
                "
            )
        );
    }
}
