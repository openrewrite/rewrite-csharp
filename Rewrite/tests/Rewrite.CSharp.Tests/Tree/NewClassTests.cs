using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class NewClassTests : RewriteTest
{
    [Fact]
    void SimpleNewClassCase()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    private Test t = new Test();
                }
                """
            )
        );
    }

    [Fact]
    void SimpleNewClassSpaces()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    private Test t = /*1*/  new /*123124*/    Test   /*12312*/   (    /*123*/  ) ;
                }
                """
            )
        );
    }

    [Fact]
    void SimpleNewNestedClassCase()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    class Nested
                    {
                        //Nested() { }
                    }

                    private Container.Nested cn = new Container.Nested();
                }
                """
            )
        );
    }

    [Fact]
    void SimpleNewNestedClassSpaces()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    class Nested
                    {
                        //Nested() { }
                    }

                    private Container.Nested cn = new Container /*12*/.   Nested(/*1233*/)   ; // asda
                }
                """
            )
        );
    }

    [Fact]
    void SimpleNewNestedParametrizedClassSpaces()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    class Nested<T, X>
                    {
                        //Nested() { }
                    }

                    private Container.Nested cn = new Container /*12*/.   Nested   /*1233*/ < /*1233*/      double  /*1233*/   , /*1233*/   double> (/*1233*/)   ; // asda
                }
                """
            )
        );
    }

    [Fact]
    void NewClassWithEmptyInitializer()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    private Container c = new Container { };
                }
                """
            )
        );
    }

    [Fact]
    void ImplicitNewClassWithInitializerSpaces()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    public string? Type;
                    public string? Type2;
                    private Container c = new /*asdas*/   () /*asdas*/   { //asdas
                        //asdas
                        Type = "123", //123
                        Type2 = "123", //123
                        /*asda*/}; //asdas
                }
                """
            )
        );
    }

    [Fact]
    void NewClassWithInitializer()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    public string? Type;
                    private Container c = new Container {
                        Type = "123",
                    };
                }
                """
            )
        );
    }

    [Fact]
    void NewClassWithInitializerSpaces()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    public string? Type;
                    private Container c = new Container /*asdas*/{ //asdas
                        //asdas
                        Type = "123", //123
                    /*asda*/}; //asdas
                }
                """
            )
        );
    }

    [Fact]
    void Generics()
    {
        RewriteRun(
            CSharp(
                """
                class Foo
                {
                    object _l = new System.Collections.Generic.List<int>();
                }
                """
            )
        );
    }

    [Fact]
    void ImplicitNewClass()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    public string? Type;
                    private Container c = new /*asdas*/   () /*asdas*/  ; //asdas
                }
                """
            )
        );
    }

    [Fact]
    void AnonymousObjectCreation()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    public void TestMethod() {
                        var pet = new { Age = 10, Name = "Fluffy" };
                    }
                }
                """
            )
        );
    }

    [Fact]
    void ComplexAnonymousObjectCreation()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    public void TestMethod()
                    {
                        var age = 10;
                        var name = "Fluffy";
                        var pet = new { age, name };
                    }
                }
                """
            )
        );
    }



    [Fact]
    void ComplexAnonymousObjectCreationWithFieldAccess()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    class ClassWithProps
                    {
                        int Age = 10;
                        string Name = "Test";
                    }
                    public void TestMethod()
                    {
                        var cls = new ClassWithProps();
                        var pet = new { cls.Age, cls.Name };
                    }
                }
                """
            )
        );
    }

    [Fact]
    void DictionaryCreation()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    public void Test() {
                        var pet = new Dictionary<string, int> {
                            { "test1", 1 },
                            { "test2", 2, },
                        };
                    }
                }
                """
            )
        );
    }

    [Fact]
    void DictionaryDictionaryCreation()
    {
        RewriteRun(
            CSharp(
                """
                class T
                {
                    void M()
                    {
                        var pet = new Dictionary<Dictionary<int, int>, int> {
                           { new Dictionary<int, int>
                               {
                                   {1, 1}
                               },
                               1
                           },
                        };
                    }
                }
                """
            )
        );
    }
}
