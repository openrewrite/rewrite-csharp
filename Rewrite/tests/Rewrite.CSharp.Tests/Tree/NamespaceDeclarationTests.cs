using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NamespaceDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
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
