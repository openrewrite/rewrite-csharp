using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class NullableTypeTests : RewriteTest
{
    [Fact]
    void NullableFieldDeclaration()
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

    [Fact]
    void CommentsOnNullableFieldDeclaration()
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
