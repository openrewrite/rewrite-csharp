using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests;

using static Assertions;

public class FindClassTests : RewriteTest
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new FindClass();
    }

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
