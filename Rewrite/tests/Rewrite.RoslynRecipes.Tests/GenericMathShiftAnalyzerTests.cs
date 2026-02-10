using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.GenericMathShiftAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class GenericMathShiftAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is reported for left shift on a generic type parameter
    /// constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task LeftShift_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T ShiftLeft<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    return value {|ORNETX0013:<<|} amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for right shift on a generic type parameter
    /// constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task RightShift_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T ShiftRight<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    return value {|ORNETX0013:>>|} amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for unsigned right shift on a generic type parameter
    /// constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task UnsignedRightShift_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T UnsignedShiftRight<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    return value {|ORNETX0013:>>>|} amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for left shift on a generic type parameter
    /// constrained to IBinaryInteger, which inherits from IShiftOperators.
    /// </summary>
    [Test]
    public async Task LeftShift_WithIBinaryIntegerConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T ShiftLeft<T>(T value, int amount) where T : IBinaryInteger<T>
                {
                    return value {|ORNETX0013:<<|} amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for compound left shift assignment on a generic
    /// type parameter constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task LeftShiftAssignment_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T ShiftLeft<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    value {|ORNETX0013:<<=|} amount;
                    return value;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for compound right shift assignment on a generic
    /// type parameter constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task RightShiftAssignment_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T ShiftRight<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    value {|ORNETX0013:>>=|} amount;
                    return value;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for compound unsigned right shift assignment on a generic
    /// type parameter constrained to IShiftOperators.
    /// </summary>
    [Test]
    public async Task UnsignedRightShiftAssignment_WithIShiftOperatorsConstraint_CreatesDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T UnsignedShiftRight<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    value {|ORNETX0013:>>>=|} amount;
                    return value;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that multiple shift operations in the same method each produce their own diagnostic.
    /// </summary>
    [Test]
    public async Task MultipleShifts_WithIShiftOperatorsConstraint_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T Compute<T>(T value, int amount) where T : IShiftOperators<T, int, T>
                {
                    var left = value {|ORNETX0013:<<|} amount;
                    var right = value {|ORNETX0013:>>|} amount;
                    return left;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for left shift on a concrete int type,
    /// which does not go through the generic math IShiftOperators interface.
    /// </summary>
    [Test]
    public async Task LeftShift_OnConcreteInt_NoDiagnostic()
    {
        const string text = """
            class A
            {
                int M(int value, int amount)
                {
                    return value << amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for left shift on a concrete byte,
    /// which uses the built-in C# shift operator (promoted to int), not generic math.
    /// </summary>
    [Test]
    public async Task LeftShift_OnConcreteByte_NoDiagnostic()
    {
        const string text = """
            class A
            {
                int M(byte value, int amount)
                {
                    return value << amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for left shift on a concrete long type.
    /// </summary>
    [Test]
    public async Task LeftShift_OnConcreteLong_NoDiagnostic()
    {
        const string text = """
            class A
            {
                long M(long value, int amount)
                {
                    return value << amount;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for addition on a generic type parameter
    /// constrained to IAdditionOperators, since addition is not a shift operation.
    /// </summary>
    [Test]
    public async Task Addition_OnGenericType_NoDiagnostic()
    {
        const string text = """
            using System.Numerics;

            class A
            {
                T Add<T>(T left, T right) where T : IAdditionOperators<T, T, T>
                {
                    return left + right;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for compound left shift assignment on a concrete int.
    /// </summary>
    [Test]
    public async Task LeftShiftAssignment_OnConcreteInt_NoDiagnostic()
    {
        const string text = """
            class A
            {
                int M(int value, int amount)
                {
                    value <<= amount;
                    return value;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
