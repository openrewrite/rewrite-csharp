using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NamespaceDeclarationTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
