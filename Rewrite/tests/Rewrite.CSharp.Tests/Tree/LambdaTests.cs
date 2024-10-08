using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class LambdaTests : RewriteTest
{

    [Fact]
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
    [Fact]
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


    [Fact]
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

    [Fact]
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

    [Fact]
    void ParenthesizedMultiArgsLambdaWithComments()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    Func<double, double> square =  /*1*/   (  /*2*/ x, /*3*/       y,       /*4*/ z)  /*5*/    =>      /*6*/  x * x;
                }
                """
            )
        );
    }

    [Fact]
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

    [Fact]
    [KnownBug]
    private void AsyncLambda()
    {
        RewriteRun(CSharp("Task.Run(async () => {});"));
    }

}
