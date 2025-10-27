using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapGet("/weather", () => { })
    .AddOpenApiOperationTransformer((operation, context, ct) =>
    {
        // Per-endpoint tweaks
        operation.Summary     = "Gets the current weather report.";
        operation.Description = "Returns a short description and emoji.";
        return Task.CompletedTask;
    });

app.Run();