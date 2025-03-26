using System.Formats.Cbor;
using System.Net.Sockets;
using PeterO.Cbor;
using Rewrite.Core;
using Serilog;

namespace Rewrite.Remote;

public class RemotingMessenger
{
    private static readonly ILogger log = Log.Logger.ForContext<RemotingMessenger>();

    private const int Ok = 0;
    private const int Error = 1;

    private RemotingContext _context;

    private readonly IDictionary<string, Func<NetworkStream, RemotingContext, CancellationToken, Task>>?
        _additionalHandlers;

    public static Tree? _state;
    public readonly List<Recipe> _recipes = [];

    public RemotingMessenger(RemotingContext context,
        IDictionary<string, Func<NetworkStream, RemotingContext, CancellationToken, Task>>? additionalHandlers = null)
    {
        _context = context;
        _additionalHandlers = additionalHandlers;
    }

    public async Task<bool> ProcessRequest(NetworkStream stream, CancellationToken cancellationToken)
    {
        var command = CBORObject.Read(stream).AsString();
        log.Debug($"Received a new client[{stream.Socket.RemoteEndPoint}]'s request command[{command}]");

        switch (command)
        {
            case "hello":
                log.Information("Handling \"hello\" Request");
                CBORObject.Read(stream);
                CBORObject.Write((int)RemotingMessageType.Response, stream);
                CBORObject.Write(Ok, stream);
                break;
            case "reset":
                log.Information("Handling \"reset\" Request");
                _state = null;
                _context = _context.Copy();
                _context.Connect(stream.Socket);
                ReadCommandEnd(stream);
                _recipes.Clear();
                CBORObject.Write((int)RemotingMessageType.Response, stream);
                CBORObject.Write(Ok, stream);
                break;
            case "load-recipe":
                LoadRecipe(stream);
                ReadCommandEnd(stream);
                break;
            case "run-recipe-visitor":
                // this already reads command end
                RunRecipe(stream);
                break;
            case "print":
                Print(stream);
                ReadCommandEnd(stream);
                break;
            default:
                if (_additionalHandlers != null && _additionalHandlers.TryGetValue(command, out var handler))
                    await handler(stream, _context, cancellationToken);
                else
                    throw new NotImplementedException("Unsupported command: " + command);

                break;
        }

        return true;
    }

    private void ProcessNestedRequest(NetworkStream stream)
    {
        AsyncHelper.RunSync(async () =>
        {
            await ProcessRequest(stream, CancellationToken.None);
            SendEndMessage(stream);
            await stream.FlushAsync();
        });
    }

    private static void ReadCommandEnd(NetworkStream stream)
    {
        CBORObject.Read(stream);
    }

    private void LoadRecipe(NetworkStream stream)
    {
        var recipeId = CBORObject.Read(stream).AsString();
        var recipeOptions = CBORObject.Read(stream);

        log.Information($"Handling \"LoadRecipe\" Request for Id[{recipeId}] with options {recipeOptions}");

        var recipe = _context.NewRecipe(recipeId, recipeOptions);
        _recipes.Add(recipe);

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        CBORObject.Write(_recipes.Count - 1, stream);
    }


    private void RunRecipe(NetworkStream stream)
    {
        var recipeIndex = CBORObject.Read(stream).AsInt32();
        var recipe = _recipes[recipeIndex];

        log.Information($"Handling \"RunRecipe\" Request for index {recipe}");

        var inputStream = RemoteUtils.ReadToCommandEnd(stream);

        var received = ReceiveTree(_context, inputStream, _state);
        var ctx = new InMemoryExecutionContext();
        RemotingExecutionContextView.View(ctx).RemotingContext = _context;
        var treeVisitor = recipe.GetVisitor();

        if (received is SourceFile sf && treeVisitor.IsAcceptable(sf, ctx))
        {
            _state = treeVisitor.Visit(sf, ctx);
        }
        else
        {
            _state = received;
        }

        if (_state == null)
        {
            throw new InvalidOperationException("State is null");
        }

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        SendTree(_context, stream, _state, received);
    }

    public static void SendTree<T>(RemotingContext context, Stream stream, T after, T? before) where T : Tree
    {
        var outputStream = new MemoryStream();
        var senderContext = context.NewSenderContext(outputStream);
        senderContext.SendTree(after, before);
        outputStream.Position = 0;

        var cborWriter = new CborWriter();
        cborWriter.WriteByteString(outputStream.ToArray());
        var cborData = cborWriter.Encode();
        stream.Write(cborData, 0, cborData.Length);
    }

    public static T ReceiveTree<T>(RemotingContext context, Stream stream, T? before) where T : Tree
    {
        var bytes = CBORObject.Read(stream).GetByteString()!;
        var receiverContext = context.NewReceiverContext(new MemoryStream(bytes));
        return receiverContext.ReceiveTree(before);
    }

    private void Print(NetworkStream stream)
    {
        var inputStream = RemoteUtils.ReadToCommandEnd(stream);

        var received = ReceiveTree(_context, inputStream, default(Tree));
        var rootCursor = new Cursor(null, Cursor.ROOT_VALUE);
        var ctx = new InMemoryExecutionContext();
        RemotingExecutionContextView.View(ctx).RemotingContext = _context;
        var print = received.Print(new Cursor(rootCursor, received), new PrintOutputCapture<int>(0));

        CBORObject.Write((int)RemotingMessageType.Response, stream);
        CBORObject.Write(Ok, stream);
        CBORObject.Write(print, stream);
    }

    public void SendRequest(NetworkStream stream, string command)
    {
        stream.WriteByte((byte)RemotingMessageType.Request);
        CBORObject.Write(command, stream);
        SendEndMessage(stream);
        stream.Flush();
    }

    public void SendRequest(NetworkStream stream, string command, params object[] args)
    {
        stream.WriteByte((byte)RemotingMessageType.Request);
        CBORObject.Write(command, stream);
        foreach (var arg in args)
        {
            // FIXME serialize properly
            CBORObject.Write(arg, stream);
        }

        SendEndMessage(stream);
        stream.Flush();
    }

    public void SendRequest(NetworkStream stream, string command, params Action<Stream>[] args)
    {
        stream.WriteByte((byte)RemotingMessageType.Request);
        CBORObject.Write(command, stream);
        foreach (var arg in args)
        {
            arg(stream);
        }

        SendEndMessage(stream);
        stream.Flush();
    }

    public static void SendEndMessage(NetworkStream stream)
    {
        stream.WriteByte(0x81);
        stream.WriteByte(0x17);
    }

    public string SendPrintRequest(NetworkStream stream, Cursor cursor)
    {
        SendRequest(stream, "print", s => { SendTree(_context, s, cursor.GetValue<Tree>()!, default); });

        int b;
        if ((b = stream.ReadByte()) != (int)RemotingMessageType.Response)
        {
            throw new ArgumentException("Unexpected message type: " + b);
        }

        if (stream.ReadByte() != 0)
        {
            throw new ArgumentException("Remote print failed: " + CBORObject.Read(stream));
        }

        var print = CBORObject.Read(stream).AsString();
        var end = CBORObject.Read(stream);
        return print;
    }

    public IList<SourceFile?> SendRunRecipeRequest(NetworkStream stream, string recipe,
        IDictionary<string, object?> options, IList<SourceFile> sourceFiles)
    {
        SendRequest(stream, "run-recipe", s =>
        {
            CBORObject.Write(recipe, s);
            CBORObject.Write(options, s);
            CBORObject.Write(sourceFiles.Count, s);
            foreach (var sourceFile in sourceFiles)
            {
                SendTree(_context, s, sourceFile, default);
            }
        });

        int b;
        while ((b = stream.ReadByte()) == (int)RemotingMessageType.Request)
        {
            ProcessNestedRequest(stream);
        }

        if (b != (int)RemotingMessageType.Response)
        {
            throw new ArgumentException("Unexpected message type: " + b);
        }

        if (stream.ReadByte() != 0)
        {
            throw new ArgumentException("Remote recipe run failed: " + CBORObject.Read(stream));
        }

        var inputStream = RemoteUtils.ReadToCommandEnd(stream);
        IList<SourceFile?> updated = new List<SourceFile?>();
        foreach (var sourceFile in sourceFiles)
        {
            updated.Add(ReceiveTree(_context, inputStream, sourceFile));
        }

        var end = CBORObject.Read(inputStream);
        return updated;
    }

    public void SendResetRequest(NetworkStream stream)
    {
        SendRequest(stream, "reset");

        var b = stream.ReadByte();
        if (b != (int)RemotingMessageType.Response)
        {
            throw new ArgumentException("Unexpected message type: " + b);
        }

        if (stream.ReadByte() != 0)
        {
            throw new ArgumentException("Remote recipe run failed: " + CBORObject.Read(stream));
        }

        var commandEnd = CBORObject.Read(stream);
    }
}
