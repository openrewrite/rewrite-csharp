using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class SwitchExpressionTests : RewriteTest
{
    [Test]
    public void Basic()
    {
        RewriteRun(
            CSharp(
                """
                number switch
                {
                    1 => "One",
                    _ => "Other"
                };
                """
            )
        );
    }

    [Test]`n    public void TuplePattern()
    {
        RewriteRun(
            CSharp(
                """
                point switch
                {
                    (0, 0) => "Origin",
                    (_, 0) => "On the X-axis",
                };
                """
            )
        );
    }

    [Test]`n    public void TypePattern()
    {
        RewriteRun(
            CSharp(
                """
                obj switch
                {
                    int i => "int",
                };
                """
            )
        );
    }

    [Test]`n    public void RangePattern()
    {
        RewriteRun(
            CSharp(
                """
                score switch
                {
                    >= 90 => "A",
                    > 80 => "B",
                    < 70 => "C",
                    <= 60 => "D",
                };
                """
            )
        );
    }

    [Test]`n    public void TypePatternWithWhenClause()
    {
        RewriteRun(
            CSharp(
                """
                obj switch
                {
                    int i when i > 0 => "Positive integer",
                };
                """
            )
        );
    }

    [Test]`n    public void PropertyPattern()
    {
        RewriteRun(
            CSharp(
                """
                obj switch
                {
                    { Type: "circle" } => "Positive integer"
                };
                """
            )
        );
    }

    [Test]`n    public void PropertyPatternWithDesignator()
    {
        RewriteRun(
            CSharp(
                """
                obj switch
                {
                    { Type: "circle" } b => "Positive integer"
                };
                """
            )
        );
    }

    [Test]`n    public void PositionalPattern()
    {
        RewriteRun(
            CSharp(
                """
                point switch
                {
                    (0, 0) => "Origin",
                    (0, _) => "On the Y-axis",
                };
                """
            )
        );
    }

    [Test]`n    public void NullCheckPattern()
    {
        RewriteRun(
            CSharp(
                """
                string input = null;
                string result = input switch
                {
                    null => "Input was null",
                };
                """
            )
        );
    }

    [Test]`n    public void MultiplePatternsUsingOr()
    {
        RewriteRun(
            CSharp(
                """
                letter switch
                {
                    'a' or 'e' or 'i' => "Vowel"
                };
                """
            )
        );
    }

    [Test]`n    public void NestedPatterns()
    {
        RewriteRun(
            CSharp(
                """
                var b = a switch
                {
                    1 => "One",
                    _ => a switch
                    {
                        4 => "Thursday",
                    }
                };
                """
            )
        );
    }

    [Test]`n    public void RecursivePatterns()
    {
        RewriteRun(
            CSharp(
                """
                string DayOfWeek(int day) => day switch
                {
                    1 => "Monday",
                    _ => DayOfWeek(day - 3)
                };
                """
            )
        );
    }

    [Test]`n    public void PatternMatchingWithRecords()
    {
        RewriteRun(
            CSharp(
                """
                public record Animal(string Species, int Age);
                Animal animal = new("Dog", 5);
                string description = animal switch
                {
                    { Species: "Dog", Age: < 3 } => "Puppy",
                    { Species: "Dog", Age: >= 3 } => "Adult Dog",
                    _ => "Unknown Animal"
                };
                """
            )
        );
    }
}
