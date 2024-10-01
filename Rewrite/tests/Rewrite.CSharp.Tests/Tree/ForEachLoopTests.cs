using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ForEachLoopTests : RewriteTest
{
    [Fact]
    void SimpleForEachLoop()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        foreach (int element in new int[]{ 0, 1, 1, 2, 3, 5, 8, 13 })
                        {
                            Console.Write(element.ToString());
                        }
                        // Output:
                        // 0 1 1 2 3 5 8 13
                    }
                }
                """
            ),
            CSharp(
                """
                class Test {
                    void test(int[] n) {
                        foreach (int element in n)
                        {
                            Console.Write((element.ToString()));
                        }
                        // Output:
                        // 0 1 1 2 3 5 8 13
                    }
                }
                """
            )
        );
    }

    [Fact]
    void CommentsForEachLoop()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test(int[] n) {
                        /*dsas?*/ foreach /*dsas?*/ (/*dsas?*/ int /*dsas?*/ element /*dsas?*/ in   n /*dsas?*/) /*dsas?*/
                        {
                            Console.Write(element.ToString());
                        }
                        // Output:
                        // 0 1 1 2 3 5 8 13
                    }
                }
                """
            )
        );
    }
}
