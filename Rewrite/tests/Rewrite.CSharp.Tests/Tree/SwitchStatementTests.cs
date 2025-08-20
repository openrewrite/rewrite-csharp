using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class SwitchStatementTests : RewriteTest
{
    [Test]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                switch (o)
                {
                    case 0:
                        a = "one";
                        break;
                    default:
                        a = "two";
                        return;
                }

                """
            )
        );
    }

    [Test]
    public void SwitchWithClause()
    {
        RewriteRun(
            CSharp(
                """
                switch (o)
                {
                    case 0 when 1 > 0:
                        return;
                }

                """
            )
        );
    }

    [Test]
    public void MultipleLabelsOnSingleArm()
    {
        RewriteRun(
            CSharp(
                """
                switch (a)
                {
                    case double.NaN:
                    case double.NaN:
                        Console.WriteLine("Failed measurement.");
                        break;
                    default:
                        return;
                }

                """
            )
        );
    }

}
