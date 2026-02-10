using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.OpenSslVersionOverrideEnvVarAnalyzer,
    Rewrite.RoslynRecipes.OpenSslVersionOverrideEnvVarCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

public class OpenSslVersionOverrideEnvVarCodeFixTests
{
    /// <summary>
    /// Tests that the old environment variable name in GetEnvironmentVariable is replaced
    /// with the new DOTNET_OPENSSL_VERSION_OVERRIDE name.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_ReplacedWithNewEnvVarName()
    {
        const string source = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|});
            """;

        const string fixedSource = """
            using System;

            var value = Environment.GetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE");
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the old environment variable name in the two-parameter GetEnvironmentVariable
    /// overload is replaced with the new name.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_WithTargetOverload_ReplacedWithNewEnvVarName()
    {
        const string source = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, EnvironmentVariableTarget.Process);
            """;

        const string fixedSource = """
            using System;

            var value = Environment.GetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE", EnvironmentVariableTarget.Process);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the old environment variable name in SetEnvironmentVariable is replaced
    /// with the new DOTNET_OPENSSL_VERSION_OVERRIDE name.
    /// </summary>
    [Test]
    public async Task SetEnvironmentVariable_ReplacedWithNewEnvVarName()
    {
        const string source = """
            using System;

            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "1.1");
            """;

        const string fixedSource = """
            using System;

            Environment.SetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE", "1.1");
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the old environment variable name in the three-parameter SetEnvironmentVariable
    /// overload is replaced with the new name.
    /// </summary>
    [Test]
    public async Task SetEnvironmentVariable_WithTargetOverload_ReplacedWithNewEnvVarName()
    {
        const string source = """
            using System;

            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "1.1", EnvironmentVariableTarget.Process);
            """;

        const string fixedSource = """
            using System;

            Environment.SetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE", "1.1", EnvironmentVariableTarget.Process);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that multiple occurrences of the old environment variable name are all replaced
    /// when batch fix is applied.
    /// </summary>
    [Test]
    public async Task MultipleUsages_AllReplacedWithNewEnvVarName()
    {
        const string source = """
            using System;

            var value = Environment.GetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|});
            Environment.SetEnvironmentVariable({|ORNETX0017:"CLR_OPENSSL_VERSION_OVERRIDE"|}, "3.0");
            """;

        const string fixedSource = """
            using System;

            var value = Environment.GetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE");
            Environment.SetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE", "3.0");
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the old environment variable name is replaced when used inside
    /// a conditional expression.
    /// </summary>
    [Test]
    public async Task GetEnvironmentVariable_InConditional_ReplacedWithNewEnvVarName()
    {
        const string source = """
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

        const string fixedSource = """
            using System;

            class A
            {
                void M()
                {
                    if (Environment.GetEnvironmentVariable("DOTNET_OPENSSL_VERSION_OVERRIDE") != null)
                    {
                    }
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.Net90).ConfigureAwait(false);
    }
}
