using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class TryTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void TryFinallyOnly()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    void M()
                    {
                        try
                        {
                        }
                        finally
                        {
                        }
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void RegularCatch()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    void M()
                    {
                        try
                        {
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void CatchWithoutVariableName()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    void M()
                    {
                        try
                        {
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var @try = cu.Descendents().OfType<J.Try>().First();
                    var vd = @try.Catches[0].Parameter.Tree;
                    vd.TypeExpression.Should().NotBeNull();
                    vd.Variables.Should().BeEmpty();
                }
            )
        );
    }

    [Fact]
    public void CatchAll()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    void M()
                    {
                        try
                        {
                        }
                        catch
                        {
                        }
                    }
                }
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var @try = cu.Descendents().OfType<J.Try>().First();
                    var vd = @try.Catches[0].Parameter.Tree;
                    vd.TypeExpression.Should().BeNull();
                    vd.Variables.Should().BeEmpty();
                }
            )
        );
    }
}
