using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

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
                class Foo /*1*/ ;
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
    public void TypeParameterWithClassConstraint()
    {
        var src = CSharp(
            """
            class Foo<T> where T : class;
            """
        );
        var lst = src.First().Parse<Cs.CompilationUnit>();
        RewriteRun(
            src
        );
    }

    [Fact]
    public void TypeParameterWithStructConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Foo<T> where T : struct;
                """
            )
        );
    }

    [Fact]
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
