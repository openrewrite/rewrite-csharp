using Rewrite.Test;
using Rewrite.Test.CSharp;
using TUnit.Core;

namespace Rewrite.Recipes;

using static Assertions;

public class RecipesTests : RewriteTest
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new InvertAssertion();
    }

    [Test]
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
