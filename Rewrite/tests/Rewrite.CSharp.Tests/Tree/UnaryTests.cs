using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class UnaryTests : RewriteTest
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
}
