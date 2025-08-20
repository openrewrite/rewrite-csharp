using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class CollectionExpressionTests : RewriteTest
{
    [Test]`n    public void Space()
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

    [Test]`n    public void Empty()
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

    [Test]`n    public void Multiple()
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

    [Test]`n    public void TrailingComma()
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

    [Test]`n    public void Spread()
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
