using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Test.Api;
using Rewrite.Test;
using Xunit.Abstractions;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

[Collection(Collections.PrinterAccess)]
public class PlayTests(ITestOutputHelper _output) : RewriteTest
{
    [Fact(Skip = "Ignored")]
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

    [Fact(Skip = "Ignored")]
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
