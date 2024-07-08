using FluentAssertions;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class InterfaceDeclarationTests : RewriteTest
{

    [Fact]
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

    [Fact]
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
                    var bar = (cu.Members.Last() as J.ClassDeclaration)!;
                    bar.GetKind().Should().Be(J.ClassDeclaration.Kind.Type.Interface);
                    bar.Extends.Should().BeNull();
                    bar.Implements.Should().NotBeNull();
                }
            )
        );
    }

    [Fact]
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
                    var bar = (cu.Members.Last() as J.ClassDeclaration)!;
                    bar.GetKind().Should().Be(J.ClassDeclaration.Kind.Type.Interface);
                    bar.Extends.Should().BeNull();
                    bar.Implements.Should().NotBeNull();
                }
            )
        );
    }
    
    [Fact]
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
    
    
    
    [Fact]
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
    
    [Fact]
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
}