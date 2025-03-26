using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class SizeOfTests : RewriteTest
{
    [Test]
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

    [Test]
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
