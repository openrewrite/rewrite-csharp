using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LockTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    private void LockStatement()
    {
        RewriteRun(
            CSharp(
                """
                lock(this)
                {

                }
                """
            )
        );
    }
    [Fact]
    private void LockNoBlock()
    {
        RewriteRun(
            CSharp(
                """
                lock(this) Console.WriteLine("test");
                """
            )
        );
    }
}
