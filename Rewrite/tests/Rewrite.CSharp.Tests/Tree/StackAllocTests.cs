using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class StackAllocTests : RewriteTest
{
    [Test]
    public void BasicStackAlloc()
    {
        RewriteRun(
            CSharp(
                """
                stackalloc int[3];
                """
            )
        );
    }
    [Test]
    public void StackAllocWithInitializer()
    {
        RewriteRun(
            CSharp(
                """
                stackalloc int[] { 1,2,3 };
                """
            )
        );
    }

    [Test]
    public void ImplicitStackAlloc()
    {
        RewriteRun(
            CSharp(
                """
                stackalloc[ ] { 1,2,3 };
                """
            )
        );
    }

    [Test]
    public void VariableAssignedStackAlloc()
    {
        RewriteRun(
            CSharp(
                """
                char* chars = stackalloc char[charCount];
                """
            )
        );
    }
}
