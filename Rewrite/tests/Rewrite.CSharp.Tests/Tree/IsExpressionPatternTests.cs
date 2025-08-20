using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IsPatternTests : RewriteTest
{

    [Test]
    public void IsType()
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
    public void IsTypeWithVariable()
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
    public void IsTypeWithDiscard()
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
    public void IsVarWithTupleDeconstruction()
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
    public void IsIntConstant()
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
    public void IsStringConstant()
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
    public void IsPropertyPattern()
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
    public void IsVarPattern()
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
    public void IsPositional()
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
    public void IsRelational()
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
    public void IsArray()
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
    public void PropertyNameNested()
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
