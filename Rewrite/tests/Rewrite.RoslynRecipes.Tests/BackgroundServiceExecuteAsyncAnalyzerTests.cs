using Rewrite.RoslynRecipes.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.BackgroundServiceExecuteAsyncAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

public class BackgroundServiceExecuteAsyncAnalyzerTests
{
    // ========================================================================
    // Positive cases
    // ========================================================================

    /// <summary>
    /// Verifies that a diagnostic is reported when a class overrides ExecuteAsync
    /// with a simple async implementation.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_SimpleAsyncOverride_CreatesDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : BackgroundService
            {
                protected override async Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when ExecuteAsync has synchronous code
    /// before the first await, which is the primary scenario affected by the behavioral change.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_WithSyncCodeBeforeAwait_CreatesDiagnostic()
    {
        const string text = """
            using System;
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : BackgroundService
            {
                protected override async Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                {
                    Console.WriteLine("Starting up");
                    await Task.Delay(1000, stoppingToken);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when ExecuteAsync returns Task.CompletedTask
    /// without any await, representing a fully synchronous implementation.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_SynchronousImplementation_CreatesDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : BackgroundService
            {
                protected override Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                {
                    return Task.CompletedTask;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when ExecuteAsync is overridden
    /// in a class that derives from an intermediate base class which itself derives
    /// from BackgroundService.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_IndirectInheritance_CreatesDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            abstract class MyBaseService : BackgroundService
            {
            }

            class MyService : MyBaseService
            {
                protected override async Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported when ExecuteAsync uses expression body syntax.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_ExpressionBody_CreatesDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : BackgroundService
            {
                protected override Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                    => Task.CompletedTask;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is reported for each class that overrides ExecuteAsync
    /// when multiple BackgroundService subclasses exist in the same file.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_MultipleServices_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class ServiceA : BackgroundService
            {
                protected override async Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }

            class ServiceB : BackgroundService
            {
                protected override Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                    => Task.CompletedTask;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    // ========================================================================
    // Negative cases
    // ========================================================================

    /// <summary>
    /// Verifies that no diagnostic is reported for a class implementing IHostedService directly,
    /// which is not affected by the BackgroundService behavioral change.
    /// </summary>
    [Test]
    public async Task IHostedService_DirectImplementation_NoDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : IHostedService
            {
                public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
                public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a class that has a method named ExecuteAsync
    /// but does not derive from BackgroundService.
    /// </summary>
    [Test]
    public async Task ExecuteAsync_NotBackgroundService_NoDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;

            class MyWorker
            {
                public virtual Task ExecuteAsync(CancellationToken stoppingToken)
                    => Task.CompletedTask;
            }

            class MyDerivedWorker : MyWorker
            {
                public override Task ExecuteAsync(CancellationToken stoppingToken)
                    => Task.CompletedTask;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.Net90).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported on a StartAsync override, while the
    /// ExecuteAsync override in the same class still triggers a diagnostic.
    /// </summary>
    [Test]
    public async Task StartAsync_Override_NoDiagnosticOnStartAsync()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            class MyService : BackgroundService
            {
                public override async Task StartAsync(CancellationToken cancellationToken)
                {
                    await base.StartAsync(cancellationToken);
                }

                protected override Task {|ORNETX0018:ExecuteAsync|}(CancellationToken stoppingToken)
                    => Task.CompletedTask;
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is reported for a BackgroundService subclass that does not
    /// override ExecuteAsync (abstract class that leaves it to further subclasses).
    /// </summary>
    [Test]
    public async Task BackgroundService_WithoutExecuteAsyncOverride_NoDiagnostic()
    {
        const string text = """
            using System.Threading;
            using System.Threading.Tasks;
            using Microsoft.Extensions.Hosting;

            abstract class MyBaseService : BackgroundService
            {
                protected void Setup() { }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text,
            Assemblies.Net90.AddPackage("Microsoft.Extensions.Hosting.Abstractions")).ConfigureAwait(false);
    }
}
