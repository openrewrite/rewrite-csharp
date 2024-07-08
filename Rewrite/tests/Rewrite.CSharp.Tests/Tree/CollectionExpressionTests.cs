using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class CollectionExpressionTests : RewriteTest
{
    [Fact]
    void Space()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int[] _i = [ /*0*/ i /*1*/ ];
                }
                """
            )
        );
    }

    [Fact]
    void Empty()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int[] _i = [ /*0*/ ];
                }
                """
            )
        );
    }

    [Fact]
    void Multiple()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int[] _i = [ 1 , 2 , 3 ];
                }
                """
            )
        );
    }

    [Fact]
    void TrailingComma()
    {
        RewriteRun(
            CSharp(
                """
                class Test {
                    int[] _i1 = [ 1 , /*1*/ ];
                }
                """
            )
        );
    }

    [Fact]
    void Spread()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false),
            CSharp(
                """
                class Test {
                    int[] M(int[] i) {
                        return [..i];
                    }
                }
                """
            )
        );
    }
}