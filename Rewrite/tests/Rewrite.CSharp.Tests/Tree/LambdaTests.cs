using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LambdaTests : RewriteTest
{

    [Test]
    void SimpleLambda()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    System.Func<double, double> square = x => x * x;
                }
                """
            )
        );
    }
    [Test]
    void SimpleLambdaWithComments()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    System.Func<double, double> square =  /*1*/    x  /*2*/    =>     /*3*/  x * x;
                }
                """
            )
        );
    }


    [Test]
    void SimpleLambdaWithBlockBody()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    Func<double, double> square = x =>
                        {
                            return x * x;
                          };
                }
                """
            )
        );
    }

    [Test]
    void ParenthesizedMultiArgsLambda()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    Func<double, double> square = (x, y, z) => x * x;
                }
                """
            )
        );
    }

    [Test]
    void ParenthesizedMultiArgsLambdaWithComments()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    Func<double, double> square =  /*1*/   (  /*2*/ x, /*3*/       y,       /*4*/ z)  /*5*/    =>      /*6*/  x * x /*7*/;
                }
                """
            )
        );
    }

    [Test]
    void ParenthesizedMultiArgsLambdaWithBlockBody()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    System.Func<double, double> square =  /*1*/   (  /*2*/ x, /*3*/       y,       /*4*/ z)  /*5*/    =>      /*6*/
                        {
                            return x * x;
                        };
                }
                """
            )
        );
    }

    [Test]
    private void AsyncLambda()
    {
        RewriteRun(CSharp("Task.Run(async () => {});"));
    }

    [Test]
    private void LambdaWithModifiers()
    {
        RewriteRun(CSharp("Task.Run( async static () => {});"));
    }

    [Test]
    private void LambdaWithReturnType()
    {
        RewriteRun(CSharp("Task.Run( async static bool () => {});"));
    }

}
