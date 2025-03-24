using Rewrite.Core;

namespace Rewrite.Remote;

public interface Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Tree;
}