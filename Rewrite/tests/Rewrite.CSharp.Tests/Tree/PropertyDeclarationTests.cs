using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class PropertyDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
{

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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

}
