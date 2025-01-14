using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class PointerFieldAccessTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
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
