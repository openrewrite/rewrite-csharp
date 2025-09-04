using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class FixedTests : RewriteTest
{
    [Test]
    public void FixedBlock()
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
    public void FixedStatement()
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
    public void FixedStatement2()
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
    public void NestedFixedStatement()
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
