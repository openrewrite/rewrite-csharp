using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class SizeOfTests : RewriteTest
{
    [Fact]
    public void SimpleSizeOf()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                @"
                public class T
                {
                    var t = sizeof(int);
                }
                "
            )
        );
    }

    [Fact]
    public void SimpleSizeOfWithComments()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                @"
                public class T
                {
                    var t =   /*1*/     sizeof /*2*/  (  /*3*/     int)   /*4*/ ; // test
                }
                "
            )
        );
    }
}
