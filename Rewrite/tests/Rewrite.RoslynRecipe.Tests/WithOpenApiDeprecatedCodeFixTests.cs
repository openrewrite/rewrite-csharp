using Microsoft.CodeAnalysis.Testing;
using Rewrite.RoslynRecipe.Tests.Verifiers;
using TUnit.Core;
using Verifier = Rewrite.RoslynRecipe.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipe.WithOpenApiDeprecatedAnalyzer,
    Rewrite.RoslynRecipe.WithOpenApiDeprecatedCodeFixProvider>;

namespace Rewrite.RoslynRecipe.Tests;

public class WithOpenApiDeprecatedCodeFixTests
{
    /// <summary>
    /// Verifies that WithOpenApi with lambda is correctly replaced with AddOpenApiOperationTransformer.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithLambda_ReplacedWithAddOpenApiOperationTransformer()
    {
        const string before = """
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

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

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

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi without parameters is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithoutParameters_ReplacedWithDefaultLambda()
    {
        const string before = """
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

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .AddOpenApiOperationTransformer((operation, context, ct) => Task.CompletedTask);
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi with simple lambda is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithSimpleLambda_ReplacedCorrectly()
    {
        const string before = """
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

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation;
                           return Task.CompletedTask;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi in a chain is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiInChain_ReplacedCorrectly()
    {
        const string before = """
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

        const string after = """
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
                       .RequireAuthorization()
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Protected endpoint";
                           return Task.CompletedTask;
                       }).WithName("GetWeather");
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi with method group is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiWithMethodGroup_ReplacedWithWrappedLambda()
    {
        const string before = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

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
            class OpenApiOperation
            {
                public string Summary { get; set; }
            }
            """;

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapGet("/weather", () => new WeatherForecast())
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           ConfigureOperation(operation);
                           return Task.CompletedTask;
                       });
                }

                static OpenApiOperation ConfigureOperation(OpenApiOperation operation)
                {
                    operation.Summary = "Gets the weather";
                    return operation;
                }
            }

            class WeatherForecast { }
            class OpenApiOperation
            {
                public string Summary { get; set; }
            }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that multiple WithOpenApi calls are all fixed.
    /// </summary>
    [Test]
    public async Task MultipleWithOpenApiCalls_AllFixed()
    {
        const string before = """
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

        const string after = """
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
                           operation.Summary = "Gets weather";
                           return Task.CompletedTask;
                       });

                    app.MapPost("/weather", (WeatherForecast weather) => weather)
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Creates weather";
                           return Task.CompletedTask;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet100
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi on MapPost is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiOnMapPost_ReplacedCorrectly()
    {
        const string before = """
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
                           operation.RequestBody.Required = true;
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapPost("/weather", (WeatherForecast weather) => weather)
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Creates a new weather forecast.";
                           operation.RequestBody.Required = true;
                           return Task.CompletedTask;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi on MapPut is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiOnMapPut_ReplacedCorrectly()
    {
        const string before = """
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
                           operation.Parameters[0].Description = "The weather forecast ID";
                           return operation;
                       });
                }
            }

            class WeatherForecast { }
            """;

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapPut("/weather/{id}", (int id, WeatherForecast weather) => weather)
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Updates a weather forecast.";
                           operation.Parameters[0].Description = "The weather forecast ID";
                           return Task.CompletedTask;
                       });
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi on MapDelete is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiOnMapDelete_ReplacedCorrectly()
    {
        const string before = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapDelete("/weather/{id}", (int id) => Results.NoContent())
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Deletes a weather forecast.";
                           operation.Responses["204"].Description = "No content";
                           return operation;
                       });
                }
            }

            class Results
            {
                public static object NoContent() => new { };
            }
            """;

        const string after = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            class Program
            {
                static void Main()
                {
                    var builder = WebApplication.CreateBuilder();
                    var app = builder.Build();

                    app.MapDelete("/weather/{id}", (int id) => Results.NoContent())
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Deletes a weather forecast.";
                           operation.Responses["204"].Description = "No content";
                           return Task.CompletedTask;
                       });
                }
            }

            class Results
            {
                public static object NoContent() => new { };
            }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verifies that WithOpenApi in complex fluent chain is correctly replaced.
    /// </summary>
    [Test]
    public async Task WithOpenApiInComplexChain_ReplacedCorrectly()
    {
        const string before = """
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
                       .WithTags("Weather")
                       .{|ORNETX0008:WithOpenApi|}(operation =>
                       {
                           operation.Summary = "Gets weather in API group";
                           operation.Tags.Add(new() { Name = "WeatherAPI" });
                           return operation;
                       })
                       .WithName("GetWeatherInGroup")
                       .RequireCors();
                }
            }

            class WeatherForecast { }
            """;

        const string after = """
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
                       .WithTags("Weather")
                       .AddOpenApiOperationTransformer((operation, context, ct) =>
                       {
                           operation.Summary = "Gets weather in API group";
                           operation.Tags.Add(new() { Name = "WeatherAPI" });
                           return Task.CompletedTask;
                       })
                       .WithName("GetWeatherInGroup")
                       .RequireCors();
                }
            }

            class WeatherForecast { }
            """;

        await Verifier.VerifyCodeFixAsync(before, after, Assemblies.AspNet90
                .AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }
}