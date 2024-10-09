using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class AttributeTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                [Serializable]
                public class Foo
                {
                    void M([Obsolete] int x)
                    {
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void TrailingComma()
    {
        RewriteRun(
            CSharp(
                """
                [Serializable /*0*/, /*1*/ ]
                public class Foo;
                """
            )
        );
    }

    [Fact]
    public void Multiple()
    {
        RewriteRun(
            CSharp(
                """
                [Serializable, Serializable]
                public class Foo;
                """
            )
        );
    }

    [Fact]
    public void Target()
    {
        RewriteRun(
            CSharp(
                """
                [type : Serializable]
                public class Foo;
                """
            )
        );
    }

    [Fact]
    public void CompilationUnit()
    {
        RewriteRun(
            CSharp(
                """
                using System;
                using System.Reflection;
                [assembly: AssemblyTitleAttribute("Production assembly 4")]
                [module: CLSCompliant(true)]
                """
            )
        );
    }
}
