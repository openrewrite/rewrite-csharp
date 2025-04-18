using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class VariableDeclarationsTests : RewriteTest
{
    [Test]
    public void Space()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    void M()
                    {
                        int i1 = /*1*/ 1 /*2*/ ; /*3*/
                        int i2 = /*4*/ 1 /*5*/ , /*6*/ i3 /*7*/ = /*8*/ 2 /*9*/ ; /*10*/
                    }
                }
                "
            )
        );
    }

    [Test]
    void Modifiers()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    void M()
                    {
                        const int i1 = 0;
                    }
                }
                """
            )
        );
    }
}
