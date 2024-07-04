using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class LiteralTests : RewriteTest
{
    [Fact]
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

    [Fact]
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

    [Fact]
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
}