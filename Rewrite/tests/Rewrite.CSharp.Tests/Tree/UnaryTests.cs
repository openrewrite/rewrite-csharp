using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnaryTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
