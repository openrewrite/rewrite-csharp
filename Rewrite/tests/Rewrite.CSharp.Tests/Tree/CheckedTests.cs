using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CheckedTests : RewriteTest
{
    // ==================== CheckedStatement Tests ====================

    [Test]
    public void CheckedStatement()
    {
        RewriteRun(
            CSharp(
                """
                checked
                {

                }
                """
            )
        );
    }

    [Test]
    public void UncheckedStatement()
    {
        RewriteRun(
            CSharp(
                """
                unchecked
                {

                }
                """
            )
        );
    }

    [Test]
    public void CheckedWithArithmetic()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        checked
                        {
                            int x = a + b;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void UncheckedWithOverflow()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        unchecked
                        {
                            int x = int.MaxValue + 1;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedWithMultipleStatements()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        checked
                        {
                            a++;
                            b *= 2;
                            int c = a + b;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedNested()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        checked
                        {
                            unchecked
                            {
                                int x = 1;
                            }
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void UncheckedNestedInChecked()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        unchecked
                        {
                            checked
                            {
                                int x = 1;
                            }
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedWhitespace()
    {
        RewriteRun(
            CSharp(
                """
                checked  {  }
                """
            )
        );
    }

    [Test]
    public void CheckedWithComments()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        /*before*/checked/*after*/
                        {
                            /*inside*/
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedInMethod()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        checked { }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedWithCast()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(long longVal)
                    {
                        checked
                        {
                            int x = (int)longVal;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedWithLoop()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        checked
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int x = i * i;
                            }
                        }
                    }
                }
                """
            )
        );
    }

    // ==================== CheckedExpression Tests ====================

    [Test]
    public void CheckedExpression()
    {
        RewriteRun(
            CSharp(
                """
                var a = checked(1);
                """
            )
        );
    }

    [Test]
    public void UncheckedExpression()
    {
        RewriteRun(
            CSharp(
                """
                var a = unchecked(1);
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionArithmetic()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        var x = checked(a + b);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionComplex()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b, int c)
                    {
                        var x = checked(a * b + c);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionWithCast()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(long longVal)
                    {
                        var x = checked((int)longVal);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionNested()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int x)
                    {
                        var y = checked(unchecked(x + 1) + 2);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void UncheckedExpressionNested()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int x)
                    {
                        var y = unchecked(checked(x + 1) + 2);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionInAssignment()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        int x = checked(a + b);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionWhitespace()
    {
        RewriteRun(
            CSharp(
                """
                var a = checked ( 1 );
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionWithComments()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        var a = /*before*/checked/*after*/(/*inner*/1/*end*/);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionInReturn()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    int M(int a, int b)
                    {
                        return checked(a + b);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionInMethodCall()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        Console.WriteLine(checked(a + b));
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionMultiply()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(int a, int b)
                    {
                        var x = checked(a * b);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void UncheckedExpressionOverflow()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        var x = unchecked(int.MaxValue + 1);
                    }
                }
                """
            )
        );
    }

    [Test]
    public void CheckedExpressionInTernary()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(bool condition, int a, int b)
                    {
                        var x = condition ? checked(a + b) : 0;
                    }
                }
                """
            )
        );
    }
}
