using Microsoft.CodeAnalysis.Testing;
using TUnit.Core;
using System.Threading.Tasks;
using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.ActionContextAccessorObsoleteAnalyzer,
    Rewrite.RoslynRecipes.ActionContextAccessorObsoleteCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

/// <summary>
/// Test suite for the ActionContextAccessorObsoleteCodeFixProvider.
/// </summary>
public class ActionContextAccessorObsoleteCodeFixTests
{
    /// <summary>
    /// Tests basic replacement of IActionContextAccessor with IHttpContextAccessor in constructor and field.
    /// </summary>
    [Test]
    public async Task BasicConstructorParameter_ReplacesTypeAndName()
    {
        const string source = """
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

        const string fixedSource = """
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

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement of ActionContext.HttpContext property access.
    /// </summary>
    [Test]
    public async Task ActionContextHttpContextAccess_ReplacesWithHttpContextAccessor()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public HttpContext? GetHttpContext()
                {
                    var actionContext = _actionContextAccessor.ActionContext;
                    return actionContext.HttpContext;
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public HttpContext? GetHttpContext()
                {
                    return _httpContextAccessor.HttpContext;
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement of ActionContext.ActionDescriptor property access.
    /// </summary>
    [Test]
    public async Task ActionContextActionDescriptorAccess_ReplacesWithGetEndpointMetadata()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Mvc.Abstractions;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public ActionDescriptor? GetActionDescriptor()
                {
                    var actionContext = _actionContextAccessor.ActionContext;
                    return actionContext.ActionDescriptor;
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Mvc.Abstractions;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public ActionDescriptor? GetActionDescriptor()
                {
                    return _httpContextAccessor.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>();
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement of ActionContext.RouteData property access.
    /// </summary>
    [Test]
    public async Task ActionContextRouteDataAccess_ReplacesWithGetRouteData()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Routing;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public RouteData? GetRouteData()
                {
                    var actionContext = _actionContextAccessor.ActionContext;
                    return actionContext.RouteData;
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Routing;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public RouteData? GetRouteData()
                {
                    return _httpContextAccessor.HttpContext?.GetRouteData();
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

//     /// <summary>
//     /// Tests that ModelState usage adds a TODO comment instead of refactoring.
//     /// </summary>
//     [Test]
//     public async Task ModelStateUsage_AddsTodoComment()
//     {
//         const string source = """
//             using Microsoft.AspNetCore.Mvc.Infrastructure;
//             using Microsoft.AspNetCore.Mvc.ModelBinding;
//
//             public class MyController
//             {
//                 private readonly IActionContextAccessor _actionContextAccessor;
//
//                 public MyController({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
//                 {
//                     _actionContextAccessor = actionContextAccessor;
//                 }
//
//                 public bool IsModelValid()
//                 {
//                     var modelState = _actionContextAccessor.ActionContext.ModelState;
//                     return modelState.IsValid;
//                 }
//             }
//             """;
//
//         const string fixedSource = """
//             using Microsoft.AspNetCore.Mvc.Infrastructure;
//             using Microsoft.AspNetCore.Mvc.ModelBinding;
//
//             public class MyController
//             {
//                 private readonly IActionContextAccessor _actionContextAccessor;
//
//                 public MyController(/* todo: obsolete type */ IActionContextAccessor actionContextAccessor)
//                 {
//                     _actionContextAccessor = actionContextAccessor;
//                 }
//
//                 public bool IsModelValid()
//                 {
//                     var modelState = _actionContextAccessor.ActionContext.ModelState;
//                     return modelState.IsValid;
//                 }
//             }
//             """;
//
//         await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
//             .ConfigureAwait(false);
//     }

    /// <summary>
    /// Tests replacement with property initialization instead of field.
    /// </summary>
    [Test]
    public async Task PropertyInitialization_ReplacesTypeAndName()
    {
        const string source = """
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

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                public IHttpContextAccessor HttpContextAccessor { get; }

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    HttpContextAccessor = httpContextAccessor;
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests complex scenario with multiple property accesses.
    /// </summary>
    [Test]
    public async Task ComplexScenario_ReplacesAllUsages()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Mvc.Abstractions;
            using Microsoft.AspNetCore.Routing;
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public void ProcessRequest()
                {
                    var actionContext = _actionContextAccessor.ActionContext;

                    var httpContext = actionContext.HttpContext;
                    var routeData = actionContext.RouteData;
                    var actionDescriptor = actionContext.ActionDescriptor;

                    // Use the values
                    if (httpContext != null)
                    {
                        var user = httpContext.User;
                    }

                    if (routeData != null)
                    {
                        var values = routeData.Values;
                    }

                    if (actionDescriptor != null)
                    {
                        var displayName = actionDescriptor.DisplayName;
                    }
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Mvc.Abstractions;
            using Microsoft.AspNetCore.Routing;
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public void ProcessRequest()
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    var routeData = _httpContextAccessor.HttpContext?.GetRouteData();
                    var actionDescriptor = _httpContextAccessor.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>();

                    // Use the values
                    if (httpContext != null)
                    {
                        var user = httpContext.User;
                    }

                    if (routeData != null)
                    {
                        var values = routeData.Values;
                    }

                    if (actionDescriptor != null)
                    {
                        var displayName = actionDescriptor.DisplayName;
                    }
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that the fix works with fully qualified type names.
    /// </summary>
    [Test]
    public async Task FullyQualifiedTypeName_ReplacesCorrectly()
    {
        const string source = """
            public class MyService
            {
                private readonly Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }
            }
            """;

        const string fixedSource = """
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

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement with nullable reference types.
    /// </summary>
    [Test]
    public async Task NullableReferenceType_ReplacesCorrectly()
    {
        const string source = """
            #nullable enable
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public class MyService
            {
                private readonly IActionContextAccessor? _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor?|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public string? GetValue()
                {
                    return _actionContextAccessor.ActionContext?.HttpContext?.TraceIdentifier;
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IHttpContextAccessor? _httpContextAccessor;

                public MyService(IHttpContextAccessor? httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public string? GetValue()
                {
                    return _httpContextAccessor.HttpContext?.TraceIdentifier;
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement with inline ActionContext property access without intermediate variable.
    /// </summary>
    [Test]
    public async Task InlinePropertyAccess_ReplacesCorrectly()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.AspNetCore.Http;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;

                public MyService({|ORNETX0009:IActionContextAccessor|} actionContextAccessor)
                {
                    _actionContextAccessor = actionContextAccessor;
                }

                public HttpContext? GetHttpContext()
                {
                    return _actionContextAccessor.ActionContext.HttpContext;
                }

                public string? GetRouteValue(string key)
                {
                    return _actionContextAccessor.ActionContext.RouteData.Values[key]?.ToString();
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Routing;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;

                public MyService(IHttpContextAccessor httpContextAccessor)
                {
                    _httpContextAccessor = httpContextAccessor;
                }

                public HttpContext? GetHttpContext()
                {
                    return _httpContextAccessor.HttpContext;
                }

                public string? GetRouteValue(string key)
                {
                    return _httpContextAccessor.HttpContext?.GetRouteData().Values[key]?.ToString();
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement in a record with primary constructor.
    /// </summary>
    [Test]
    public async Task RecordPrimaryConstructor_ReplacesCorrectly()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;

            public record MyService({|ORNETX0009:IActionContextAccessor|} ActionContextAccessor);
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;

            public record MyService(IHttpContextAccessor HttpContextAccessor);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests replacement with multiple constructor parameters including IActionContextAccessor.
    /// </summary>
    [Test]
    public async Task MultipleConstructorParameters_OnlyReplacesActionContextAccessor()
    {
        const string source = """
            using Microsoft.AspNetCore.Mvc.Infrastructure;
            using Microsoft.Extensions.Logging;

            public class MyService
            {
                private readonly IActionContextAccessor _actionContextAccessor;
                private readonly ILogger<MyService> _logger;

                public MyService(
                    {|ORNETX0009:IActionContextAccessor|} actionContextAccessor,
                    ILogger<MyService> logger)
                {
                    _actionContextAccessor = actionContextAccessor;
                    _logger = logger;
                }

                public void LogHttpContext()
                {
                    var httpContext = _actionContextAccessor.ActionContext.HttpContext;
                    _logger.LogInformation("Request: {TraceId}", httpContext.TraceIdentifier);
                }
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Http;
            using Microsoft.Extensions.Logging;

            public class MyService
            {
                private readonly IHttpContextAccessor _httpContextAccessor;
                private readonly ILogger<MyService> _logger;

                public MyService(
                    IHttpContextAccessor httpContextAccessor,
                    ILogger<MyService> logger)
                {
                    _httpContextAccessor = httpContextAccessor;
                    _logger = logger;
                }

                public void LogHttpContext()
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    _logger.LogInformation("Request: {TraceId}", httpContext.TraceIdentifier);
                }
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource, Assemblies.AspNet90)
            .ConfigureAwait(false);
    }
}