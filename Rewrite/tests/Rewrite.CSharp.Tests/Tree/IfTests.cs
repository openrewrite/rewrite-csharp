using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class IfTests : RewriteTest
{
    [Fact]
    void IfElse()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        int n = 0;
                        if(n == 0) {
                        }
                        else if(n == 1) {
                        }
                        else {
                        }
                    }
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
                class Test {
                    void test() {
                        int n = 0;
                        if (n == 0) {
                        }
                    }
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
                class Test {
                    void test() {
                        int n = 0;
                        if (n == 0) n++;
                        else if (n == 1) n++;
                        else n++;
                    }
                }
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
                class Test {
                    void test() {
                        if (true) {
                        }
                        else{ 
                            Console.WriteLine("test");
                        }
                    }
                }
                """
            )
        );
    }
}