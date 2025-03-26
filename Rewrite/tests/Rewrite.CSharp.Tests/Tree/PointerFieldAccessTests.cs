using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class PointerFieldAccessTests : RewriteTest
{
    [Test]
    void Simple()
    {
        RewriteRun(
            CSharp(
                """
                IntPtr loopHandle = ((uv_stream_t*)this.Handle) -> loop;
                """
            )
        );
    }

}
