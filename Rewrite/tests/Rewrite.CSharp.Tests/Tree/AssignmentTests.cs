using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class AssignmentTests : RewriteTest
{
    [Test]
    public void Simple()
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

    [Test]
    public void AssignmentOperation()
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

    [Test]
    public void NullCoalescing()
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
