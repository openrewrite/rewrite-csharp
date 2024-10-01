using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ClassDeclarationTests : RewriteTest
{
    [Fact]
    public void ModifierAndClassWithoutBody()
    {
        RewriteRun(
            CSharp(
                """
                public    class Foo;
                """
            )
        );
    }

    [Fact]
    public void ClassWithoutBody()
    {
        RewriteRun(
            CSharp(
                """
                class Foo;
                """
            )
        );
    }


    [Fact]
    public void ClassWithEmptyPrimaryCtorWithoutBody()
    {
        RewriteRun(
            CSharp(
                """
                class Foo();
                """
            )
        );
    }

    [Fact]
    public void ClassWithEmptyPrimaryCtorWithEmptyBody()
    {
        RewriteRun(
            CSharp(
                """
                class Foo()
                {

                }
                """
            )
        );
    }

    [Fact]
    public void ClassWithPrimaryCtorWithEmptyBody()
    {
        RewriteRun(
            CSharp(
                """
                class Foo(int   test);
                """
            )
        );
    }

    [Fact]
    public void NestedClass()
    {
        RewriteRun(
            CSharp(
                """
                class Foo
                {
                    class Bar;
                }
                """
            )
        );
    }

    [Fact]
    public void TypeParameter()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T>;
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithTypeConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : System.String;
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithClassConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : class;
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithEnumConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : enum;
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithNewConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : new();
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithMultipleConstraints()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : IList<int>, IEnumerable<int>, new();
                """
            )
        );
    }
}
