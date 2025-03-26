using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class InterfaceDeclarationTests : RewriteTest
{

    [Test]
    public void ModifierAndInterfaceWithoutBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                public    interface Foo;
                "
            )
        );
    }

    [Test]
    public void InterfaceExtendingSingle()
    {
        RewriteRun(
            CSharp(
                @"
                interface Foo;
                interface Baz : Foo;
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var bar = cu.Descendents().OfType<Cs.ClassDeclaration>().First(x => x.Name == "Baz");
                    bar.Kind.KindType.Should().Be(J.ClassDeclaration.Kind.Types.Interface);
                    bar.Extendings.Should().BeNull();
                    bar.Implementings.Should().NotBeNull();
                }
            )
        );
    }

    [Test]
    public void InterfaceExtendingMultiple()
    {
        RewriteRun(
            CSharp(
                @"
                interface Foo;
                interface Bar;
                interface Baz : Foo, Bar;
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var bar = cu.Descendents().OfType<Cs.ClassDeclaration>().First(x => x.Name == "Baz");
                    bar.Kind.KindType.Should().Be(J.ClassDeclaration.Kind.Types.Interface);
                    bar.Extendings.Should().BeNull();
                    bar.Implementings.Should().NotBeNull();
                }
            )
        );
    }

    [Test]
    public void ClassWithoutBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    interface Foo;
                "
            )
        );
    }



    [Test]
    public void InterfaceWithWithoutBodyWithTypeParams()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    interface Foo<T>;
                "
            )
        );
    }

    [Test]
    public void InterfaceWithEmptyBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    interface Foo
                    {

                    }
                "
            )
        );
    }

    [Test]
    public void InterfaceWithWithoutBodyWithTypeParamsInOut()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    interface Foo<in T, out K>;
                "
            )
        );
    }
}
