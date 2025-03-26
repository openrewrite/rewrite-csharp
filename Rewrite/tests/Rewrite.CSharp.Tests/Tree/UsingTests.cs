using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UsingStatementTests : RewriteTest
{
    [Test]
    public void BasicUsing()
    {
        RewriteRun(
            CSharp(
                """
                using (var file = File.OpenRead("test.txt"))
                {
                    // do something
                }
                """
            )
        );
    }

    [Test]
    public void UsingDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                using var file = File.OpenRead("test.txt");
                // rest of the code
                """
            )
        );
    }

    [Test]
    public void MultipleResources()
    {
        RewriteRun(
            CSharp(
                """
                using (var reader = new StreamReader("test.txt"))
                using (var writer = new StreamWriter("output.txt"))
                {
                    // do something
                }
                """
            )
        );
    }

    [Test]
    public void UsingWithoutBraces()
    {
        RewriteRun(
            CSharp(
                """
                using (var stream = new MemoryStream())
                    stream.WriteByte(0);
                """
            )
        );
    }

    [Test]
    public void NestedUsing()
    {
        RewriteRun(
            CSharp(
                """
                using (var outer = new MemoryStream())
                {
                    using (var inner = new MemoryStream())
                    {

                    }
                }
                """
            )
        );
    }

    [Test]
    public void UsingWithMultipleDeclarations()
    {
        RewriteRun(
            CSharp(
                """
                using (Stream stream = File.OpenRead("input.txt"), output = File.Create("output.txt"))
                {
                    // do something with both streams
                }
                """
            )
        );
    }

    [Test]
    public void UsingWithExplicitType()
    {
        RewriteRun(
            CSharp(
                """
                using (Stream stream = new MemoryStream())
                {
                    // explicit type declaration
                }
                """
            )
        );
    }

    [Test]
    public void UsingWithNull()
    {
        RewriteRun(
            CSharp(
                """
                using (Stream stream = null)
                {
                    // using with null resource
                }
                """
            )
        );
    }

    [Test]
    public void UsingWithAwait()
    {
        RewriteRun(
            CSharp(
                """
                  await using (var resource = await GetResourceAsync())
                {
                    // async operations
                }
                """
            )
        );
    }

    [Test]
    public void UsingDeclarationWithAwait()
    {
        RewriteRun(
            CSharp(
                """
                int a;
                  await using var resource = await GetResourceAsync();
                // rest of async code
                """
            )
        );
    }
}
