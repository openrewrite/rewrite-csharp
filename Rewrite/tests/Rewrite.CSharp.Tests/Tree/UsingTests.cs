using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class UsingTests : RewriteTest
{
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
                        using StreamReader reader = new StreamReader("file1.txt");
                    }
                }
                """
            )
        );
    }
}