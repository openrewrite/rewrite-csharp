namespace Rewrite.Remote;

public interface TreeReceiver
{
    DiffEvent ReceiveNode();
    DiffEvent ReceiveValue(Type expectedType);
}