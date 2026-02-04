using Rewrite.Test.CSharp;
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
    public void FixedStatementWithComments()
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

    [Test]
    public void FixedWithArray()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M(int[] array)
                    {
                        fixed (int* p = array)
                        {
                            *p = 42;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void FixedWithString()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M(string str)
                    {
                        fixed (char* p = str)
                        {
                            char c = *p;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void FixedWithMultiplePointers()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M(byte[] arr1, byte[] arr2)
                    {
                        fixed (byte* p1 = arr1, p2 = arr2)
                        {
                            byte b = (byte)(*p1 + *p2);
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void FixedWithAddressOf()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M()
                    {
                        int value = 10;
                        fixed (int* p = &value)
                        {
                            *p = 20;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void FixedWhitespacePreserved()
    {
        RewriteRun(
            CSharp(
                """
                fixed  (  int*  p  =  0  )  {  }
                """
            )
        );
    }

    [Test]
    public void FixedInsideUnsafe()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M(byte[] bytes)
                    {
                        unsafe
                        {
                            fixed (byte* ptr = bytes)
                            {
                                byte b = *ptr;
                            }
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void FixedWithStackalloc()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M()
                    {
                        int* p = stackalloc int[10];
                        fixed (int* ptr = &p[0])
                        {
                            *ptr = 1;
                        }
                    }
                }
                """
            )
        );
    }
}
