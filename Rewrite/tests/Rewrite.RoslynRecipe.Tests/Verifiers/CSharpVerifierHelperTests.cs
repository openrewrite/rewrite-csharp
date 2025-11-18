using TUnit.Core;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace Rewrite.RoslynRecipe.Tests.Verifiers;

/// <summary>
/// Tests for the CSharpVerifierHelper utility class.
/// </summary>
public class CSharpVerifierHelperTests
{
    /// <summary>
    /// Tests that simple analyzer boundary markers are correctly stripped.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_SimpleMarker_Stripped()
    {
        const string input = "{|ORNETX0009:IActionContextAccessor|}";
        const string expected = "IActionContextAccessor";

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests that multiple analyzer boundary markers in the same string are all stripped.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_MultipleMarkers_AllStripped()
    {
        const string input = "private readonly {|ORNETX0009:IActionContextAccessor|} _accessor; public {|ORNETX0009:IActionContextAccessor|} Prop { get; }";
        const string expected = "private readonly IActionContextAccessor _accessor; public IActionContextAccessor Prop { get; }";

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests that code without analyzer boundary markers remains unchanged.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_NoMarkers_Unchanged()
    {
        const string input = "public class MyClass { private readonly IActionContextAccessor _accessor; }";
        const string expected = "public class MyClass { private readonly IActionContextAccessor _accessor; }";

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests that analyzer boundary markers with different diagnostic IDs are correctly stripped.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_DifferentDiagnosticIds_AllStripped()
    {
        const string input = "{|CS0001:Error1|} and {|ORNETX0009:IActionContextAccessor|} and {|IDE0001:Warning|}";
        const string expected = "Error1 and IActionContextAccessor and Warning";

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests that multiline code with analyzer boundary markers is correctly processed.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_MultilineCode_CorrectlyProcessed()
    {
        const string input = """
            public class MyController
            {
                private readonly {|ORNETX0009:IActionContextAccessor|} _actionContextAccessor;

                public MyController({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        const string expected = """
            public class MyController
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyController(IActionContextAccessor actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }


    /// <summary>
    /// Tests that nested curly braces don't interfere with marker stripping.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_NestedBraces_CorrectlyHandled()
    {
        const string input = "public void Method() { var x = new {|ORNETX0009:IActionContextAccessor|}(); }";
        const string expected = "public void Method() { var x = new IActionContextAccessor(); }";

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    /// <summary>
    /// Tests complex real-world scenario with multiple markers and varied formatting.
    /// </summary>
    [Test]
    public async Task StripAnalyzerBoundaryMarkers_ComplexScenario_AllMarkersStripped()
    {
        const string input = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class Startup
            {
                public void ConfigureServices(IServiceCollection services)
                {
                    services.AddSingleton<{|ORNETX0009:IActionContextAccessor|}, ActionContextAccessor>();
                    services.AddScoped<{|ORNETX0009:IActionContextAccessor|}, ActionContextAccessor>();

                    {|ORNETX0009:IActionContextAccessor|} accessor = null;
                    var type = typeof({|ORNETX0009:IActionContextAccessor|});
                }
            }
            """;

        const string expected = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class Startup
            {
                public void ConfigureServices(IServiceCollection services)
                {
                    services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                    services.AddScoped<IActionContextAccessor, ActionContextAccessor>();

                    IActionContextAccessor accessor = null;
                    var type = typeof(IActionContextAccessor);
                }
            }
            """;

        var result = CSharpVerifierHelper.StripAnalyzerBoundaryMarkers(input);

        await Assert.That(result).IsEqualTo(expected);
    }
}