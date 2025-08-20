using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class AttributeTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
    public void NamedArgumentWithColonSpecifier()
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

    [Test]
    public void NamedArgumentWithPropertySpecifier()
    {
        RewriteRun(
            CSharp(
                """
                [Skip(Value = true)]
                public class Foo;
                """
            )
        );
    }

    [Test]
    public void NamedProperty()
    {
        RewriteRun(
            CSharp(
                """
                [Fact(Skip = "yes")]`n    public class Foo;
                """
            )
        );
    }

    [Test]
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
