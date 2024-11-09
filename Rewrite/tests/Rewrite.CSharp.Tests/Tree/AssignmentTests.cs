using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class AssignmentTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    private void Simple()
    {
        RewriteRun(
          CSharp(
              """
              class T
              {
                  void M()
                  {
                      int n;
                      n = 1;
                  }
              }
              """
          )
        );
    }

    [Fact]
    private void AssignmentOperation()
    {
        RewriteRun(
          CSharp(
              """
              class T
              {
                  void M()
                  {
                      int n = 0;
                      n += 1;
                      n -= 1;
                      n *= 1;
                      n /= 1;
                      n %= 1;
                      n &= 1;
                      n |= 1;
                      n ^= 1;
                      n <<= 1;
                      n >>= 1;
                      n >>>= 1;
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
                      o ??= 1;
                      return o;
                  }
              }
              """
          )
        );
    }
}
