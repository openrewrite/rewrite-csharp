using Rewrite.Test;
using Rewrite.Test.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Rewrite.Recipes;

using static Assertions;

[Collection("C# remoting")]
public class Tests(ITestOutputHelper output) : RewriteTest(output)
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new InvertAssertion();
    }

    [Fact]
    public void VerifyItWorksTest()
    {
        RewriteRun(
            CSharp(
                """
                    class MyClass
                    {
                        void test()
                        {
                            bool a = false;
                            Assert.True(!a);
                        }
                    }             
                """,
                """
                    class MyClass
                    {
                        void test()
                        {
                            bool a = false;
                            Assert.False(a);
                        }
                    }             
                """
            )
        );
    }
}
