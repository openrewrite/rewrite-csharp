using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.GetKeyedServiceAnyKeyAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class GetKeyedServiceAnyKeyAnalyzerTests
{
    // ========================================================================
    // Positive cases - GetKeyedService (generic)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when the generic GetKeyedService
    /// extension method is called with KeyedService.AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedService_Generic_WithAnyKey_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    sp.GetKeyedService<string>({|ORNETX0021:KeyedService.AnyKey|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - GetKeyedService (non-generic)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyedService is called
    /// on an IKeyedServiceProvider with KeyedService.AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedService_OnIKeyedServiceProvider_WithAnyKey_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IKeyedServiceProvider sp)
                {
                    sp.GetKeyedService(typeof(string), {|ORNETX0021:KeyedService.AnyKey|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - GetKeyedServices (generic)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when the generic GetKeyedServices
    /// extension method is called with KeyedService.AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedServices_Generic_WithAnyKey_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    var services = sp.GetKeyedServices<string>({|ORNETX0021:KeyedService.AnyKey|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - GetKeyedServices (non-generic)
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when the non-generic GetKeyedServices
    /// extension method is called with KeyedService.AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedServices_NonGeneric_WithAnyKey_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    var services = sp.GetKeyedServices(typeof(string), {|ORNETX0021:KeyedService.AnyKey|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Positive cases - Multiple usages
    // ========================================================================

    /// <summary>
    /// Verifies that diagnostics are reported for both GetKeyedService and GetKeyedServices
    /// with AnyKey when used in the same method.
    /// </summary>
    [Test]
    public async Task BothMethods_WithAnyKey_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    var svc = sp.GetKeyedService<string>({|ORNETX0021:KeyedService.AnyKey|});
                    var svcs = sp.GetKeyedServices<string>({|ORNETX0021:KeyedService.AnyKey|});
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when GetKeyedService result is used
    /// inline without assignment.
    /// </summary>
    [Test]
    public async Task GetKeyedService_UsedInline_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    var str = sp.GetKeyedService<string>({|ORNETX0021:KeyedService.AnyKey|})?.ToString();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported when GetKeyedService is called
    /// with a specific string key instead of AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedService_WithSpecificStringKey_NoDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    sp.GetKeyedService<string>("myKey");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when GetKeyedServices is called
    /// with a specific string key instead of AnyKey.
    /// </summary>
    [Test]
    public async Task GetKeyedServices_WithSpecificStringKey_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Linq;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    var svcs = sp.GetKeyedServices<string>("myKey").ToList();
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a custom class that has
    /// a method named GetKeyedService with an AnyKey parameter.
    /// </summary>
    [Test]
    public async Task CustomGetKeyedService_WithAnyKey_NoDiagnostic()
    {
        const string text = """
            class KeyedService
            {
                public static readonly object AnyKey = new object();
            }

            class MyProvider
            {
                public object? GetKeyedService(object key) => null;
            }

            class A
            {
                void M()
                {
                    var provider = new MyProvider();
                    provider.GetKeyedService(KeyedService.AnyKey);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when KeyedService.AnyKey is used
    /// in a non-DI context, such as a variable assignment.
    /// </summary>
    [Test]
    public async Task AnyKey_NotInGetKeyedServiceCall_NoDiagnostic()
    {
        const string text = """
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M()
                {
                    var key = KeyedService.AnyKey;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when GetKeyedService is called
    /// with a variable key rather than the AnyKey constant directly.
    /// </summary>
    [Test]
    public async Task GetKeyedService_WithVariableKey_NoDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp, object key)
                {
                    sp.GetKeyedService<string>(key);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported when GetRequiredKeyedService is called
    /// with AnyKey, since it is not one of the affected APIs documented in the breaking change.
    /// </summary>
    [Test]
    public async Task GetRequiredKeyedService_WithAnyKey_NoDiagnostic()
    {
        const string text = """
            using System;
            using Microsoft.Extensions.DependencyInjection;

            class A
            {
                void M(IServiceProvider sp)
                {
                    sp.GetRequiredKeyedService<string>(KeyedService.AnyKey);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.DependencyInjection.Abstractions")).ConfigureAwait(false);
    }
}
