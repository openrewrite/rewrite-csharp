using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Rewrite.RoslynRecipe.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpAnalyzerVerifier<Rewrite.RoslynRecipe.WithOpenApiDeprecatedAnalyzer>;

namespace Rewrite.RoslynRecipe.Tests;

public class WithOpenApiDeprecatedAnalyzerTests
{
    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used with a lambda expression.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithLambda_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Gets the current weather report.";
                           operation.Description = "Returns a short description and emoji.";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used without parameters.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithoutParameters_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}();
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used with a simple lambda.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithSimpleLambda_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(operation => operation);
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is chained after other methods.
    /// </summary>
    [Test]
    public async Task WithOpenApiChained_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .RequireAuthorization()
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Protected endpoint";
                           return operation;
                       })
                       .WithName("GetWeather");
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used with method group.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithMethodGroup_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using Microsoft.OpenApi.Models;
            
            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(ConfigureOperation);
                }

                static OpenApiOperation ConfigureOperation(OpenApiOperation operation)
                {
                    operation.Summary = "Gets the weather";
                    return operation;
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used on MapPost.
    /// </summary>
    [Test]
    public async Task WithOpenApiOnMapPost_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapPost("/weather", (WeatherForecast weather) => weather)
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Creates a new weather forecast.";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used on MapPut.
    /// </summary>
    [Test]
    public async Task WithOpenApiOnMapPut_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapPut("/weather/{id}", (int id, WeatherForecast weather) => weather)
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Updates a weather forecast.";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that a diagnostic is created when WithOpenApi is used in a controller endpoint.
    /// </summary>
    [Test]
    public async Task WithOpenApiInController_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using Microsoft.AspNetCore.Mvc;

            [ApiController]
            [Route("[controller]")]
            class WeatherController : ControllerBase
            {
                public void ConfigureEndpoints(WebApplication app)
                {
                    app.MapGet("/api/weather", GetWeather)
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Gets weather from controller";
                           return operation;
                       });
                }

                private WeatherForecast GetWeather()
                {
                    return new WeatherForecast();
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when AddOpenApiOperationTransformer is used (the recommended replacement).
    /// </summary>
    [Test]
    public async Task AddOpenApiOperationTransformer_NoDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Gets the current weather report.";
                           operation.Description = "Returns a short description and emoji.";
                           return Task.CompletedTask;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet100
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that no diagnostic is created when a different WithOpenApi method is used (not from ASP.NET Core).
    /// </summary>
    [Test]
    public async Task NonAspNetCoreWithOpenApi_NoDiagnostic()
    {
        const string text = """
            class CustomClass
            {
                public CustomClass WithOpenApi(string config)
                {
                    return this;
                }
            }

            class Program
            {
                static void Main()
                {
                    var obj = new CustomClass();
                    obj.WithOpenApi("config");
                }
            }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

   

    /// <summary>
    /// Verifies that multiple WithOpenApi calls create multiple diagnostics.
    /// </summary>
    [Test]
    public async Task MultipleWithOpenApiCalls_CreatesMultipleDiagnostics()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Gets weather";
                           return operation;
                       });

                    app.MapPost("/weather", (WeatherForecast weather) => weather)
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Creates weather";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi with expression-bodied lambda creates a diagnostic.
    /// </summary>
    [Test]
    public async Task WithOpenApiExpressionBodiedLambda_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Weather";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi in a complex fluent chain creates a diagnostic.
    /// </summary>
    [Test]
    public async Task WithOpenApiInComplexChain_CreatesDiagnostic()
    {
        const string text = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGroup("/api")
                       .RequireAuthorization()
                       .MapGet("/weather", () => new WeatherForecast())
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Gets weather in API group";
                           return operation;
                       })
                       .WithName("GetWeatherInGroup")
                       .RequireCors();
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyAnalyzerAsync(text, Assemblies.AspNet90
               .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }
}