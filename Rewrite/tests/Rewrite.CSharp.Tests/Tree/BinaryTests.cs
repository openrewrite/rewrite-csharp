using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class BinaryTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    void Arithmetic()
    {
        RewriteRun(
          CSharp(
            """
              class Test
              {
                  void M()
                  {
                      int n;
                      n = 0 + 1;
                      n = 10 - 5;
                      n = 10 / 5;
                      n = 10 * 5;
                      n = 10 % 5;
                      n = 10 ^ 5;
                      n = 10 & 5;
                      n = 10 | 5;
                  }
              }
              """
          )
        );
    }

    [Fact]
    void BooleanLogic()
    {
        RewriteRun(
            CSharp(
                """
                class Test
                {
                    void test()
                    {
                        bool b = 1 == 2 //
                                 && 3 == 4 //
                                 || 4 == 5;
                    }
                }
                """
            )
        );
    }

    [Fact]
    private void NullCoalescing()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    object M(object? o)
                    {
                        return o ?? 1;
                    }
                }
                """
            )
        );
    }

    [Fact]
    private void As()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    object M(object? o)
                    {
                        return as int;
                    }
                }
                """
            )
        );
    }
}
