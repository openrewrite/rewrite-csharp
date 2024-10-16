﻿using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.Test.CSharp;
using Rewrite.Test;
using Xunit.Abstractions;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
[Exploratory]
public class PlayTests(ITestOutputHelper _output) : RewriteTest
{
    /// <summary>
    /// Some pretty print tests for string delta report
    /// </summary>
    [Theory]
    [Exploratory]
    [InlineData("abc\n123","abc123")]
    [InlineData("abc123","abc134")]
    [InlineData("""
                one
                two
                three
                four
                NOT five
                """,
        """
        one
        two
        four
        five
        six
        """)]
    public void Delta(string before, string after)
    {
        after.ShouldBeSameAs(before);
    }

    [Fact]
    public void MyTest()
    {
        var root = new CSharpParser.Builder().Build().Parse(
                """
                public class Foo
                {
                    void Bar()
                }
                """
        );
        var methodDeclaration = root.Descendents().OfType<J.MethodDeclaration>().First();
        var newRoot = root.ReplaceNode(methodDeclaration, methodDeclaration.WithName(methodDeclaration.Name.WithSimpleName("Hello")));
        _output.WriteLine(newRoot.ToString());
    }

    [Fact]
    public void ParseTests()
    {
        var root = new CSharpParser.Builder().Build().Parse(
            """
            public class Foo
            {
                void Main()
                {
                    a.Hello().There;
                }
            }
            """
        );
        var node = root.Descendents().OfType<J.MethodDeclaration>().First().Body!.Statements[0];
        _output.WriteLine(node.ToString());
    }
}
