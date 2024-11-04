using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UsingTests(ITestOutputHelper output) : RewriteTest
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                class Foo
                {
                    void M()
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("/path/to/your/file.txt", true))
                        {
                            file.WriteLine("Hello World");
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void Multiple()
    {
        RewriteRun(
            CSharp(
                """
                using System.IO;

                class Foo
                {
                    void M()
                    {
                        using (StreamReader reader1 = new StreamReader("file1.txt"), reader2 = new StreamReader("file2.txt"))
                        {
                            // code
                        }
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void Multiple2()
    {
        var src = CSharp(
            """
            using System.IO;

            class Foo
            {
                void M()
                {
                    using /*1%*/ (/*2%*/ StreamReader reader1 = new StreamReader("file1.txt") /*3%*/ )
                    using (StreamReader reader2 = new StreamReader("file2.txt")) /*4%*/
                    {
                        // code
                    }
                }
            }
            """).First();

        var lst = src.Parse<Cs.CompilationUnit>();
        var body = lst.Descendents().OfType<J.MethodDeclaration>().First().Body!;
        _output.WriteLine(lst.ToString());
        lst.ToString().ShouldBeSameAs(src.Before);
    }

    [Fact]
    public void NoBraces()
    {
        RewriteRun(
            CSharp(
                """
                using System.IO;

                class Foo
                {
                    void M()
                    {
                        using /*1%*/ StreamReader reader = new StreamReader("file1.txt")/*2%*/ ;
                    }
                }
                """
            )
        );
    }
}
