using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace Rewrite.Server.Commands.Infrastructure;

public sealed class HostResolver : ITypeResolver, IDisposable
{
    private readonly IHost _host;

    public HostResolver(IHost host)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        if (type == typeof(IHost))
            return _host;
        return _host.Services.GetService(type);
    }

    public void Dispose()
    {
        if (_host is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
