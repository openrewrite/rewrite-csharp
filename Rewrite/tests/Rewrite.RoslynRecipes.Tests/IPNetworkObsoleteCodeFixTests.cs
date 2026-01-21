using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.IPNetworkObsoleteAnalyzer,
    Rewrite.RoslynRecipes.IPNetworkObsoleteCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

/// <summary>
/// Tests for the <see cref="IPNetworkObsoleteCodeFixProvider"/> which provides code fixes for
/// migrating from obsolete Microsoft.AspNetCore.HttpOverrides.IPNetwork to System.Net.IPNetwork
/// and from KnownNetworks to KnownIPNetworks.
/// </summary>
public class IPNetworkObsoleteCodeFixTests
{
    /// <summary>
    /// Tests that KnownNetworks property access is correctly replaced with KnownIPNetworks.
    /// Avoids using IPNetwork.Add() to prevent type conflicts.
    /// </summary>
    [Test]
    public async Task KnownNetworksPropertyAccess_ReplacedWithKnownIPNetworks()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            options.{|ORNETX0011:KnownNetworks|}?.Clear();
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            options.KnownIPNetworks?.Clear();
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides", "2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that KnownNetworks Count property access is correctly replaced with KnownIPNetworks.
    /// </summary>
    [Test]
    public async Task KnownNetworksCountAccess_ReplacedWithKnownIPNetworks()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            var count = options.{|ORNETX0011:KnownNetworks|}.Count;
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            var count = options.KnownIPNetworks.Count;
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides", "2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that KnownNetworks in UseForwardedHeaders configuration is correctly replaced with KnownIPNetworks.
    /// </summary>
    [Test]
    public async Task KnownNetworksInUseForwardedHeaders_ReplacedWithKnownIPNetworks()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            var options = new ForwardedHeadersOptions();
            _ = options.{|ORNETX0011:KnownNetworks|}.Count;
            app.UseForwardedHeaders(options);
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            var options = new ForwardedHeadersOptions();
            _ = options.KnownIPNetworks.Count;
            app.UseForwardedHeaders(options);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides", "2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork type in variable declaration is correctly replaced with System.Net.IPNetwork.
    /// Uses fully qualified name to avoid ambiguity between System.Net.IPNetwork and Microsoft.AspNetCore.HttpOverrides.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeInVariableDeclaration_ReplacedWithSystemNetIPNetwork()
    {
        const string source = """
            using System.Net;

            var network = new {|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|}(IPAddress.Loopback, 8);
            """;

        const string fixedSource = """
            using System.Net;

            var network = new IPNetwork(IPAddress.Loopback, 8);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork type as method parameter is correctly replaced with System.Net.IPNetwork.
    /// Uses fully qualified name to avoid ambiguity between System.Net.IPNetwork and Microsoft.AspNetCore.HttpOverrides.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsMethodParameter_ReplacedWithSystemNetIPNetwork()
    {
        const string source = """
            class TestClass
            {
                void ProcessNetwork({|ORNETX0010:Microsoft.AspNetCore.HttpOverrides.IPNetwork|} network)
                {
                }
            }
            """;

        const string fixedSource = """
            using System.Net;

            class TestClass
            {
                void ProcessNetwork(IPNetwork network)
                {
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork type as return type is correctly replaced with System.Net.IPNetwork.
    /// Uses fully qualified name to avoid ambiguity between System.Net.IPNetwork and Microsoft.AspNetCore.HttpOverrides.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsReturnType_ReplacedWithSystemNetIPNetwork()
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

        const string fixedSource = """
            using System.Net;

            class TestClass
            {
                IPNetwork GetNetwork()
                {
                    return new IPNetwork(IPAddress.Loopback, 8);
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides", "2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork type as field type is correctly replaced with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsFieldType_ReplacedWithSystemNetIPNetwork()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;

            class TestClass
            {
                private {|ORNETX0010:IPNetwork|}? _network;
            }
            """;

        const string fixedSource = """
            using System.Net;

            class TestClass
            {
                private IPNetwork? _network;
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that IPNetwork type as property type is correctly replaced with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeAsPropertyType_ReplacedWithSystemNetIPNetwork()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;

            class TestClass
            {
                public {|ORNETX0010:IPNetwork|}? Network { get; set; }
            }
            """;

        const string fixedSource = """
            using System.Net;

            class TestClass
            {
                public IPNetwork? Network { get; set; }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
        
    }

    /// <summary>
    /// Tests that IPNetwork type in generic collection is correctly replaced with System.Net.IPNetwork.
    /// </summary>
    [Test]
    public async Task IPNetworkTypeInGenericCollection_ReplacedWithSystemNetIPNetwork()
    {
        const string source = """
            using Microsoft.AspNetCore.HttpOverrides;
            using System.Collections.Generic;

            class TestClass
            {
                private List<{|ORNETX0010:IPNetwork|}> _networks = new();
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;
            using System.Net;

            class TestClass
            {
                private List<IPNetwork> _networks = new();
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that multiple KnownNetworks usages in the same file are all correctly replaced.
    /// </summary>
    [Test]
    public async Task MultipleKnownNetworksUsages_AllReplaced()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;

            var options1 = new ForwardedHeadersOptions();
            _ = options1.{|ORNETX0011:KnownNetworks|}.Count;

            var options2 = new ForwardedHeadersOptions();
            options2.{|ORNETX0011:KnownNetworks|}?.Clear();
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;

            var options1 = new ForwardedHeadersOptions();
            _ = options1.KnownIPNetworks.Count;

            var options2 = new ForwardedHeadersOptions();
            options2.KnownIPNetworks?.Clear();
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides", "2.3.9"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that KnownNetworks with chained method calls is correctly replaced.
    /// No IPNetwork in this test, so no ambiguity to resolve.
    /// </summary>
    [Test]
    public async Task KnownNetworksWithChainedCalls_ReplacedCorrectly()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            var count = options.{|ORNETX0011:KnownNetworks|}.Count;
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;

            var options = new ForwardedHeadersOptions();
            var count = options.KnownIPNetworks.Count;
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.HttpOverrides","2.3.9"))
            .ConfigureAwait(false);
    }

    void Test()
    {
        
    }
}
