using FluentAssertions;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class TryTests : RewriteTest
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
                    var cd = (cu.Members[0] as J.ClassDeclaration)!;
                    var md = (cd.Body.Statements[0] as J.MethodDeclaration)!;
                    var @try = (md.Body!.Statements[0] as J.Try)!;
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
                    var cd = (cu.Members[0] as J.ClassDeclaration)!;
                    var md = (cd.Body.Statements[0] as J.MethodDeclaration)!;
                    var @try = (md.Body!.Statements[0] as J.Try)!;
                    var vd = @try.Catches[0].Parameter.Tree;
                    vd.TypeExpression.Should().BeNull();
                    vd.Variables.Should().BeEmpty();
                }
            )
        );
    }
}
