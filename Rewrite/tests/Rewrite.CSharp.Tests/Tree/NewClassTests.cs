using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NewClassTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
    public void ImplicitNewClassWithInitializerSpaces()
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

    [Test]
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

    [Test]
    void NewClassWithMultiValueInitializer()
    {
        var src = CSharp(
                """
                new List<int>
                {
                    1,
                    2
                };
                """);
        var lst = src.Parse();
        lst.ToString().ShouldBeSameAs(src.Before);

    }

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
    void ImplicitElementAccess()
    {
        RewriteRun(
            CSharp(
                """
                new Dictionary<int, string>
                {
                    [1] = "one",
                    [2] = "two",
                };
                """
            )
        );
    }

    [Test]
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

    [Test]
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



    [Test]
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

    [Test]
    void DictionaryCreation()
    {
        RewriteRun(
            CSharp(
                """

                var pet = new Dictionary<string, int> {
                    { "test1", 1 },
                    { "test2", 2 },
                };

                """
            )
        );
    }

    [Test]
    void DictionaryDictionaryCreation()
    {
        RewriteRun(
            CSharp(
                """
                var pet = new Dictionary<Dictionary<int, int>, int>
                {
                   {
                       new Dictionary<int, int>
                       {
                           {1, 1}
                       },
                       1
                   },
                };
                """
            )
        );
    }
}
