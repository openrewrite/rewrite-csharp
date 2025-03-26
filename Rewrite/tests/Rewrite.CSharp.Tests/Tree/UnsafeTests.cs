using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class UnsafeTests : RewriteTest
{
    [Test]
    private void UnsafeStatement()
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

}
