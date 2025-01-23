using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DirectiveTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    void ConditionalPragma()
    {
        RewriteRun(
          CSharp(
            """
            #if NET40
            a();
            #endif
            """
          )
        );
    }
    [Fact]
    void NullableDirective()
    {
        RewriteRun(
            CSharp(
                """
                #nullable enable
                int a;
                """
            )
        );
    }
}
