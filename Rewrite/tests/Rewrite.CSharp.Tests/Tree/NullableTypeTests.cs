using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class NullableTypeTests : RewriteTest
{
    [Test]`n    public void NullableFieldDeclaration()
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

    [Test]`n    public void CommentsOnNullableFieldDeclaration()
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
