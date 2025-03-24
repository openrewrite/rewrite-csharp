using Rewrite.Core;

namespace Rewrite.Remote;

public interface Receiver
{
    ReceiverContext Fork(ReceiverContext ctx);
    public object Receive<T>(T? before, ReceiverContext ctx) where T : Tree;
}