using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ForEachLoopTests : RewriteTest
{
    [Test]`n    public void SimpleForEachLoop()
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

    [Test]`n    public void CommentsForEachLoop()
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

    [Test]`n    public void MultiVariableForEachLoop()
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

    [Test]`n    public void ForEachAwait()
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
