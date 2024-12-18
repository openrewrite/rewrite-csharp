namespace Rewrite.CSharp.Tests;

[Exploratory]
public class PlayTests(ITestOutputHelper output) : RewriteTest(output)
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
    [Exploratory]
    public void MyTest()
    {
        var root = new CSharpParser.Builder().Build().Parse(
                """
                /*1*/
                a/*2*/
                .FirstOrDefault()/*3*/?/*4*/
                ./*5*/GetCustomAttribute<D>(1)/*6*/?/*7*/./*8*/Name;
                """
        );
        if (root is ParseError)
        {

        }
        _output.WriteLine(root.ToString());
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
