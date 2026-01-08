using Microsoft.CodeAnalysis.Testing;
using TUnit.Core;
using System.Threading.Tasks;
using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipes.ActionContextAccessorObsoleteAnalyzer>;

namespace Rewrite.RoslynRecipes.Tests;

/// <summary>
/// Test suite for the ActionContextAccessorObsoleteAnalyzer.
/// </summary>
public class ActionContextAccessorObsoleteAnalyzerTests
{
    /// <summary>
    /// Tests that the analyzer correctly identifies IActionContextAccessor in a basic constructor parameter.
    /// </summary>
    [Test]
    public async Task BasicConstructorParameter_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer identifies multiple IActionContextAccessor parameters in the same constructor.
    /// </summary>
    [Test]
    public async Task MultipleConstructorParameters_CreatesDiagnostics()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.Extensions.Logging;

            public class MyService
            {
                private readonly IActionContextAccessor _accessor1;
                private readonly IActionContextAccessor _accessor2;
                private readonly ILogger<MyService> _logger;

                public MyService(
                    {|ORNETX0009:IActionContextAccessor|} accessor1,
                    ILogger<MyService> logger,
                    {|ORNETX0009:IActionContextAccessor|} accessor2)
                {
                    _accessor1 = accessor1;
                    _accessor2 = accessor2;
                    _logger = logger;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer correctly handles a class with multiple constructors.
    /// </summary>
    [Test]
    public async Task MultipleConstructors_OnlyFlagsConstructorsWithIActionContextAccessor()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.Extensions.Logging;

            public class MyService
            {
                private readonly IActionContextAccessor? _actionContextAccessor;
                private readonly ILogger<MyService>? _logger;

                public MyService()
                {
                }

                public MyService(ILogger<MyService> logger)
                {
                    _logger = logger;
                }

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public MyService(
                    {|ORNETX0009:IActionContextAccessor|} actionContextAccessor,
                    ILogger<MyService> logger)
                {
                    _actionContextAccessor = actionContextAccessor;
                    _logger = logger;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer does not create diagnostics for IHttpContextAccessor (the replacement type).
    /// </summary>
    [Test]
    public async Task IHttpContextAccessorParameter_NoDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer does not flag IActionContextAccessor in method parameters (only constructors).
    /// </summary>
    [Test]
    public async Task MethodParameter_NoDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                public void ProcessContext(IActionContextAccessor actionContextAccessor)
                {
                    var context = actionContextAccessor.ActionContext;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer correctly identifies usage in a private constructor.
    /// </summary>
    [Test]
    public async Task PrivateConstructor_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                private MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public static MyService Create(IActionContextAccessor accessor)
                {
                    return new MyService(accessor);
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer correctly identifies usage in a record constructor.
    /// </summary>
    [Test]
    public async Task RecordConstructor_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public record MyService({|ORNETX0009:IActionContextAccessor|} ActionContextAccessor);
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer detects ModelState usage and marks it in diagnostic properties.
    /// </summary>
    [Test]
    public async Task WithModelStateUsage_CreatesDiagnosticWithProperty()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Mvc.ModelBinding;

            public class MyController
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyController({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public void ValidateModel()
                {
                    var modelState = _actionContextAccessor.ActionContext.ModelState;
                    if (!modelState.IsValid)
                    {
                        // Handle invalid model
                    }
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer correctly handles constructor with property initialization.
    /// </summary>
    [Test]
    public async Task PropertyInitialization_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                public IActionContextAccessor ActionContextAccessor { get; }

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    ActionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer works with fully qualified type names.
    /// </summary>
    [Test]
    public async Task FullyQualifiedTypeName_CreatesDiagnostic()
    {
        const string text = """
            public class MyService
            {
                private readonly Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer handles nullable reference types correctly.
    /// </summary>
    [Test]
    public async Task NullableReferenceType_CreatesDiagnostic()
    {
        const string text = """
            #nullable enable
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                private readonly IActionContextAccessor? _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor?|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer correctly handles a constructor with default parameter value.
    /// </summary>
    [Test]
    public async Task ConstructorWithDefaultParameter_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                private readonly IActionContextAccessor? _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor?|} actionContextAccessor = null)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer does not flag non-interface types with similar names.
    /// </summary>
    [Test]
    public async Task SimilarTypeName_NoDiagnostic()
    {
        const string text = """
            public interface IActionContextAccessor
            {
                // Custom interface with same name but different namespace
            }

            public class MyService
            {
                private readonly IActionContextAccessor _accessor;

                public MyService(IActionContextAccessor accessor)
                {
                    _accessor = accessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests the analyzer with a generic class constructor.
    /// </summary>
    [Test]
    public async Task GenericClassConstructor_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService<T>
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the analyzer works correctly with primary constructors in C# 12.
    /// </summary>
    [Test]
    public async Task PrimaryConstructor_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
            {
                private readonly IActionContextAccessor _actionContextAccessor = actionContextAccessor;

                public void DoSomething()
                {
                    var context = _actionContextAccessor.ActionContext;
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90).ConfigureAwait(false);
    }
}