using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IfTests : RewriteTest
{
    [Test]
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


    [Test]
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

    [Test]
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

    [Test]
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
