using Rewrite.Recipes;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteJava;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

using static Assertions;

[Collection("C# remoting")]
public class FindClassDeclarationTests : RewriteTest
{
    public override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new FindClass(null);
    }

    [Fact]
    public void ModifierAndClassWithoutBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                public    class Foo;
                ",
                @"
                // comment
                /*~~>*/public    class Foo;
                "
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
                    class Foo;
                ",
                @"
                // comment
                    /*~~>*/class Foo;
                "
            )
        );
    }
    
    
    
    [Fact]
    public void ClassWithEmptyPrimaryCtorWithoutBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    class Foo();
                ",
                @"
                // comment
                    /*~~>*/class Foo();
                "
            )
        );
    }
    
    [Fact]
    public void ClassWithEmptyPrimaryCtorWithEmptyBody()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    class Foo()
                    {
                        
                    }
                ",
                @"
                // comment
                    /*~~>*/class Foo()
                    {
                        
                    }
                "
            )
        );
    }
    
    [Fact(Skip = "For now we dont extract types")]
    public void ClassWithoutBody2()
    {
        RewriteRun(
            CSharp(
                @"
                // comment
                    class Foo(int   test);
                "
            )
        );
    }
}