using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IsPatternTests(ITestOutputHelper output) : RewriteTest(output)
{

    [Fact]
    private void IsType()
    {
        RewriteRun(
            CSharp(
                """
                return o is int;
                """
            )
        );
    }

    [Fact]
    private void IsTypeWithVariable()
    {
        RewriteRun(
            CSharp(
                """
                return a is int i;
                """
            )
        );
    }

    [Fact]
    private void IsTypeWithDiscard()
    {
        RewriteRun(
            CSharp(
                """
                return o is int _;
                """
            )
        );
    }

    [Fact]
    private void IsVarWithTupleDeconstruction()
    {
        RewriteRun(
            CSharp(
                """
                return o is var (a,b);
                """
            )
        );
    }

    [Fact]
    private void IsIntConstant()
    {
        RewriteRun(
            CSharp(
                """
                return o is 1;
                """
            )
        );
    }

    [Fact]
    private void IsStringConstant()
    {
        RewriteRun(
            CSharp(
                """
                return o is "one";
                """
            )
        );
    }

    [Fact]
    private void IsPropertyPattern()
    {
        RewriteRun(
            CSharp(
                """
                return o is { SomeProp: "one" };
                """
            )
        );
    }

    [Fact]
    private void IsVarPattern()
    {
        RewriteRun(
            CSharp(
                """
                return o is var (a,b);
                """
            )
        );
    }

    [Fact]
    private void IsPositional()
    {
        RewriteRun(
            CSharp(
                """
                return o is (a,b);
                """
            )
        );
    }

    [Fact]
    private void IsRelational()
    {
        RewriteRun(
            CSharp(
                """
                return o is > 5;
                """
            )
        );
    }

    [Fact]
    private void IsArray()
    {
        RewriteRun(
            CSharp(
                """
                return o is [1, .., 3];
                """
            )
        );
    }
}
