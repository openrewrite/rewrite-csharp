using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NullableTypeTests : RewriteTest
{
    [Test]
    public void NullableFieldDeclaration()
    {

        RewriteRun(
            CSharp(
                """
                class Test {
                    object? nullableVal;
                }
                """
            )
        );
    }

    [Test]
    public void CommentsOnNullableFieldDeclaration()
    {

        RewriteRun(
            CSharp(
                """
                class Test {
                    private /*test*/ object/*test*/? /*test*/ nullableVal;
                }
                """
            )
        );
    }
}
