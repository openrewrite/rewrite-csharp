namespace Rewrite.CSharp.Tests.Tree;
using static Assertions;
public class GlobalTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    void TopLevelProgram()
    {
        RewriteRun(
            CSharp(
                """
                var x = "hi";
                """
            ));
    }
}
