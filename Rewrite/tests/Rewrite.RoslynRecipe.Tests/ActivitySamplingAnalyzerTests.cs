using Microsoft.CodeAnalysis.CSharp.Testing;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.ActivitySamplingAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class ActivitySamplingAnalyzerTests
{
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
