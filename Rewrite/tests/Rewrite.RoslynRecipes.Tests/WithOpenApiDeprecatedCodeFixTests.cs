using Rewrite.RoslynRecipes.Tests.Verifiers;
using Verifier = Rewrite.RoslynRecipes.Tests.Verifiers.CSharpCodeFixVerifier<
    Rewrite.RoslynRecipes.WithOpenApiDeprecatedAnalyzer,
    Rewrite.RoslynRecipes.WithOpenApiDeprecatedCodeFixProvider>;

namespace Rewrite.RoslynRecipes.Tests;

public class WithOpenApiDeprecatedCodeFixTests
{
    /// <summary>
    /// Tests that WithOpenApi without parameters is correctly refactored with a default lambda.
    /// </summary>
    [Test]
    public async Task WithOpenApi_NoParameters_FixedWithDefaultLambda()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "test")
               .{|ORNETX0008:WithOpenApi|}();
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "test")
               .AddOpenApiOperationTransformer((operation, context, ct) => Task.CompletedTask);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
                Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }


    /// <summary>
    /// Tests that WithOpenApi with statement lambda containing property assignments is correctly refactored.
    /// </summary>
    [Test]
    public async Task WithOpenApi_MultipleStatementsInLambda()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "sunny")
               .{|ORNETX0008:WithOpenApi|}(operation =>
               {
                   operation.Summary = "test";
                   return operation;
               });
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "sunny")
               .AddOpenApiOperationTransformer((operation, context, ct) =>
               {
                   operation.Summary = "test";
                   return Task.CompletedTask;
               });
 
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that WithOpenApi with method group is correctly refactored by wrapping the method group.
    /// </summary>
    [Test]
    public async Task WithOpenApi_MethodGroup_WrappedInLambda()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using Microsoft.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "test")
               .{|ORNETX0008:WithOpenApi|}(ConfigureOperation);

            static OpenApiOperation ConfigureOperation(OpenApiOperation operation)
            {
                operation.Summary = "Test";
                return operation;
            }
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using Microsoft.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/", () => "test")
               .AddOpenApiOperationTransformer((operation, context, ct) => { ConfigureOperation(operation); return Task.CompletedTask; });

            static OpenApiOperation ConfigureOperation(OpenApiOperation operation)
            {
                operation.Summary = "Test";
                return operation;
            }
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that WithOpenApi with simple lambda (single parameter) is correctly refactored.
    /// </summary>
    [Test]
    public async Task WithOpenApi_SimpleLambda_FixedToAddOpenApiOperationTransformer()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapPost("/data", (object data) => data)
               .{|ORNETX0008:WithOpenApi|}(op => op);
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapPost("/data", (object data) => data)
               .AddOpenApiOperationTransformer((operation, context, ct) => Task.CompletedTask);
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that multiple WithOpenApi calls in the same file are all correctly refactored.
    /// </summary>
    [Test]
    public async Task MultipleWithOpenApiCalls_AllFixed()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/first", () => "first")
               .{|ORNETX0008:WithOpenApi|}(operation =>
               {
                   operation.Summary = "First endpoint";
                   return operation;
               });

            app.MapPost("/second", () => "second")
               .{|ORNETX0008:WithOpenApi|}(operation =>
               {
                   operation.Summary = "Second endpoint";
                   return operation;
               });
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/first", () => "first")
               .AddOpenApiOperationTransformer((operation, context, ct) =>
               {
                   operation.Summary = "First endpoint";
                   return Task.CompletedTask;
               });

            app.MapPost("/second", () => "second")
               .AddOpenApiOperationTransformer((operation, context, ct) =>
               {
                   operation.Summary = "Second endpoint";
                   return Task.CompletedTask;
               });
            
            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }



    /// <summary>
    /// Tests that WithOpenApi with parenthesized lambda with multiple parameters is correctly handled.
    /// </summary>
    [Test]
    public async Task WithOpenApi_ParenthesizedLambda_FixedCorrectly()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/test", () => "result")
               .{|ORNETX0008:WithOpenApi|}((operation) =>
               {
                   return operation;
               });
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/test", () => "result")
               .AddOpenApiOperationTransformer((operation, context, ct) =>
               {
                   return Task.CompletedTask;
               });

            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Tests that WithOpenApi with inline modifications is correctly refactored.
    /// </summary>
    [Test]
    public async Task WithOpenApi_InlineModifications_FixedCorrectly()
    {
        const string source = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/inline", () => "test")
               .{|ORNETX0008:WithOpenApi|}(op => { op.Deprecated = true; return op; });
            """;

        const string fixedSource = """
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.OpenApi;
            using System.Threading.Tasks;

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/inline", () => "test")
               .AddOpenApiOperationTransformer((operation, context, ct) => { operation.Deprecated = true; return Task.CompletedTask; });

            """;

        await Verifier.VerifyCodeFixAsync(source, fixedSource,
            Assemblies.AspNet100.AddPackage("Microsoft.AspNetCore.OpenApi"))
            .ConfigureAwait(false);
    }
    


}