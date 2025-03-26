using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DirectiveTests : RewriteTest
{
    [Test]
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
    [Test]
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
