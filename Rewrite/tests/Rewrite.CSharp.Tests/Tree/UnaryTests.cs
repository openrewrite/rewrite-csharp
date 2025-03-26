using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnaryTests : RewriteTest
{
    [Test]
    void Format()
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
    void Negation()
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
    void NullWarningSuppress()
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
    void NullableFunctionProperty()
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
    void IndexExpression()
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
    void AddressOfExpression()
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
    void PointerIndirection()
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
    void PointerType()
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
