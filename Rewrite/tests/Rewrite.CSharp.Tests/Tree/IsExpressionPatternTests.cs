using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IsPatternTests : RewriteTest
{

    [Test]`n    public void IsType()
    {
        RewriteRun(
            CSharp(
                """
                return o is int;
                """
            )
        );
    }

    [Test]`n    public void IsTypeWithVariable()
    {
        RewriteRun(
            CSharp(
                """
                return a is int i;
                """
            )
        );
    }

    [Test]`n    public void IsTypeWithDiscard()
    {
        RewriteRun(
            CSharp(
                """
                return o is int _;
                """
            )
        );
    }

    [Test]`n    public void IsVarWithTupleDeconstruction()
    {
        RewriteRun(
            CSharp(
                """
                return o is var (a,b);
                """
            )
        );
    }

    [Test]`n    public void IsIntConstant()
    {
        RewriteRun(
            CSharp(
                """
                return o is 1;
                """
            )
        );
    }

    [Test]`n    public void IsStringConstant()
    {
        RewriteRun(
            CSharp(
                """
                return o is "one";
                """
            )
        );
    }

    [Test]`n    public void IsPropertyPattern()
    {
        RewriteRun(
            CSharp(
                """
                return o is { SomeProp: "one" };
                """
            )
        );
    }

    [Test]`n    public void IsVarPattern()
    {
        RewriteRun(
            CSharp(
                """
                return o is var (a,b);
                """
            )
        );
    }

    [Test]`n    public void IsPositional()
    {
        RewriteRun(
            CSharp(
                """
                return o is (a,b);
                """
            )
        );
    }

    [Test]`n    public void IsRelational()
    {
        RewriteRun(
            CSharp(
                """
                return o is > 5;
                """
            )
        );
    }

    [Test]`n    public void IsArray()
    {
        RewriteRun(
            CSharp(
                """
                return o is [1, .., 3];
                """
            )
        );
    }

    [Test]`n    public void PropertyNameNested()
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
