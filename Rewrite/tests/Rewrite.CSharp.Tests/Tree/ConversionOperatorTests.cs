using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class ConversionOperatorTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void ImplicitConversion()
    {
        RewriteRun(
            CSharp(
                """
                public class Money
                {
                    public decimal Amount { get; }

                    public static implicit operator decimal(Money money)
                    {
                        return money.Amount;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ExplicitConversion()
    {
        RewriteRun(
            CSharp(
                """
                public class Temperature
                {
                    public double Celsius { get; }

                    public static explicit operator int(Temperature temp)
                    {
                        return (int)temp.Celsius;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionWithGenerics()
    {
        RewriteRun(
            CSharp(
                """
                public class Wrapper<T>
                {
                    private T value;

                    public static implicit operator T(Wrapper<T> wrapper)
                    {
                        return wrapper.value;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionBetweenClasses()
    {
        RewriteRun(
            CSharp(
                """
                public class Dollars
                {
                    public decimal Value { get; }
                }

                public class Euros
                {
                    public decimal Value { get; }

                    public static explicit operator Dollars(Euros euros)
                    {
                        return new Dollars { Value = euros.Value * 1.1m };
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionWithNullable()
    {
        RewriteRun(
            CSharp(
                """
                public class OptionalValue
                {
                    public int? Value { get; }

                    public static implicit operator int?(OptionalValue optional)
                    {
                        return optional?.Value;
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionOperatorWithAttributes()
    {
        RewriteRun(
            CSharp(
                """
                public class Vector
                {
                    public double X { get; }
                    public double Y { get; }

                    [Obsolete("Use explicit cast instead")]
                    public static implicit operator double(Vector vector)
                    {
                        return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionWithConstraints()
    {
        RewriteRun(
            CSharp(
                """
                public class Container<T> where T : struct
                {
                    private T value;

                    public static explicit operator T(Container<T> container)
                    {
                        return container.value;
                    }
                }
                """
            )
        );
    }


    [Fact]
    public void MultipleConversionOperators()
    {
        RewriteRun(
            CSharp(
                """
                public class Distance
                {
                    private readonly double _meters;

                    public static implicit operator double(Distance d) => d._meters;
                    public static explicit operator int(Distance d) => (int)d._meters;
                    public static explicit operator float(Distance d) => (float)d._meters;
                }
                """
            )
        );
    }
}
