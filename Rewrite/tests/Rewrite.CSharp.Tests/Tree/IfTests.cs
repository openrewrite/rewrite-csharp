using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IfTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    void IfElse()
    {
        RewriteRun(
            CSharp(
                """
                int n = 0;
                if(n == 0) {
                }
                else if(n == 1) {
                }
                else {
                }

                """
            )
        );
    }


    [Fact]
    void NoElse()
    {
        RewriteRun(
            CSharp(
                """
                int n = 0;
                if (n == 0) {
                }

                """
            )
        );
    }

    [Fact]
    void SingleLineIfElseStatements()
    {
        RewriteRun(
            CSharp(
                """
                int n = 0;
                if (n == 0) n++;
                else if (n == 1) n++;
                else n++;
                """
            )
        );
    }

    [Fact]
    void ElseWithTrailingSpace()
    {
        RewriteRun(
            CSharp(
                """
                if (true) {
                }
                else{
                    Console.WriteLine("test");
                }
                """
            )
        );
    }
}
