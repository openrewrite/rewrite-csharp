using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TypeCastTests : RewriteTest
{
    [Test]
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
