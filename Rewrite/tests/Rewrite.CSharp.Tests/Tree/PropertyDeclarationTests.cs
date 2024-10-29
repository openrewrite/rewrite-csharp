using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class PropertyDeclarationTests : RewriteTest
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
                public class Date
                {
                    private int _month = 7;  // Backing store

                    public int Month
                    {
                        get => _month    /*space*/   ; // comment
                        set// comment store
                        /*123*/  {  // comment store
                            if ((value > 0) && (value < 13))
                            {
                                _month = value;
                          /* comment store*/  } // comment store
                        }    // comment store
                        // comment store
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
                class Manager
                {
                    private string _name;
                    public string Name => _name != null ? _name : "NA"   /*space*/  ;   /// commment
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
            """).First();
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
            """).First();
        RewriteRun(sourceSpec);
    }

}
