using System.Diagnostics;
using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class AliasTests: RewriteTest
{
    [Test]
    public void AliasInvocation()
    {
        RewriteRun(
            CSharp(
                """
                using C = System.Console;

                /*1*/C/*2*/::/*3*/WriteLine("Hello");
                """
            )
        );
    }

    [Test]
    public void AliasMember()
    {
        RewriteRun(
            CSharp(
                """
                using C = System.String;

                /*1*/C/*2*/::/*3*/Empty;
                """
            )
        );
    }

    [Test]
    public void GlobalAlias()
    {
        RewriteRun(
            CSharp(
                """
                global ::  Something();
                """
            )
        );
    }
}
