using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LockTests : RewriteTest
{
    [Test]
    public void LockStatement()
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
    [Test]
    public void LockNoBlock()
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
