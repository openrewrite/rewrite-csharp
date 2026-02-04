using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnsafeTests : RewriteTest
{
    [Test]
    public void UnsafeStatement()
    {
        RewriteRun(
            CSharp(
                """
                unsafe
                {

                }
                """
            )
        );
    }

    [Test]
    public void PointerDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        int value = 10;
                        unsafe
                        {
                            int* ptr = &value;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void PointerArithmetic()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        unsafe
                        {
                            int* p1 = null;
                            int* p2 = p1 + 1;
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void PointerDereference()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        int value = 10;
                        unsafe
                        {
                            int* ptr = &value;
                            *ptr = 100;
                        }
                    }
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
    public void SizeOf()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        unsafe
                        {
                            int size = sizeof(int);
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void StackAlloc()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    void M()
                    {
                        unsafe
                        {
                            byte* buffer = stackalloc byte[10];
                        }
                    }
                }
                """
            )
        );
    }

    [Test]
    public void WhitespacePreserved()
    {
        RewriteRun(
            CSharp(
                """
                unsafe  {  }
                """
            )
        );
    }

    [Test]
    public void WithStatements()
    {
        RewriteRun(
            CSharp(
                """
                unsafe
                {
                    int x = 1;
                    x++;
                }
                """
            )
        );
    }

    [Test]
    public void UnsafeInMethod()
    {
        RewriteRun(
            CSharp(
                """
                public class Test
                {
                    unsafe void M()
                    {
                        int* p = null;
                    }
                }
                """
            )
        );
    }

    [Test]
    public void UnsafeClass()
    {
        RewriteRun(
            CSharp(
                """
                public unsafe class Test
                {
                    int* ptr;
                }
                """
            )
        );
    }

    [Test]
    public void UnsafeStruct()
    {
        RewriteRun(
            CSharp(
                """
                public unsafe struct Buffer
                {
                    public fixed byte data[128];
                }
                """
            )
        );
    }
}
