using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IsPatternTests : RewriteTest
{

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
    private void PropertyNameNested()
    {
        RewriteRun(
            CSharp(
                """
                return organization is { Permissions.ManageUsers: true };
                """
            )
        );
    }
}
