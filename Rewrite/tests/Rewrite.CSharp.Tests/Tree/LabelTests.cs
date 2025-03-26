using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LabelTests : RewriteTest
{
    [Test]
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
