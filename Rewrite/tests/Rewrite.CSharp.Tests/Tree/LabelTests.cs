using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class LabelTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public void Foo()
                    {
                        foo:
                        var s = "Foo";
                    }
                }
                """
            )
        );
    }
}
