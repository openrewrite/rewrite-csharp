using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class FixedTests : RewriteTest
{
    [Test]
    private void FixedBlock()
    {
        RewriteRun(
            CSharp(
                """
                fixed(int* p1 = 0)
                {

                }
                """
            )
        );
    }

    [Test]
    private void FixedStatement()
    {
        RewriteRun(
            CSharp(
                """

                fixed(int* p1 = 0)
                    return p1;
                """
            )
        );
    }
    [Test]
    private void FixedStatement2()
    {
        RewriteRun(
            CSharp(
                """
                public bool M()
                {

                       /*1*/fixed (byte* arr = &b[s])
                            /*2*/return/*3*/a/*4*/;
                }
                """
            )
        );
    }

    [Test]
    private void NestedFixedStatement()
    {
        RewriteRun(
            CSharp(
                """
                public bool M()
                {
                    fixed(int* p1 = 0)
                       /*1*/fixed (byte* arr = &b[s])
                            /*2*/return/*3*/a/*4*/;
                }
                """
            )
        );
    }
}
