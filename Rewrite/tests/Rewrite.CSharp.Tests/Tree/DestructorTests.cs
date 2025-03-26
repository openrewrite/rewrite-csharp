using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DestructorTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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


