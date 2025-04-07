using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Rewrite.MSBuild.Tests;

public class BaseTests
{
    private readonly ServiceProvider _serviceProvider;

    public BaseTests()
    {
        var services = new ServiceCollection();
        services.AddLogging(c => c
            .AddSerilog());
        services.AddSingleton<RecipeManager>();
        services.AddSingleton<NuGet.Common.ILogger, NugetLogger>();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    

    /// <summary>
    /// Constructs / gets object from DI container.
    /// Any constructor arguments that can be satisfied from ServiceProvider will be used, with the rest coming from user supplied parameters 
    /// </summary>
    /// <param name="args">Additional arguments used to construct the object</param>
    /// <typeparam name="T">Type of object to create / retrieve</typeparam>
    protected T CreateObject<T>(params object[] args)
    {
        return ActivatorUtilities.CreateInstance<T>(_serviceProvider, args);
    }

    protected virtual ServiceCollection ConfigureServices(ServiceCollection services)
    {
        return services;
    }
}