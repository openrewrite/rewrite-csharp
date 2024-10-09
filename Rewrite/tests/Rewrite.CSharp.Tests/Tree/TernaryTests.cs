using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class TernaryTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    int _i = true ? 1 : 2;
                }
                "
            )
        );
    }

    [Fact]
    public void Space()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    int _i = /*1*/ true /*2*/ ? /*3*/ 1 /*4*/ : /*5*/ 2 /*6*/ ; /*7*/
                }
                "
            )
        );
    }
}
