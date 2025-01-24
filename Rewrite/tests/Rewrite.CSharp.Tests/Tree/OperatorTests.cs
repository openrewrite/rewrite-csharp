using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class OperatorDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void UnaryOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    public static Complex operator +(Complex c) => c;
                }
                """
            )
        );
    }

    [Fact]
    public void BinaryOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    public static Complex operator +(Complex a, Complex b) => a;
                }
                """
            )
        );
    }

    [Fact]
    public void ConversionOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    public static explicit operator double(Complex c) => 0.0;
                }
                """
            )
        );
    }

    [Fact]
    public void ImplicitConversionOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    public static implicit operator Complex(double d) => new Complex();
                }
                """
            )
        );
    }

    [Fact]
    public void OverloadedComparisonOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    public static bool operator ==(Complex left, Complex right) => true;
                    public static bool operator !=(Complex left, Complex right) => false;
                }
                """
            )
        );
    }

    [Fact]
    public void CustomIndexOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Matrix
                {
                    public double this[int i, int j]
                    {
                        get => 0.0;
                        set { }
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void CheckedOperator()
    {
        RewriteRun(
            CSharp(
                """
                public class Integer
                {
                    public static Integer operator checked +(Integer a, Integer b) => a;
                }
                """
            )
        );
    }

    [Fact]
    public void OperatorWithAttributes()
    {
        RewriteRun(
            CSharp(
                """
                public class Complex
                {
                    [Obsolete]
                    public static Complex operator +(Complex a, Complex b) => a;
                }
                """
            )
        );
    }
}
