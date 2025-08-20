using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class MethodDeclarationTests : RewriteTest
{
    [Test]
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
                    var ctor = cu.Descendents().OfType<J.MethodDeclaration>().First();
                    ctor.Parameters.Should().HaveCount(1);
                    ctor.Modifiers.Should().BeEmpty();
                    ctor.Body.Should().NotBeNull();
                }
            )
        );
    }

    [Test]
    public void MethodDeclarationWithDefaultParameters()
    {
        RewriteRun(
            CSharp(
                """
                public class C
                {
                    M ( int i = 1 )
                    {
                    }
                }
                """,
                spec => spec.AfterRecipe = cu =>
                {
                    var ctor = cu.Descendents().OfType<J.MethodDeclaration>().First();
                    ctor.Parameters.Should().HaveCount(1);
                    ctor.Modifiers.Should().BeEmpty();
                    ctor.Body.Should().NotBeNull();
                }
            )
        );
    }

    [Test]
    public void MethodWithAttributes()
    {
        RewriteRun(
            CSharp(
                """
                public class Foo
                {
                    [Obsolete]
                    Foo(int i)
                    {
                    }
                }
                """
            )
        );
    }

    [Test]
    public void ConstructorDelegation()
    {
        var src = CSharp(
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
        );
        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Test]
    public void ConstructorDelegation2()
    {
        RewriteRun(CSharp(
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
        ));
    }

    [Test]
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
                    var ctor = cu.Descendents().OfType<J.MethodDeclaration>().First();
                    ctor.Modifiers.Should().Contain(m => m.ModifierType == J.Modifier.Types.Static);
                }
            )
        );
    }

    [Test]
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

    [Test]
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
                    var ctor = cu.Descendents().OfType<Cs.MethodDeclaration>().First();
                    ctor.Parameters.Should().HaveCount(1);
                    var vd = (ctor.Parameters[0] as J.VariableDeclarations)!;
                    vd.Variables.Should().HaveCount(1);
                    var b = vd.Variables[0];
                    b.Initializer.Should().BeOfType(typeof(J.Literal));
                }
            )
        );
    }

    [Test]
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

    [Test]
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

    [Test]
    public void ExplicitInterfaceImplementation()
    {
        RewriteRun(
            CSharp(
                """
                public abstract class C
                {
                    object ICloneable.Clone() => Clone();
                }
                """
            )
        );
    }




    [Test]
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


    [Test]
    public void ArrowLocalFunctionDeclaration()
    {
        RewriteRun(
            CSharp(
                @"
                void M()
                {
                    int LM(int number) => number < 2 ? 1 : 2;
                }
                "
            )
        );
    }

    [Test]
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

    [Test]
    public void AnonymousMethodDeclaration()
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

    [Test]
    public void GenericMethodDeclarationWithSingleTypeConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : System.String;
                }
                """
            )
        );
    }

    [Test]
    public void NoBodyClass()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method() /*1*/ ;
                }
                """
            )
        );
    }

    [Test]
    public void GenericMethodDeclarationWithClassConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() /*1*/ where /*2*/  T /*3*/  : /*4*/  class /*5*/ ;
                }
                """
            )
        );
    }


    [Test]
    public void GenericMethodDeclarationWithNewConstraint()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    void Method<T>() where T : new();
                }
                """
            )
        );
    }

    [Test]
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

    [Test]
    public void WithPointerParameter()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    abstract unsafe bool ByteArrayEquals(byte* bytes1);
                }
                """
            )
        );
    }

    [Test]
    public void AnonymousFunctionDelegate()
    {
        RewriteRun(
            CSharp(
                """
                obj.SomeEvent += delegate   { };
                """
            )
        );
    }

}
