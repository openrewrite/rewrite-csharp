using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ThrowTests : RewriteTest
{
    [Fact]
    void SimpleThrowStatement()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void M() {
                        throw new ArgumentException();
                    }
                }
                """
            )
        );
    }

    [Fact]
    void EmptyThrowStatement()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        try
                        {
                            Console.Write("Process");
                        }
                        catch (Exception e)
                        {
                            Console.Write("Failed to process request");
                            throw;
                        }
                        finally
                        {
                            Busy = false;
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    void ThrowExpression()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int M(int i)
                    {
                        var e = new Exception();
                        return i > 0 ? i : throw e;
                    }
                }
                """
            )
        );
    }
}
