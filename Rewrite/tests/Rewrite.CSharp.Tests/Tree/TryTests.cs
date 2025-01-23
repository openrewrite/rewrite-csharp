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
                try
                {
                }
                finally
                {
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
                try
                {
                }
                catch (Exception e)
                {
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
                try
                {
                }
                catch (Exception)
                {
                }
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var @try = cu.Descendents().OfType<Cs.Try>().First();
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
                try
                {
                }
                catch
                {
                }
                ",
                spec => spec.AfterRecipe = cu =>
                {
                    var @try = cu.Descendents().OfType<Cs.Try>().First();
                    var vd = @try.Catches[0].Parameter.Tree;
                    vd.TypeExpression.Should().BeNull();
                    vd.Variables.Should().BeEmpty();
                }
            )
        );
    }

    [Fact]
    public void CatchWithWhenFilter()
    {
        RewriteRun(
            CSharp(
                """
                try
                {
                }
                catch(Exception e) when (e.Message.StartWith("blah"))
                {
                }
                """
            )
        );
    }
}
