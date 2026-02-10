using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.OpenSslVersionOverrideEnvVarAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class OpenSslVersionOverrideEnvVarAnalyzerTests
{
    // ========================================================================
    // Positive cases - GetEnvironmentVariable
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when GetEnvironmentVariable is called
    /// with the old CLR_OPENSSL_VERSION_OVERRIDE string literal.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithOldEnvVar_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|});
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when the two-parameter overload of
    /// GetEnvironmentVariable is called with the old environment variable name.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithTargetOverload_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, EnvironmentVariableTarget.Process);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetEnvironmentVariable with the old
    /// environment variable name is used inside a conditional.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_InConditional_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M()
                {
                    if (Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}) != null)
                    {
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - SetEnvironmentVariable
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when SetEnvironmentVariable is called
    /// with the old CLR_OPENSSL_VERSION_OVERRIDE string literal.
    /// </summary>
    [Test]
    public async Task SetEnvironmentVariable_WithOldEnvVar_CreatesDiagnostic()
    {
        const string text = """
            using System;

            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "1.1");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when the three-parameter overload of
    /// SetEnvironmentVariable is called with the old environment variable name.
    /// </summary>
    [Test]
    public async Task SetEnvironmentVariable_WithTargetOverload_CreatesDiagnostic()
    {
        const string text = """
            using System;

            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "1.1", EnvironmentVariableTarget.Process);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - Multiple usages
    // ========================================================================

    /// <summary>
    /// Verifies that multiple diagnostics are reported when both Get and Set
    /// use the old environment variable name.
    /// </summary>
    [Test]
    public async Task GetAndSet_WithOldEnvVar_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|});
            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "3.0");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported when GetEnvironmentVariable is called
    /// with the new DOTNET_OPENSSL_VERSION_OVERRIDE name.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithNewEnvVar_NoDiagnostic()
    {
        const string text = """
            using System;

            var value = Environment.GetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when SetEnvironmentVariable is called
    /// with the new DOTNET_OPENSSL_VERSION_OVERRIDE name.
    /// </summary>
    [Test]
    public async Task SetEnvironmentVariable_WithNewEnvVar_NoDiagnostic()
    {
        const string text = """
            using System;

            Environment.SetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE", "1.1");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when GetEnvironmentVariable is called
    /// with a completely unrelated environment variable name.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithUnrelatedEnvVar_NoDiagnostic()
    {
        const string text = """
            using System;

            var value = Environment.GetEnvironmentVariable("PATH");
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when the old variable name is passed
    /// via a variable rather than a string literal, since the analyzer only detects literals.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithVariableArg_NoDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M(string envVarName)
                {
                    var value = Environment.GetEnvironmentVariable(envVarName);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when a custom class has its own
    /// GetEnvironmentVariable method with the old variable name.
    /// </summary>
    [Test]
    public async Task CustomGetEnvironmentVariable_WithOldEnvVar_NoDiagnostic()
    {
        const string text = """
            class EnvHelper
            {
                public static string? GetEnvironmentVariable(string name) => null;
            }

            class A
            {
                void M()
                {
                    var value = EnvHelper.GetEnvironmentVariable("CLR_OPENSSL_VERSION_OVERRIDE");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when the old variable name appears
    /// as a string literal outside of Environment method calls.
    /// </summary>
    [Test]
    public async Task OldEnvVarAsPlainString_NoDiagnostic()
    {
        const string text = """
            using System;

            var name = "CLR_OPENSSL_VERSION_OVERRIDE";
            Console.WriteLine(name);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
