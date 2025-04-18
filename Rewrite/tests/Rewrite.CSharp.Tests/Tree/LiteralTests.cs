using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LiteralTests : RewriteTest
{
    [Test]
    private void Boolean()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    bool _t = true;
                    bool _f = false;
                }
                """
            )
        );
    }

    [Test]
    private void Number()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    byte _b = 1;
                    sbyte _sb = 1;
                    short _s = 1;
                    ushort _us = 1;
                    int _i = 1;
                    uint _ui = 1;
                    long _l = 1L;
                    ulong _ul = 1UL;
                    float _f = 1.23f;
                    double _d = 1.23d;
                    decimal _de = 1.23m;
                }
                """
            )
        );
    }

    [Test]
    private void NativeInts()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    nint _ni = 1;
                    nuint _nui = 1;
                }
                """
            )
        );
    }

    [Test]
    private void Null()
    {
        RewriteRun(
            CSharp(
                """
                public class T
                {
                    object _o = null;
                }
                """,
                spec: spec => spec.AfterRecipe = cu =>
                {
                    var o = cu.Descendents().OfType<J.VariableDeclarations>().First();
                    o.Should().NotBeNull();
                    var nullLiteral = (J.Literal)o.Variables[0].Initializer!;
                    nullLiteral.Type.Should().BeOfType<JavaType.Primitive>();
                    nullLiteral.Type.Kind.Should().Be(JavaType.Primitive.PrimitiveType.Null);
                }
            )
        );
    }
}
