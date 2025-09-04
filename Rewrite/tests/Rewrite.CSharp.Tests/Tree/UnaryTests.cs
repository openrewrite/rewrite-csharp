using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnaryTests : RewriteTest
{
    [Test]
    public void Format()
    {
        RewriteRun(
          CSharp(
            """
              class Test {
                  void test()
                  {
                      int i = 0;
                      int j = ++i;
                      int k = i ++;
                      int l = --i;
                      int m = i  --;
                      int n = -(i + 2);
                      int o = +n  ;
                      int p = ~o;
                  }
              }
              """
          )
        );
    }

    [Test]
    public void Negation()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    bool b = !(1 == 2);
                }
                """
            )
        );
    }

    [Test]
    public void NullWarningSuppress()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    string a = ""!;
                }
                """
            )
        );
    }

    [Test]
    [KnownBug]
    public void NullableFunctionProperty()
    {
        RewriteRun(
            CSharp(
                """
                field.Method!();
                """
            )
        );
    }

    [Test]
    public void IndexExpression()
    {
        RewriteRun(
            CSharp(
                """
                a[^1];
                """
            )
        );
    }

    [Test]
    public void AddressOfExpression()
    {
        RewriteRun(
            CSharp(
                """
                &myvar;
                """
            )
        );
    }

    [Test]
    public void PointerIndirection()
    {
        RewriteRun(
            CSharp(
                """
                *ptr;
                """
            )
        );
    }

    [Test]
    public void PointerType()
    {
        RewriteRun(
            CSharp(
                """
                int* a;
                """
            )
        );
    }
}
