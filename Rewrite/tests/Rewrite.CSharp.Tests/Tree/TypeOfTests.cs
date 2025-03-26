using Rewrite.Test;
using Rewrite.Test.CSharp;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TypeOfTests : RewriteTest
{
    [Test]
    public void SimpleTypeOf()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                @"
                public class T
                {
                    var t = typeof(int);
                }
                "
            )
        );
    }

    [Test]
    public void SimpleTypeOfWithComments()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                @"
                public class T
                {
                    var t =   /*1*/     typeof /*2*/  (  /*3*/     int)   /*4*/ ; // test
                }
                "
            )
        );
    }
}
