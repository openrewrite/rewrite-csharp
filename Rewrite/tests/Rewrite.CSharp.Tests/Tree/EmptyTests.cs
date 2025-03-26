using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EmptyTests : RewriteTest
{
    [Test]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    void M()
                    {
                        ;
                    }
                }
                """
            )
        );
    }
}
