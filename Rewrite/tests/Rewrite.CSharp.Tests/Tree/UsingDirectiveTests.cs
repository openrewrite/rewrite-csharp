using Rewrite.RewriteCSharp.Test;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection("C# remoting")]
public class UsingDirectiveTests : RewriteTest
{
    [Fact]
    public void Simple()
    {
        RewriteRun(
            CSharp(
                """
                using System.Collections.Generic;
                
                public class Foo
                {
                    List<int> _l;
                }
                """
            )
        );
    }

    [Fact]
    public void InFileNamespace()
    {
        RewriteRun(
            CSharp(
                """
                namespace Foo;

                using System.Collections.Generic;
                
                class Foo;
                """
            )
        );
    }

    [Fact]
    public void InBlockNamespace()
    {
        RewriteRun(
            CSharp(
                """
                namespace Foo
                {
                    using System.Collections.Generic;
                }
                """
            )
        );
    }

    [Fact]
    public void Global()
    {
        RewriteRun(
            CSharp(
                """
                global using System.Collections.Generic;
                
                public class Foo
                {
                    List<int> _l;
                }
                """
            )
        );
    }

    [Fact]
    public void Static()
    {
        RewriteRun(
            CSharp(
                """
                using static System.Math;
                
                public class Foo
                {
                    double _d = Sqrt(1);
                }
                """
            )
        );
    }

    [Fact]
    public void Alias()
    {
        RewriteRun(
            CSharp(
                """
                using M = System.Math;
                
                public class Foo
                {
                    double _d = M.Sqrt(1);
                }
                """
            )
        );
    }

    [Fact]
    public void AliasUnnamed()
    {
        RewriteRun(
            spec => spec.TypeValidation = new TypeValidation(Unknowns: false), // TODO support tuple types
            CSharp(
                """
                using Person = (string First, string Last);
                
                public class Foo
                {
                    Person _p = ("John", "Doe");
                }
                """
            )
        );
    }
}