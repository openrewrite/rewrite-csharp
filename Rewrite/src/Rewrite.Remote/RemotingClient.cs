using System.Net.Sockets;
using Rewrite.Core;

namespace Rewrite.Remote;

public class RemotingClient : IRemotingClient
{
    private readonly RemotingMessenger _messenger;
    private readonly Func<NetworkStream> _streamSupplier;

    internal RemotingClient(RemotingContext context, Socket socket)
    {
        _messenger = new RemotingMessenger(context);
        _streamSupplier = () => new NetworkStream(socket);
    }

    public void Hello()
    {
        var stream = _streamSupplier();
        lock (stream)
        {
            _messenger.SendRequest(stream, "hello");
        }
    }

    public string Print(Cursor cursor)
    {
        var stream = _streamSupplier();
        lock (stream)
        {
            return _messenger.SendPrintRequest(stream, cursor);
        }
    }

    public void Reset()
    {
        var stream = _streamSupplier();
        lock (stream)
        {
            _messenger.SendResetRequest(stream);
        }
    }

    public IList<SourceFile?> RunRecipe(string recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles)
    {
        var stream = _streamSupplier();
        lock (stream)
        {
            return _messenger.SendRunRecipeRequest(stream, recipe, options, sourceFiles);
        }
    }
}