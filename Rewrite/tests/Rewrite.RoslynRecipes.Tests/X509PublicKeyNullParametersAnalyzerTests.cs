using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.X509PublicKeyNullParametersAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class X509PublicKeyNullParametersAnalyzerTests
{
    // ========================================================================
    // Positive cases - GetKeyAlgorithmParameters (ORNETX0019)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyAlgorithmParameters is called
    /// on an X509Certificate instance.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithmParameters_OnX509Certificate_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var p = cert.{|ORNETX0019:GetKeyAlgorithmParameters|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyAlgorithmParameters is called
    /// on an X509Certificate2 instance, which inherits the method from X509Certificate.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithmParameters_OnX509Certificate2_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate2 cert)
                {
                    var p = cert.{|ORNETX0019:GetKeyAlgorithmParameters|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyAlgorithmParametersString is called
    /// on an X509Certificate instance.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithmParametersString_OnX509Certificate_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var s = cert.{|ORNETX0019:GetKeyAlgorithmParametersString|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyAlgorithmParametersString is called
    /// on an X509Certificate2 instance.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithmParametersString_OnX509Certificate2_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate2 cert)
                {
                    var s = cert.{|ORNETX0019:GetKeyAlgorithmParametersString|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyAlgorithmParameters result is used
    /// inline without assignment.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithmParameters_UsedInline_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var length = cert.{|ORNETX0019:GetKeyAlgorithmParameters|}().Length;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that diagnostics are reported for both method calls when used in the same method.
    /// </summary>
    [Test]
    public async Task BothMethods_InSameCode_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var p = cert.{|ORNETX0019:GetKeyAlgorithmParameters|}();
                    var s = cert.{|ORNETX0019:GetKeyAlgorithmParametersString|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - PublicKey.EncodedParameters (ORNETX0020)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when EncodedParameters property is accessed
    /// on a PublicKey instance.
    /// </summary>
    [Test]
    public async Task EncodedParameters_PropertyAccess_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(PublicKey key)
                {
                    var p = key.{|ORNETX0020:EncodedParameters|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when EncodedParameters is accessed via
    /// X509Certificate2.PublicKey.EncodedParameters chain.
    /// </summary>
    [Test]
    public async Task EncodedParameters_ViaCertificateChain_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate2 cert)
                {
                    var p = cert.PublicKey.{|ORNETX0020:EncodedParameters|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when EncodedParameters is accessed
    /// and a member is subsequently accessed on it.
    /// </summary>
    [Test]
    public async Task EncodedParameters_WithMemberAccess_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(PublicKey key)
                {
                    var raw = key.{|ORNETX0020:EncodedParameters|}.RawData;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when EncodedParameters is passed as
    /// a method argument.
    /// </summary>
    [Test]
    public async Task EncodedParameters_PassedAsArgument_CreatesDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography;
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void Process(AsnEncodedData data) { }

                void M(PublicKey key)
                {
                    Process(key.{|ORNETX0020:EncodedParameters|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - Mixed
    // ========================================================================

    /// <summary>
    /// Verifies that both method and property diagnostics are reported when all three
    /// affected APIs are used in the same class.
    /// </summary>
    [Test]
    public async Task AllAffectedApis_InSameCode_CreatesAllDiagnostics()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate2 cert)
                {
                    var p = cert.{|ORNETX0019:GetKeyAlgorithmParameters|}();
                    var s = cert.{|ORNETX0019:GetKeyAlgorithmParametersString|}();
                    var e = cert.PublicKey.{|ORNETX0020:EncodedParameters|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported for other X509Certificate methods
    /// like GetKeyAlgorithm() which are not affected by this change.
    /// </summary>
    [Test]
    public async Task GetKeyAlgorithm_NoDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var alg = cert.GetKeyAlgorithm();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a custom class that has
    /// a method named GetKeyAlgorithmParameters.
    /// </summary>
    [Test]
    public async Task CustomGetKeyAlgorithmParameters_NoDiagnostic()
    {
        const string text = """
            class MyCert
            {
                public byte[] GetKeyAlgorithmParameters() => new byte[0];
            }

            class A
            {
                void M()
                {
                    var cert = new MyCert();
                    var p = cert.GetKeyAlgorithmParameters();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a custom class that has
    /// a property named EncodedParameters.
    /// </summary>
    [Test]
    public async Task CustomEncodedParameters_NoDiagnostic()
    {
        const string text = """
            class MyKey
            {
                public byte[]? EncodedParameters { get; set; }
            }

            class A
            {
                void M()
                {
                    var key = new MyKey();
                    var p = key.EncodedParameters;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when accessing other PublicKey properties
    /// like EncodedKeyValue which are not affected by this change.
    /// </summary>
    [Test]
    public async Task EncodedKeyValue_NoDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(PublicKey key)
                {
                    var v = key.EncodedKeyValue;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for X509Certificate.GetPublicKey()
    /// which is a different method not affected by this change.
    /// </summary>
    [Test]
    public async Task GetPublicKey_NoDiagnostic()
    {
        const string text = """
            using System.Security.Cryptography.X509Certificates;

            class A
            {
                void M(X509Certificate cert)
                {
                    var pk = cert.GetPublicKey();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }
}
