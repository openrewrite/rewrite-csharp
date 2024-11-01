using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class SwitchStatementTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                class Foo
                {
                    public void PrintDayOfWeek(int day)
                    {
                        switch (day)
                        {
                            case 0:
                                System.Console.WriteLine("Sunday");
                                break;
                            default:
                                System.Console.WriteLine("Not Sunday");
                                break;
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    void SimpleSwitch()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void DisplayMeasurement(double measurement)
                    {
                        switch (measurement)
                        {
                            case < 0.0:
                                Console.WriteLine("Measured value is too low.");
                                break;

                            case > 15.0:
                                Console.WriteLine("Measured value is too high.");
                                break;
                             /*asda*/
                            /*asda*/ case /*asda*/ double.NaN /*asda*/:
                            case double.NaN:
                                Console.WriteLine("Failed measurement.");
                                break;

                            default:
                                Console.WriteLine("Measured value is " + measurement);
                                break;
                        }
                    }
                }
                """
            )
        );
    }

}
