using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class StackAllocTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
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
    [Fact]
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

    [Fact]
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

    [Fact]
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
