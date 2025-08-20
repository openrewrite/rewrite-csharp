using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;


public class BaseExpressionTests : RewriteTest
{
    [Test]
    public void BaseExpresion()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    void M()
                    {
                        base.ToString();
                    }
                }
                """
            )
        );
    }
}
