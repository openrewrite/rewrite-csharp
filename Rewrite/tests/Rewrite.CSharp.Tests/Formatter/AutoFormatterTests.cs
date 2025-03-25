using Rewrite.RewriteCSharp.Format;

namespace Rewrite.CSharp.Tests.Formatter;
using static Assertions;

public class AutoFormatterTests(ITestOutputHelper output) : RewriteTest(output)
{
    protected override void Defaults(RecipeSpec spec)
    {
        spec.Recipe = new AutoFormatRecipe();
    }

    [Fact]
    public void FormatBasic()
    {
        RewriteRun(CSharp(
            before: """
                public class Foo { public int   A {    get;  set;   } }
                
                """,
            after: """
                   public class Foo
                   {
                       public int A { get; set; }
                   }
                   """));
    }

    [Fact]
    public void PartialFormat()
    {
        var spec = CSharp(
            before: """
                    public class Foo{
                    public int   A {    get;  set;   }
                    }
                    """,
            after: """
                   public class Foo{
                       public int A { get; set; }
                   }
                   """);
        var beforeLst = CSharpParser.Instance.Parse(spec.Before!);
        var formatTarget = beforeLst.Descendents().OfType<Cs.PropertyDeclaration>().First()!;
        var visitor = new AutoFormatVisitor<int>(formatTarget);
        var afterLst = visitor.Visit(beforeLst, 0)!;
        var actual = afterLst.ToString();
        var expected = spec.After;
        actual.ShouldBeSameAs(expected);

    }
}
