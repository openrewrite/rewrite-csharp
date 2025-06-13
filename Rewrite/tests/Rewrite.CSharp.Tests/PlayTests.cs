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
    
    [Test]
    [Explicit]
    public async Task BenchmarkAsync()
    {
        var node = CSharpParser.Instance.Parse(File.ReadAllText(@"C:\Projects\openrewrite\rewrite-csharp\Rewrite\tests\fixtures\server\test\Core.Test\AdminConsole\OrganizationFeatures\Policies\SavePolicyCommandTests.cs"));
        var printer = new CSharpPrinter<object>();
        var asyncPrinter = new CSharpPrinterAsync<object>();

        // var sync = Stopwatch.StartNew();
        // for (int i = 0; i < 100; i++)
        // {
        //     printer.Visit(node, new PrintOutputCapture<object>(new object()));
        // }
        // sync.Stop();
        
        var async = Stopwatch.StartNew();
        var o = new PrintOutputCapture<object>(new object());
        for (int i = 0; i < 100; i++)
        {
            o = new PrintOutputCapture<object>(new object());
            await asyncPrinter.Visit(node, o);
        }
        Console.WriteLine(o.Out.ToString());
        async.Stop();
        
        // Console.WriteLine(sync.Elapsed);
        Console.WriteLine(async.Elapsed);
    }
    
    class TestVisitor : CSharpIsoVisitor<int>
    {
        public override JRightPadded<J2>? VisitRightPadded<J2>(JRightPadded<J2>? right, JRightPadded.Location loc, int p)
        {
            return base.VisitRightPadded(right, loc, p);
        }
    }

}
