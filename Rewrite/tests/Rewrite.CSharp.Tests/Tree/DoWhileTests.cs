using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DoWhileTests : RewriteTest
{
    [Test]
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

    [Test]
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
