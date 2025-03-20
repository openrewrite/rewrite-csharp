using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ClassDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void BaseParameters()
    {
        RewriteRun(
            CSharp(
                """
                class   PlayTests(ITestOutputHelper output) : RewriteTest( output    ) {

                }
                """, c =>
                {
                    c.AfterRecipe = comp => { _output.WriteLine(comp.ToString()); };
                }
            )
        );
    }


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
        var lst = src.Parse();
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

    [Fact]
    public void MultipleTypeConstraints()
    {
        RewriteRun(
            CSharp(
                """
                public interface IRepository<T, TId>
                    where TId : IEquatable<TId>
                    where T : class, ITableObject<TId>
                {
                }
                """
            )
        );
    }

    [Fact]
    public void ClassDeclarationWithAttribute()
    {
        RewriteRun(
            CSharp(
                """
                [Obsolete]
                public    class Foo;
                """
            )
        );
    }
}
