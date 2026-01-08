using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.InlineArrayExplicitSizeAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class InlineArrayExplicitSizeAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct has StructLayout with explicit LayoutKind and Size parameter.
    /// </summary>
    [Test]
    public async Task InlineArrayWithExplicitSize_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Explicit, Size = 32)]
            struct {|CS9168:{|ORNETX0004:Int8InlineArray|}|}
            {
                [FieldOffset(0)]
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct has StructLayout with Auto LayoutKind and Size parameter.
    /// </summary>
    [Test]
    public async Task InlineArrayWithExplicitSizeAutoLayout_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(4)]
            [StructLayout(LayoutKind.Auto, Size = 16)]
            struct {|ORNETX0004:MyInlineArray|}
            {
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct has StructLayout with Sequential LayoutKind and Size parameter.
    /// </summary>
    [Test]
    public async Task InlineArrayWithExplicitSizeSequentialLayout_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(10)]
            [StructLayout(LayoutKind.Sequential, Size = 40)]
            struct {|ORNETX0004:SequentialInlineArray|}
            {
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when an InlineArray struct has StructLayout without a Size parameter.
    /// </summary>
    [Test]
    public async Task InlineArrayWithoutExplicitSize_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Explicit)]
            struct {|CS9168:Int8InlineArray|}
            {
                [FieldOffset(0)]
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when an InlineArray struct has no StructLayout attribute.
    /// </summary>
    [Test]
    public async Task InlineArrayWithoutStructLayout_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;

            [InlineArray(8)]
            struct Int8InlineArray
            {
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when a struct has StructLayout with Size but no InlineArray attribute.
    /// </summary>
    [Test]
    public async Task StructWithExplicitSizeButNoInlineArray_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            [StructLayout(LayoutKind.Explicit, Size = 32)]
            struct RegularStruct
            {
                [FieldOffset(0)]
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when an InlineArray struct has StructLayout with Pack parameter but no Size.
    /// </summary>
    [Test]
    public async Task InlineArrayWithPackButNoSize_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            struct PackedInlineArray
            {
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when an InlineArray struct has StructLayout with CharSet parameter but no Size.
    /// </summary>
    [Test]
    public async Task InlineArrayWithCharSetButNoSize_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            struct CharSetInlineArray
            {
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when InlineArray and StructLayout with Size are applied to a class (not a struct).
    /// </summary>
    [Test]
    public async Task ClassWithInlineArrayAndSize_CompilerError()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [{|CS0592:InlineArray|}(8)]
            [StructLayout(LayoutKind.Explicit, Size = 32)]
            class NotAStruct
            {
                [FieldOffset(0)]
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct has StructLayout with Size set to zero.
    /// </summary>
    [Test]
    public async Task InlineArrayWithSizeZero_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Explicit, Size = 0)]
            struct {|CS9168:{|ORNETX0004:ZeroSizeInlineArray|}|}
            {
                [FieldOffset(0)]
                private int _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that multiple diagnostics are created when multiple InlineArray structs have StructLayout with Size parameter.
    /// </summary>
    [Test]
    public async Task MultipleInlineArraysWithIssues_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(8)]
            [StructLayout(LayoutKind.Explicit, Size = 32)]
            struct {|CS9168:{|ORNETX0004:FirstInlineArray|}|}
            {
                [FieldOffset(0)]
                private int _value;
            }

            [InlineArray(4)]
            [StructLayout(LayoutKind.Sequential, Size = 16)]
            struct {|ORNETX0004:SecondInlineArray|}
            {
                private byte _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct nested inside a class has StructLayout with Size parameter.
    /// </summary>
    [Test]
    public async Task InlineArrayNestedInClass_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            class Container
            {
                [InlineArray(8)]
                [StructLayout(LayoutKind.Explicit, Size = 32)]
                struct {|CS9168:{|ORNETX0004:NestedInlineArray|}|}
                {
                    [FieldOffset(0)]
                    private int _value;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an InlineArray struct has StructLayout with a large Size value.
    /// </summary>
    [Test]
    public async Task InlineArrayWithLargeSize_CreatesErrorDiagnostic()
    {
        const string text = """
            using System.Runtime.CompilerServices;
            using System.Runtime.InteropServices;

            [InlineArray(100)]
            [StructLayout(LayoutKind.Sequential, Size = 1024)]
            struct {|ORNETX0004:LargeSizeInlineArray|}
            {
                private long _value;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
