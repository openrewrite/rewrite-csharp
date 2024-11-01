using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ConstructorTests : RewriteTest
{
    [Fact]
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

    [Fact]
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
}


