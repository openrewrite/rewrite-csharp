using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

using static Assertions;

public class FindClassDeclarationTests : RewriteTest
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new FindClass();
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

    [Fact]
    [KnownBug]
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
