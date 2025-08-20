using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnaryTests : RewriteTest
{
    [Test]`n    public void Format()
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

    [Test]`n    public void Negation()
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

    [Test]`n    public void NullWarningSuppress()
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

    [Test]`n    public [KnownBug] void NullableFunctionProperty()
    {
        RewriteRun(
            CSharp(
                """
                field.Method!();
                """
            )
        );
    }

    [Test]`n    public void IndexExpression()
    {
        RewriteRun(
            CSharp(
                """
                a[^1];
                """
            )
        );
    }

    [Test]`n    public void AddressOfExpression()
    {
        RewriteRun(
            CSharp(
                """
                &myvar;
                """
            )
        );
    }

    [Test]`n    public void PointerIndirection()
    {
        RewriteRun(
            CSharp(
                """
                *ptr;
                """
            )
        );
    }

    [Test]`n    public void PointerType()
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
