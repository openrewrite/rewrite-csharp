using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DestructorTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void BasicDestructor()
    {
        RewriteRun(
            CSharp("""
                   public class A
                   {
                       ~A()
                       {
                       }
                   }
                   """));
    }

    [Fact]
    public void DestructorWithBody()
    {
        RewriteRun(
            CSharp("""
                   class Resource
                   {
                       ~Resource()
                       {
                           Dispose(false);
                           GC.SuppressFinalize(this);
                       }
                   }
                   """));
    }

    [Fact]
    public void PartialDestructor()
    {
        RewriteRun(
            CSharp("""
                   partial class C
                   {
                       ~C()
                       {
                       }
                   }
                   """));
    }

    [Fact]
    public void NestedClassDestructor()
    {
        RewriteRun(
            CSharp("""
                   public class Outer
                   {
                       private class Inner
                       {
                           ~Inner()
                           {
                           }
                       }
                   }
                   """));
    }

    [Fact]
    public void DestructorWithAttributes()
    {
        RewriteRun(
            CSharp("""
                   public class D
                   {
                       [Conditional]
                       ~D()
                       {
                       }
                   }
                   """));
    }

    [Fact]
    public void ExpressionBodiedDestructor()
    {
        RewriteRun(
            CSharp("""
                   public class E
                   {
                       ~E() => Dispose();
                   }
                   """));
    }
}


