using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class AttributeTests(ITestOutputHelper output) : RewriteTest(output)
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
    public void NamedArgument()
    {
        RewriteRun(
            CSharp(
                """
                [Skip(value: true)]
                public class Foo;
                """
            )
        );
    }

    [Fact]
    public void NamedProperty()
    {
        RewriteRun(
            CSharp(
                """
                [Fact(Skip = "yes")]
                class Foo;
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
