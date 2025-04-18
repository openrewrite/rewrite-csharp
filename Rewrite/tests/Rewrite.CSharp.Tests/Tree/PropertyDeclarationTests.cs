using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class PropertyDeclarationTests : RewriteTest
{

    [Test]
    void SimpleAutoPropertyDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                public class MyClass
                {
                    public string TestProp
                    {
                        get;
                        set;
                    }
                }
                """
            )
        );
    }

    [Test]
    void AutoPropertyWithInitializerDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                public class MyClass
                {
                    public string TestProp /*space*/
                    /*space*/ {
                        get  /*space*/   ; // after
                        /*before*/     set;
                        //after
                    }  /*space*/    =   /*space*/    "Hello world"    /*space*/ ; //comment
                }
                """
            )
        );
    }

    [Test]
    void SimpleExplicitPropertyDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class Person
                {
                    private string _name = "N/A";

                    // Declare a Name property of type string:
                    public string Name
                    {
                        /*space*/ get // comment
                        /*space*/   {      // comment
                            return _name;
                       /*space*/  }   // comment

                        // space comment
                        // space comment 2
                        // space comment 3

                          /*space*/       set       // comment
                        {
                            _name = value;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    void ExpressionBodyAccessorsPropertyDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class C
                {
                    int _m;

                    public int M
                    {
                        get => _m;
                        set
                        {
                            _m = value;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    void ExpressionBodyPropertyDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                class M
                {
                    string _n;
                    public string Name => _n != null ? _n : "NA" ;
                }
                """
            )
        );
    }

    [Test]
    void BinaryExpressionProperty()
    {
        var sourceSpec = CSharp(
            """
            class Test
            {
                static bool Is64Bit => true || true;
            }
            """);
        RewriteRun(sourceSpec);
    }

    [Test]
    void UnaryExpressionProperty()
    {
        var sourceSpec = CSharp(
            """
            class Test
            {
                static bool Is64Bit => true /*1*/ ;
            }
            """);
        RewriteRun(sourceSpec);
    }


    [Test]
    void AccessorWithAnnotation()
    {
        var sourceSpec = CSharp(
            """
            public override int Capacity
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.Length;
            }
            """);
        RewriteRun(sourceSpec);
    }
}
