using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.DllImportSearchPathAnalyzer,
    Rewrite.RoslynRecipes.DllImportSearchPathCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

public class DllImportSearchPathCodeFixTests
{
    /// <summary>
    /// Tests that the DefaultDllImportSearchPaths attribute is removed from a P/Invoke method
    /// when it only specifies DllImportSearchPath.AssemblyDirectory.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_OnMethod_AttributeRemoved()
    {
        const string source = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                [{|ORNETX0023:DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)|}]
                public static extern void ExampleMethod();
            }
            """;

        const string fixedSource = """
            using System.Runtime.InteropServices;

            class NativeLib
            {
                [DllImport("example.dll")]
                public static extern void ExampleMethod();
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the DefaultDllImportSearchPaths attribute is removed at the assembly level
    /// when it only specifies DllImportSearchPath.AssemblyDirectory.
    /// </summary>
    [Test]
    public async Task DefaultDllImportSearchPaths_OnAssembly_AttributeRemoved()
    {
        const string source = """
            using System.Runtime.InteropServices;

            [assembly: {|ORNETX0023:DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)|}]
            """;

        const string fixedSource = """
            using System.Runtime.InteropServices;


            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that DllImportSearchPath.AssemblyDirectory is replaced with null
    /// in a NativeLibrary.Load call to restore default search behavior.
    /// </summary>
    [Test]
    public async Task NativeLibraryLoad_AssemblyDirectoryReplacedWithNull()
    {
        const string source = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example", Assembly.GetExecutingAssembly(), {|ORNETX0024:DllImportSearchPath.AssemblyDirectory|});
            """;

        const string fixedSource = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            var handle = NativeLibrary.Load("example", Assembly.GetExecutingAssembly(), null);

            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that DllImportSearchPath.AssemblyDirectory is replaced with null
    /// in a NativeLibrary.TryLoad call to restore default search behavior.
    /// </summary>
    [Test]
    public async Task NativeLibraryTryLoad_AssemblyDirectoryReplacedWithNull()
    {
        const string source = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            NativeLibrary.TryLoad("example", Assembly.GetExecutingAssembly(), {|ORNETX0024:DllImportSearchPath.AssemblyDirectory|}, out var handle);
            """;

        const string fixedSource = """
            using System.Reflection;
            using System.Runtime.InteropServices;

            NativeLibrary.TryLoad("example", Assembly.GetExecutingAssembly(), null, out var handle);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }
}
