using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ReturnTests : RewriteTest
{
    [Test]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                @"
                public class Foo
                {
                    public int M()
                    {
                        return 1;
                    }
                }
                "
            )
        );
    }

    [Test]
    public void Void()
    {
        RewriteRun(
            CSharp(
                @"
                public class T
                {
                    public void M()
                    {
                        return;
                    }
                }
                "
            )
        );
    }

    [Test]
    public void MethodCall()
    {
        RewriteRun(
            CSharp(
                @"

                int M()
                {
                    return PlatformDependent0.ByteArrayEquals(array1, array2, length);
                }

                "
            )
        );
    }
}
