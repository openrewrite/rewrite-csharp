using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Rewrite.Core.Marker;

namespace Rewrite.CSharp.Tests;

[Exploratory]
public class PlayTests : RewriteTest
{
    /// <summary>
    /// Some pretty print tests for string delta report
    /// </summary>
    [Test]
    [Explicit]
    [Arguments("abc\n123", "abc123")]
    [Arguments("abc123", "abc134")]
    [Arguments("""
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

    [Test]
    [Explicit]
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


    [Test]
    [Explicit] 
    public void TestCursor()
    {
        var node = CSharpParser.Instance.Parse(
            """
            public class Foo
            {
                int _field = 1;
            }
            """
        );
        var visitor = new TestVisitor();
        visitor.Visit(node, 0);
    }
    
    class TestVisitor : CSharpIsoVisitor<int>
    {
        public override JRightPadded<J2>? VisitRightPadded<J2>(JRightPadded<J2>? right, JRightPadded.Location loc, int p)
        {
            return base.VisitRightPadded(right, loc, p);
        }
    }

}
