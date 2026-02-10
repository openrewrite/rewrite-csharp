using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.DllImportSearchPathAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class DllImportSearchPathAnalyzerTests
{
    // ========================================================================
    // Positive cases - Attribute
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when DefaultDllImportSearchPaths attribute
    /// uses DllImportSearchPath.AssemblyDirectory as the sole flag on a P/Invoke method.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_AssemblyDirectoryOnMethod_CreatesDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                [{|ORNETX0023:DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)|}]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when DefaultDllImportSearchPaths attribute
    /// uses DllImportSearchPath.AssemblyDirectory as the sole flag at the assembly level.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_AssemblyDirectoryOnAssembly_CreatesDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            [assembly: {|ORNETX0023:DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)|}]
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when DefaultDllImportSearchPaths attribute
    /// uses the fully qualified attribute name with Attribute suffix.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPathsAttribute_FullName_CreatesDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                [{|ORNETX0023:DefaultDllImportSearchPathsAttribute(DllImportSearchPath.AssemblyDirectory)|}]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - NativeLibrary method calls
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when NativeLibrary.Load is called
    /// with DllImportSearchPath.AssemblyDirectory as the sole search path.
    /// </summary>
    [Test]
    public async Task NativeLibraryLoad_AssemblyDirectory_CreatesDiagnostic()
    {
        const string text = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example", Assembly.GetExecutingAssembly(), {|ORNETX0024:DllImportSearchPath.AssemblyDirectory|});
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when NativeLibrary.TryLoad is called
    /// with DllImportSearchPath.AssemblyDirectory as the sole search path.
    /// </summary>
    [Test]
    public async Task NativeLibraryTryLoad_AssemblyDirectory_CreatesDiagnostic()
    {
        const string text = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            NativeLibrary.TryLoad("example", Assembly.GetExecutingAssembly(), {|ORNETX0024:DllImportSearchPath.AssemblyDirectory|}, out var handle);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported when DefaultDllImportSearchPaths attribute
    /// uses combined flags including AssemblyDirectory.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_CombinedFlags_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.System32)]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when DefaultDllImportSearchPaths attribute
    /// uses a different flag (not AssemblyDirectory).
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_System32Only_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a DllImport attribute without
    /// any DefaultDllImportSearchPaths attribute.
    /// </summary>
    [Test]
    public async Task DllImport_WithoutSearchPaths_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when NativeLibrary.Load is called
    /// without a search path argument (single-argument overload).
    /// </summary>
    [Test]
    public async Task NativeLibraryLoad_NoSearchPath_NoDiagnostic()
    {
        const string text = """
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when NativeLibrary.Load is called
    /// with combined DllImportSearchPath flags.
    /// </summary>
    [Test]
    public async Task NativeLibraryLoad_CombinedFlags_NoDiagnostic()
    {
        const string text = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example", Assembly.GetExecutingAssembly(), DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.System32);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when NativeLibrary.Load is called
    /// with null as the search path.
    /// </summary>
    [Test]
    public async Task NativeLibraryLoad_NullSearchPath_NoDiagnostic()
    {
        const string text = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example", Assembly.GetExecutingAssembly(), null);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
