using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class FindClassTests : RewriteTest
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new FindClass();
    }

    [Fact]
    public void FindSimpleClassTest()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo;
                ",
                @"
                /*~~>*/public class Foo;
                "
            )
        );
    }

    [Fact]
    public void FindInterfaceTest()
    {
        RewriteRun(
            CSharp(
                @"
                public    interface Foo{}
                ",
                @"
                /*~~>*/public    interface Foo{}
                "
            )
        );
    }

    [Fact]
    public void FindRecordTest()
    {
        RewriteRun(
            CSharp(
                @"
                public    record Foo{}
                ",
                @"
                /*~~>*/public    record Foo{}
                "
            )
        );
    }

    [Fact]
    public void FindNestedClasses()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public class Bar;
                    public interface Baz;
                }
                ",
                @"
                /*~~>*/public class Foo
                {
                    /*~~>*/public class Bar;
                    /*~~>*/public interface Baz;
                }
                "
            )
        );
    }
}
