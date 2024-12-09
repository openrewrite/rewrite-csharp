using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ForLoopTests(ITestOutputHelper output) : RewriteTest(output)
{

    [Fact]
    void ForLoopMultipleInit()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        int i;
                        int j;
                        for(i = 0, j = 0;;) {
                        }
                    }
                  }
                """
            )
        );
    }

    [Fact]
    void ForLoop()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for(int i = 0; i < 10; i++) {
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    void InfiniteLoop()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for(;;) {
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    void Format()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for ( int i = 0 ; i < 10 ; i++ ) {
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    void FormatInfiniteLoop()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for ( ; ; ) {}
                    }
                }
                """
            )
        );
    }

    [Fact]
    void FormatLoopNoInit()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test(int i) {
                        for ( ; i < 10/*1*/; i++ ) {}
                    }
                }
                """
            )
        );
    }

    [Fact]
    void FormatLoopNoCondition()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        int i = 0;
                        for(; i < 10; i++) {}
                    }
                }
                """
            )
        );
    }

    [Fact]
    void StatementTerminatorForSingleLineForLoops()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for(;;) test();
                    }
                }
                """
            )
        );
    }

    [Fact]
    void InitializerIsAnAssignment()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {

                        int[] a;
                        int i=0;
                        for(i=0; i<a.length; i++) {}
                    }
                }
                """
            )
        );
    }

    [Fact]
    void MultiVariableInitialization()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        for(int i, j = 0;;) {}
                    }
                }
                """
            )
        );
    }

    [Fact]
    void MultiInitExpressions()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void test() {
                        int i;
                        int j = 3;
                        for (/*asd*/ i = 0, Console.WriteLine($"Start: i={i}, j={j}")/*asd*/; i < j; i++, j--, Console.WriteLine($"Step: i={i}, j={j}"))
                        {
                            //...
                        }
                    }
                }
                """
            )
        );
    }
}
