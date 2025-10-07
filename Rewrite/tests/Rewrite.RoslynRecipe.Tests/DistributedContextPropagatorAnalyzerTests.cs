using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.DistributedContextPropagatorAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class DistributedContextPropagatorAnalyzerTests
{
    [Test]
    public async Task CreateDefaultPropagator_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    var propagator = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyRead_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    var propagator = DistributedContextPropagator.{|ORNETX0003:Current|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyWrite_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    DistributedContextPropagator.{|ORNETX0003:Current|} = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyAssignedToCreatePreW3CPropagator_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    DistributedContextPropagator.Current = DistributedContextPropagator.CreatePreW3CPropagator();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CreateDefaultPropagatorUsedInAssignment_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    DistributedContextPropagator.{|ORNETX0003:Current|} = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyInCondition_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    if (DistributedContextPropagator.{|ORNETX0003:Current|} != null)
                    {
                        // do something
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyPassedAsArgument_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    ProcessPropagator(DistributedContextPropagator.{|ORNETX0003:Current|});
                }

                void ProcessPropagator(DistributedContextPropagator propagator)
                {
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CreateDefaultPropagatorInFieldInitializer_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                private DistributedContextPropagator _propagator = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentPropertyInFieldInitializer_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                private DistributedContextPropagator _propagator = DistributedContextPropagator.{|ORNETX0003:Current|};
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task MultipleUsages_CreateMultipleDiagnostics()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    var default1 = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
                    var current = DistributedContextPropagator.{|ORNETX0003:Current|};
                    var default2 = DistributedContextPropagator.{|ORNETX0003:CreateDefaultPropagator|}();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task NonDistributedContextPropagatorType_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class MyPropagator
            {
                public static MyPropagator Current { get; set; }
                public static MyPropagator CreateDefaultPropagator() => new MyPropagator();
            }

            class TestClass
            {
                void Method()
                {
                    var propagator = MyPropagator.CreateDefaultPropagator();
                    var current = MyPropagator.Current;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CreatePreW3CPropagator_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    var propagator = DistributedContextPropagator.CreatePreW3CPropagator();
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    [Test]
    public async Task CurrentAssignedToVariable_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Method()
                {
                    var custom = DistributedContextPropagator.CreatePreW3CPropagator();
                    DistributedContextPropagator.{|ORNETX0003:Current|} = custom;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }
}
