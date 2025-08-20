using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ForLoopTests : RewriteTest
{

    [Test]`n    public void ForLoopMultipleInit()
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

    [Test]`n    public void ForLoop()
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

    [Test]`n    public void InfiniteLoop()
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

    [Test]`n    public void Format()
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

    [Test]`n    public void FormatInfiniteLoop()
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

    [Test]`n    public void FormatLoopNoInit()
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

    [Test]`n    public void FormatLoopNoCondition()
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

    [Test]`n    public void StatementTerminatorForSingleLineForLoops()
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

    [Test]`n    public void InitializerIsAnAssignment()
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

    [Test]`n    public void MultiVariableInitialization()
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

    [Test]`n    public void MultiInitExpressions()
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
