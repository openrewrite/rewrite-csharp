using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.IPNetworkObsoleteAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

/// <summary>
/// Tests for the <see cref="IPNetworkObsoleteAnalyzer"/> which detects usage of obsolete
/// Microsoft.AspNetCore.HttpOverrides.IPNetwork type and ForwardedHeadersOptions.KnownNetworks property.
/// </summary>
public class IPNetworkObsoleteAnalyzerTests
{
    /// <summary>
    /// Tests that using the obsolete IPNetwork type in a variable declaration triggers ORNETX0010 diagnostic.
    /// Uses fully qualified name to avoid ambiguity with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeInVariableDeclaration_TriggersDiagnostic()
    {
        const string source = """
            using System.Net;

            var network = new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8);
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using the obsolete IPNetwork type as a method parameter type triggers ORNETX0010 diagnostic.
    /// Uses fully qualified name to avoid ambiguity with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsMethodParameter_TriggersDiagnostic()
    {
        const string source = """
            class TestClass
            {
                void ProcessNetwork({|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|} network)
                {
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using the obsolete IPNetwork type as a return type triggers ORNETX0010 diagnostic.
    /// Uses fully qualified name to avoid ambiguity with System.Net.IPNetwork.
    /// Also verifies the IPNetwork usage in new statement triggers a second diagnostic.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsReturnType_TriggersDiagnostic()
    {
        const string source = """
            using System.Net;

            class TestClass
            {
                {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|} GetNetwork()
                {
                    return new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using the obsolete IPNetwork type as a field type triggers ORNETX0010 diagnostic.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsFieldType_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;

            class TestClass
            {
                private {|ORNETX0010:IPNetwork|}? _network;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using the obsolete IPNetwork type as a property type triggers ORNETX0010 diagnostic.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsPropertyType_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;

            class TestClass
            {
                public {|ORNETX0010:IPNetwork|}? Network { get; set; }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that accessing ForwardedHeadersOptions.KnownNetworks property triggers ORNETX0011 diagnostic.
    /// Uses fully qualified IPNetwork name to avoid ambiguity with System.Net.IPNetwork.
    /// Also verifies that the IPNetwork usage triggers ORNETX0010.
    /// </summary>
    [Test]
    public async Task KnownNetworksPropertyAccess_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using System.Net;

            var options = new ForwardedHeadersOptions();
            options.{|ORNETX0011:KnownNetworks|}.Add(new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8));
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork usage in object initializer triggers ORNETX0010 diagnostic.
    /// Note: KnownNetworks usage in object initializers is not currently detected by the analyzer.
    /// Uses fully qualified IPNetwork name to avoid ambiguity with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task KnownNetworksInObjectInitializer_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using System.Net;

            var options = new ForwardedHeadersOptions
            {
                KnownNetworks = { new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8) }
            };
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that both IPNetwork type and KnownNetworks property usages in the same code trigger both diagnostics.
    /// </summary>
    [Test]
    public async Task IPNetworkAndKnownNetworksTogether_TriggersBothDiagnostics()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.HttpOverrides;
            using System.Net;

            var network = new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8);
            var options = new ForwardedHeadersOptions();
            options.{|ORNETX0011:KnownNetworks|}.Add(network);
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using System.Net.IPNetwork (the recommended replacement) does not trigger any diagnostic.
    /// </summary>
    [Test]
    public async Task SystemNetIPNetwork_NoDiagnostic()
    {
        const string source = """
            using System.Net;

            var network = new IPNetwork(IPAddress.Loopback, 8);
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.Net90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that accessing KnownIPNetworks (the recommended replacement) does not trigger any diagnostic.
    /// </summary>
    [Test]
    public async Task KnownIPNetworksProperty_NoDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using System.Net;

            var options = new ForwardedHeadersOptions();
            options.KnownIPNetworks.Add(new IPNetwork(IPAddress.Loopback, 8));
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that a custom type named IPNetwork does not trigger the diagnostic.
    /// </summary>
    [Test]
    public async Task CustomIPNetworkType_NoDiagnostic()
    {
        const string source = """
            class IPNetwork
            {
                public string? Address { get; set; }
            }

            class TestClass
            {
                void Test()
                {
                    var network = new IPNetwork();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.Net90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that a custom property named KnownNetworks does not trigger the diagnostic.
    /// </summary>
    [Test]
    public async Task CustomKnownNetworksProperty_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;

            class CustomOptions
            {
                public List<string> KnownNetworks { get; } = new();
            }

            class TestClass
            {
                void Test()
                {
                    var options = new CustomOptions();
                    options.KnownNetworks.Add("192.168.1.0/24");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.Net90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using IPNetwork in a generic collection triggers ORNETX0010 diagnostic.
    /// </summary>
    [Test]
    public async Task IPNetworkInGenericCollection_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;
            using System.Collections.Generic;

            class TestClass
            {
                private List<{|ORNETX0010:IPNetwork|}> _networks = new();
            }
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that using IPNetwork in UseForwardedHeaders context triggers both diagnostics.
    /// </summary>
    [Test]
    public async Task IPNetworkWithUseForwardedHeaders_TriggersDiagnostic()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using System.Net;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            var options = new ForwardedHeadersOptions();
            options.{|ORNETX0011:KnownNetworks|}.Add(new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Parse("10.0.0.0"), 8));
            app.UseForwardedHeaders(options);
            """;

        await Verifier.VerifyAnalyzerAsync(source,
            Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }
}
