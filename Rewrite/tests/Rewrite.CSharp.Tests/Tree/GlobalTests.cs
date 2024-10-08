namespace Rewrite.CSharp.Tests.Tree;
using static Assertions;
[Collection(Collections.PrinterAccess)]
public class GlobalTests : RewriteTest
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
