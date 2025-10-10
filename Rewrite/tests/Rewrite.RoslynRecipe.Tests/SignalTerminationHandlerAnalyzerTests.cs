using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.SignalTerminationHandlerAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

/// <summary>
/// Tests for the SignalTerminationHandlerAnalyzer that detects registrations of AppDomain.ProcessExit
/// and AssemblyLoadContext.Unloading event handlers, which have reliability limitations in .NET 10.0+.
/// </summary>
public class SignalTerminationHandlerAnalyzerTests
{
    [Test]
    public async Task ProcessExitEventRegistration_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                void Method()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += OnProcessExit;
                }

                void OnProcessExit(object sender, EventArgs e)
                {
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task ProcessExitEventWithLambda_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                void Method()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += (sender, args) =>
                    {
                        Console.WriteLine("Exiting");
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task UnloadingEventRegistration_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                void Method()
                {
                    AssemblyLoadContext.Default.{|ORNETX0007:Unloading|} += OnUnloading;
                }

                void OnUnloading(AssemblyLoadContext context)
                {
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task UnloadingEventWithLambda_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                void Method()
                {
                    AssemblyLoadContext.Default.{|ORNETX0007:Unloading|} += context =>
                    {
                        Console.WriteLine("Unloading");
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task ProcessExitInConstructor_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                public TestClass()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += (s, e) => { };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task UnloadingInConstructor_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                public TestClass()
                {
                    AssemblyLoadContext.Default.{|ORNETX0007:Unloading|} += context => { };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task BothEventsRegistered_CreatesBothDiagnostics()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                void Method()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += OnProcessExit;
                    AssemblyLoadContext.Default.{|ORNETX0007:Unloading|} += OnUnloading;
                }

                void OnProcessExit(object sender, EventArgs e) { }
                void OnUnloading(AssemblyLoadContext context) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task ProcessExitInStaticMethod_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                static void Main()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += OnExit;
                }

                static void OnExit(object sender, EventArgs e) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task UnloadingOnCustomContext_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                void Method()
                {
                    var context = new AssemblyLoadContext("MyContext");
                    context.{|ORNETX0007:Unloading|} += OnUnloading;
                }

                void OnUnloading(AssemblyLoadContext ctx) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task ProcessExitEventUnregistration_NoDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                void Method()
                {
                    AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
                }

                void OnProcessExit(object sender, EventArgs e) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task UnloadingEventUnregistration_NoDiagnostic()
    {
        const string text = """
            using System;
            using System.Runtime.Loader;

            class TestClass
            {
                void Method()
                {
                    AssemblyLoadContext.Default.Unloading -= OnUnloading;
                }

                void OnUnloading(AssemblyLoadContext context) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task NonEventProperty_NoDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                void Method()
                {
                    var domain = AppDomain.CurrentDomain;
                    var name = domain.FriendlyName;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CustomProcessExitEvent_NoDiagnostic()
    {
        const string text = """
            using System;

            class MyDomain
            {
                public event EventHandler ProcessExit;
            }

            class TestClass
            {
                void Method()
                {
                    var domain = new MyDomain();
                    domain.ProcessExit += (s, e) => { };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task ProcessExitInFieldInitializer_CreatesWarningDiagnostic()
    {
        const string text = """
            using System;

            class TestClass
            {
                private EventHandler _handler = (s, e) => { };

                public TestClass()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += _handler;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task MultipleProcessExitRegistrations_CreatesMultipleDiagnostics()
    {
        const string text = """
            using System;

            class TestClass
            {
                void Method()
                {
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += Handler1;
                    AppDomain.CurrentDomain.{|ORNETX0006:ProcessExit|} += Handler2;
                }

                void Handler1(object s, EventArgs e) { }
                void Handler2(object s, EventArgs e) { }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }
}
