using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CollectionExpressionTests(ITestOutputHelper output) : RewriteTest(output)
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
                a[..i];
                }
                """
            )
        );
    }
}
