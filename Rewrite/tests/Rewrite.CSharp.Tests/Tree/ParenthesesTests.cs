using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ParenthesesTests : RewriteTest
{

    [Test]
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

    [Test]
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
