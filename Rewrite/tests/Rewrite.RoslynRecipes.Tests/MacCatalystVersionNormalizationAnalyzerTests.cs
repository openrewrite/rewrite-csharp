using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.MacCatalystVersionNormalizationAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class MacCatalystVersionNormalizationAnalyzerTests
{
    // ========================================================================
    // IsMacCatalystVersionAtLeast - Positive cases (ORNETX0014)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when IsMacCatalystVersionAtLeast is called
    /// with all three arguments (major, minor, build).
    /// </summary>
    [Test]
    public async Task IsMacCatalystVersionAtLeast_WithThreeArgs_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0014:IsMacCatalystVersionAtLeast|}(15, 0, 1);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsMacCatalystVersionAtLeast is called
    /// with only the major version argument, relying on optional parameter defaults.
    /// </summary>
    [Test]
    public async Task IsMacCatalystVersionAtLeast_WithMajorOnly_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0014:IsMacCatalystVersionAtLeast|}(15);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsMacCatalystVersionAtLeast is called
    /// with major and minor arguments.
    /// </summary>
    [Test]
    public async Task IsMacCatalystVersionAtLeast_WithMajorAndMinor_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0014:IsMacCatalystVersionAtLeast|}(15, 4);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsMacCatalystVersionAtLeast is used
    /// inside a conditional expression.
    /// </summary>
    [Test]
    public async Task IsMacCatalystVersionAtLeast_InConditional_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M()
                {
                    if (OperatingSystem.{|ORNETX0014:IsMacCatalystVersionAtLeast|}(15, 0))
                    {
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // IsOSPlatformVersionAtLeast - Positive cases (ORNETX0015)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when IsOSPlatformVersionAtLeast is called
    /// with all five arguments.
    /// </summary>
    [Test]
    public async Task IsOSPlatformVersionAtLeast_WithAllArgs_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}("maccatalyst", 15, 0, 1, 0);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsOSPlatformVersionAtLeast is called
    /// with only the platform and major version, relying on defaults for minor, build, and revision.
    /// </summary>
    [Test]
    public async Task IsOSPlatformVersionAtLeast_WithPlatformAndMajor_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}("maccatalyst", 15);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsOSPlatformVersionAtLeast is called
    /// with a non-MacCatalyst platform string, since the platform value cannot always be
    /// determined at compile time and the behavioral change affects the method itself.
    /// </summary>
    [Test]
    public async Task IsOSPlatformVersionAtLeast_WithOtherPlatformString_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}("ios", 16, 0);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsOSPlatformVersionAtLeast is called
    /// with a variable as the platform name, since the value cannot be determined at compile time.
    /// </summary>
    [Test]
    public async Task IsOSPlatformVersionAtLeast_WithVariablePlatform_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M(string platform)
                {
                    var result = OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}(platform, 15);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when IsOSPlatformVersionAtLeast is used
    /// inside a conditional expression.
    /// </summary>
    [Test]
    public async Task IsOSPlatformVersionAtLeast_InConditional_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M()
                {
                    if (OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}("maccatalyst", 15, 4))
                    {
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Environment.OSVersion - Positive cases (ORNETX0016)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when Environment.OSVersion is accessed
    /// and assigned to a variable.
    /// </summary>
    [Test]
    public async Task EnvironmentOSVersion_PropertyAccess_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var version = Environment.{|ORNETX0016:OSVersion|};
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when Environment.OSVersion is accessed
    /// inside a conditional to check the version.
    /// </summary>
    [Test]
    public async Task EnvironmentOSVersion_InVersionComparison_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void M()
                {
                    if (Environment.{|ORNETX0016:OSVersion|}.Version.Major >= 15)
                    {
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when Environment.OSVersion is passed
    /// as a method argument.
    /// </summary>
    [Test]
    public async Task EnvironmentOSVersion_PassedAsArgument_CreatesDiagnostic()
    {
        const string text = """
            using System;

            class A
            {
                void Log(OperatingSystem os) { }

                void M()
                {
                    Log(Environment.{|ORNETX0016:OSVersion|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when Environment.OSVersion.ToString() is called.
    /// </summary>
    [Test]
    public async Task EnvironmentOSVersion_ToStringCall_CreatesDiagnostic()
    {
        const string text = """
            using System;

            var versionString = Environment.{|ORNETX0016:OSVersion|}.ToString();
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Multiple diagnostics in same code
    // ========================================================================

    /// <summary>
    /// Verifies that multiple diagnostics are reported when both IsMacCatalystVersionAtLeast
    /// and IsOSPlatformVersionAtLeast are used in the same method.
    /// </summary>
    [Test]
    public async Task MultipleMethods_InSameCode_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System;

            class A
            {
                void M()
                {
                    var a = OperatingSystem.{|ORNETX0014:IsMacCatalystVersionAtLeast|}(15);
                    var b = OperatingSystem.{|ORNETX0015:IsOSPlatformVersionAtLeast|}("maccatalyst", 15);
                    var c = Environment.{|ORNETX0016:OSVersion|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported for other OperatingSystem version check methods
    /// that are not affected by the MacCatalyst version normalization change.
    /// </summary>
    [Test]
    public async Task IsWindowsVersionAtLeast_NoDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19041);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for IsLinuxVersionAtLeast, which is not affected
    /// by the MacCatalyst version normalization change.
    /// </summary>
    [Test]
    public async Task IsMacOS_NoDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.IsMacOS();
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for IsMacCatalyst (without version parameters),
    /// which is a simple platform check unaffected by version normalization.
    /// </summary>
    [Test]
    public async Task IsMacCatalyst_WithoutVersion_NoDiagnostic()
    {
        const string text = """
            using System;

            var result = OperatingSystem.IsMacCatalyst();
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when a custom class defines a method
    /// named IsMacCatalystVersionAtLeast.
    /// </summary>
    [Test]
    public async Task CustomIsMacCatalystVersionAtLeast_NoDiagnostic()
    {
        const string text = """
            class PlatformHelper
            {
                public static bool IsMacCatalystVersionAtLeast(int major, int minor = 0, int build = 0) => false;
            }

            class A
            {
                void M()
                {
                    var result = PlatformHelper.IsMacCatalystVersionAtLeast(15);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when a custom class defines a method
    /// named IsOSPlatformVersionAtLeast.
    /// </summary>
    [Test]
    public async Task CustomIsOSPlatformVersionAtLeast_NoDiagnostic()
    {
        const string text = """
            class PlatformHelper
            {
                public static bool IsOSPlatformVersionAtLeast(string platform, int major, int minor = 0, int build = 0, int revision = 0) => false;
            }

            class A
            {
                void M()
                {
                    var result = PlatformHelper.IsOSPlatformVersionAtLeast("maccatalyst", 15);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when a custom class defines a property
    /// named OSVersion.
    /// </summary>
    [Test]
    public async Task CustomOSVersionProperty_NoDiagnostic()
    {
        const string text = """
            class MyEnvironment
            {
                public static string OSVersion => "custom";
            }

            class A
            {
                void M()
                {
                    var v = MyEnvironment.OSVersion;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
