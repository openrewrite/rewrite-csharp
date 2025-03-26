using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ClassDeclarationTests : RewriteTest
{
    [Test]
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


    [Test]
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

    [Test]
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


    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
