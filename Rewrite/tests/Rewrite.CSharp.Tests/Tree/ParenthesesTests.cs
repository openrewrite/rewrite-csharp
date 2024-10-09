using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class ParenthesesTests : RewriteTest
{

    [Fact]
    void Parentheses() {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int n = (0);
                }
                """
            )
        );
    }

    [Fact]
    void ParenthesesWithComments() {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int n = /*adas*/ (  0 /*adas*/) /*adas*/; //asdasda
                }
                """
            )
        );
    }

}
