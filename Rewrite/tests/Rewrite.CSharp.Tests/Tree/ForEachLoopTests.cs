using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ForEachLoopTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    void SimpleForEachLoop()
    {
        RewriteRun(
            CSharp(
                """
                foreach (int element in new int[]{ 0, 1, 1, 2, 3, 5, 8, 13 })
                {

                }

                """
            ),
            CSharp(
                """
                var n = new int[] { 1, 2, 3};
                foreach (int element in n)
                {

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
                /*dsas?*/ foreach /*dsas?*/ (/*dsas?*/ int /*dsas?*/ element /*dsas?*/ in   n /*dsas?*/) /*dsas?*/
                {

                }
                """
            )
        );
    }

    [Fact]
    void MultiVariableForEachLoop()
    {
        RewriteRun(
            CSharp(
                """
                var points = new[]{(1,2)};
                foreach ((var x, var y) in points)
                {

                }
                """
            )
        );
    }

    [Fact]
    void ForEachAwait()
    {
        RewriteRun(
            CSharp(
                """
                await foreach (var a in b)
                {

                }
                """
            ));
    }
}
