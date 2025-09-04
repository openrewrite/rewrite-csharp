namespace Rewrite.CSharp.Tests.Tree;
using static Assertions;
public class GlobalTests : RewriteTest
{
    [Test]
    public void TopLevelProgram()
    {
        RewriteRun(
            CSharp(
                """
                var x = "hi";
                """
            ));
    }
}
