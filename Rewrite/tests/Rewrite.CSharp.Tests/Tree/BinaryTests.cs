using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class BinaryTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
