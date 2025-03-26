using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ConstructorTests : RewriteTest
{
    [Test]
    public void VanillaConstructor()
    {
        var src = CSharp("""
                         public class A
                         {
                             public A()
                             {}
                         })
                         """);
        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void ConstructorWithThisCall()
    {
        var src = CSharp("""
                         public class A
                         {
                             public A() : this(1)
                             {}

                             public A(int a)
                             {
                             }
                         })
                         """);
        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void ConstructorWithArrowStatement()
    {
        RewriteRun(
            CSharp(
                """
                class C {
                    C(P p) => _p = p;
                }
                """
            )
        );
    }
}


