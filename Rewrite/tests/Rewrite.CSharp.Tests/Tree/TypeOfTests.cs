using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class TypeOfTests : RewriteTest
{
    [Fact]
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
    
    [Fact]
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