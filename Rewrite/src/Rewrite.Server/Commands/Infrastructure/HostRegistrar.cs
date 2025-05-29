using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace Rewrite.Remote.Server.Commands.Infrastructure;

public sealed class HostRegistrar : ITypeRegistrar
{
    private readonly HostApplicationBuilder _builder;

    public HostRegistrar(HostApplicationBuilder builder)
    {
        _builder = builder;
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(_builder.Build().Services);
    }

    public void Register(Type service, Type implementation)
    {
        _builder.Services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _builder.Services.AddSingleton(service, implementation);
        if(service.IsAssignableTo(typeof(ICommandModel)))
        {
            _builder.Services.AddSingleton(typeof(ICommandModel), implementation);
        }
		
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        _builder.Services.AddSingleton(service, (provider) => func());
    }
}