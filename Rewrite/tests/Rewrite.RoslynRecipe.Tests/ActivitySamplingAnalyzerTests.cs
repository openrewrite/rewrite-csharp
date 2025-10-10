using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.ActivitySamplingAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class ActivitySamplingAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is directly returned from a Sample delegate.
    /// </summary>
    [Test]
    public async Task DirectReturnPropagationData_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return ActivitySamplingResult.PropagationData;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is returned conditionally within a Sample delegate.
    /// </summary>
    [Test]
    public async Task ConditionalReturnWithPropagationData_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            if (options.Parent == default)
                                return ActivitySamplingResult.AllDataAndRecorded;

                            return ActivitySamplingResult.PropagationData;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is assigned to a local variable and then returned.
    /// </summary>
    [Test]
    public async Task LocalVariableAssignedPropagationData_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            var result = ActivitySamplingResult.PropagationData;
                            return result;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is used in a local variable initializer and then returned.
    /// </summary>
    [Test]
    public async Task LocalVariableWithInitializer_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            ActivitySamplingResult result = ActivitySamplingResult.PropagationData;
                            return result;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when a method returns ActivitySamplingResult.PropagationData and is called from a Sample delegate.
    /// </summary>
    [Test]
    public async Task MethodCallReturn_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return GetSamplingResult(options);
                        }|}
                    };
                }

                ActivitySamplingResult GetSamplingResult(ActivityCreationOptions<ActivityContext> options)
                {
                    return ActivitySamplingResult.PropagationData;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is returned via a ternary expression.
    /// </summary>
    [Test]
    public async Task TernaryExpressionReturn_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return options.Parent == default
                                ? ActivitySamplingResult.AllDataAndRecorded
                                : ActivitySamplingResult.PropagationData;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when ActivitySamplingResult.PropagationData is returned via a switch expression.
    /// </summary>
    [Test]
    public async Task SwitchExpressionReturn_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return options.Kind switch
                            {
                                ActivityKind.Internal => ActivitySamplingResult.PropagationData,
                                _ => ActivitySamplingResult.AllDataAndRecorded
                            };
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when a method group returning ActivitySamplingResult is assigned to the Sample property.
    /// </summary>
    [Test]
    public async Task MethodGroupAssignment_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = SampleMethod|}
                    };
                }

                ActivitySamplingResult SampleMethod(ref ActivityCreationOptions<ActivityContext> options)
                {
                    return ActivitySamplingResult.AllDataAndRecorded;
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an anonymous method returning ActivitySamplingResult.PropagationData is assigned to the Sample property.
    /// </summary>
    [Test]
    public async Task AnonymousMethod_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = delegate(ref ActivityCreationOptions<ActivityContext> options)
                        {
                            return ActivitySamplingResult.PropagationData;
                        }|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when only ActivitySamplingResult.AllDataAndRecorded is returned.
    /// </summary>
    [Test]
    public async Task OnlyAllDataAndRecorded_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return ActivitySamplingResult.AllDataAndRecorded;
                        }
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when only ActivitySamplingResult.None is returned.
    /// </summary>
    [Test]
    public async Task OnlyNone_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                        {
                            return ActivitySamplingResult.None;
                        }
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when the Sample property is on a non-ActivityListener type.
    /// </summary>
    [Test]
    public async Task NonActivityListenerSampleProperty_NoDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class MyClass
            {
                public object Sample { get; set; }
            }

            class TestClass
            {
                void Setup()
                {
                    var obj = new MyClass
                    {
                        Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.PropagationData
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when the Sample property is assigned outside of an object initializer with PropagationData.
    /// </summary>
    [Test]
    public async Task AssignmentOutsideObjectInitializer_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true
                    };

                    {|ORNETX0002:listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
                    {
                        return ActivitySamplingResult.PropagationData;
                    }|};
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when an expression-bodied lambda returns ActivitySamplingResult.PropagationData.
    /// </summary>
    [Test]
    public async Task ExpressionBodiedLambda_CreatesDiagnostic()
    {
        const string text = """
            using System.Diagnostics;

            class TestClass
            {
                void Setup()
                {
                    var listener = new ActivityListener
                    {
                        ShouldListenTo = _ => true,
                        {|ORNETX0002:Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.PropagationData|}
                    };
                }
            }
            """;

        await Verifier.VerifyAnalyzerDotnet100Async(text).ConfigureAwait(false);
    }
}
