using Rewrite.Test.CSharp;
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

    [Fact]
    [KnownBug]
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

    [Fact]
    [KnownBug]
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

    [Fact]
    [KnownBug]
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

    [Fact]
    [KnownBug]
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

    [Fact]
    [KnownBug]
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
