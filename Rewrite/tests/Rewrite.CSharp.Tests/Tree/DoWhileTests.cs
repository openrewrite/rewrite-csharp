using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DoWhileTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void BasicDoWhile()
    {
        RewriteRun(
            CSharp(
                """
                do { } while ( true ) ;
                """
            )
        );
    }

    [Fact]
    public void NonBlockStatement()
    {
        RewriteRun(
            CSharp(
                """
                do Console.WriteLine("test"); while ( true ) ;
                """
            )
        );
    }

}
