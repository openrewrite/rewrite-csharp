using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TypeCastTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    var s = (string)""Foo"";
                }
                "
            )
        );
    }
}
