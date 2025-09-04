using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IfTests : RewriteTest
{
    [Test]
    public void IfElse()
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
    public void NoElse()
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
    public void SingleLineIfElseStatements()
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
    public void ElseWithTrailingSpace()
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
