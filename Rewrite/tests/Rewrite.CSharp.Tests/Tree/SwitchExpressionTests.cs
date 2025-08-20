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

    [Test]
    public void TuplePattern()
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

    [Test]
    public void TypePattern()
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

    [Test]
    public void RangePattern()
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

    [Test]
    public void TypePatternWithWhenClause()
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

    [Test]
    public void PropertyPattern()
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

    [Test]
    public void PropertyPatternWithDesignator()
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

    [Test]
    public void PositionalPattern()
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

    [Test]
    public void NullCheckPattern()
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

    [Test]
    public void MultiplePatternsUsingOr()
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

    [Test]
    public void NestedPatterns()
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

    [Test]
    public void RecursivePatterns()
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

    [Test]
    public void PatternMatchingWithRecords()
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
