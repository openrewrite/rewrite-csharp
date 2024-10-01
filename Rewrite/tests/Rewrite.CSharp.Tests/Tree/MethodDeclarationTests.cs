using FluentAssertions;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class MethodDeclarationTests : RewriteTest
{
    [Fact]
    public void Constructor()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    Foo(int i)
                    {
                    }
                }
                """,
                spec => spec.AfterRecipe = cu =>
                {
                    var cd = (cu.Members[0] as J.ClassDeclaration)!;
                    var ctor = (cd.Body.Statements[0] as J.MethodDeclaration)!;
                    ctor.Parameters.Should().HaveCount(1);
                    ctor.Modifiers.Should().BeEmpty();
                    ctor.Body.Should().NotBeNull();
                }
            )
        );
    }

    [Fact]
    public void ConstructorDelegation()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                """
                public class Foo
                {
                    Foo() : this(1)
                    {
                    }
                    Foo(int i)
                    {
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void StaticConstructor()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    static Foo()
                    {
                    }
                }
                """,
                spec => spec.AfterRecipe = cu =>
                {
                    var cd = (cu.Members[0] as J.ClassDeclaration)!;
                    var ctor = (cd.Body.Statements[0] as J.MethodDeclaration)!;
                    ctor.Modifiers.Should().Contain(m => m.ModifierType == J.Modifier.Type.Static);
                }
            )
        );
    }

    [Fact]
    public void NoParamsVoidMethodDeclaration()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public void Test()
                    {
                        // hello world
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void ParamWithDefault()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    public void M(bool b = true)
                    {
                    }
                }
                """,
                spec => spec.AfterRecipe = cu =>
                {
                    var cd = (cu.Members[0] as J.ClassDeclaration)!;
                    var ctor = (cd.Body.Statements[0] as J.MethodDeclaration)!;
                    ctor.Parameters.Should().HaveCount(1);
                    var vd = (ctor.Parameters[0] as J.VariableDeclarations)!;
                    vd.Variables.Should().HaveCount(1);
                    var b = vd.Variables[0];
                    b.Initializer.Should().BeOfType(typeof(J.Literal));
                }
            )
        );
    }

    [Fact]
    public void VarargsParameter()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M(params bool bs)
                    {
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void OverrideModifier()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    public override string ToString()
                    {
                        return "Foo" ;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ArrowSyntaxMethodDeclaration()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public string Test() /*123*/=>  /*123*/  ""Foo""  /*123*/ ;
                }
                "
            )
        );
    }

    [Fact]
    public void ArrowLocalFunctionDeclaration()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public static int LocalFunctionFactorial(int n)
                    {
                        return nthFactorial(n);

                        int nthFactorial(int number) => number < 2
                            ? 1
                            : number * nthFactorial(number - 1);
                    }
                }
                "
            )
        );
    }

    [Fact]
    public void LocalFunctionDeclaration()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public static int LocalFunctionFactorial(int n)
                    {
                        return nthFactorial(n);

                        int nthFactorial(int number)
                            {
                                return number < 2
                                        ?  1
                                        : number * nthFactorial(number - 1);
                            }
                    }
                }
                "
            )
        );
    }

    [Fact]
    void AnonymousMethodDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    System.Func<double, double> anonymousM = delegate(double val) {
                        return val;
                    };
                }
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    void GenericMethodDeclarationWithSingleTypeConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : System.String
                }
                """
            )
        );
    }



    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void GenericMethodDeclarationWithClassConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : class
                }
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void GenericMethodDeclarationWithEnumConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : enum
                }
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void GenericMethodDeclarationWithNewConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : new()
                }
                """
            )
        );
    }

    [Fact(Skip = SkipReason.NotYetImplemented)]
    public void TypeParameterWithMultipleConstraints()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : IList<int>, IEnumerable<int>, new();
                }
                """
            )
        );
    }
}
