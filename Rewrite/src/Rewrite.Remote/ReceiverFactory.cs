using Rewrite.Core;

namespace Rewrite.Remote;

public interface ReceiverFactory
{
    Tree Create<T>(string type, ReceiverContext ctx) where T : Tree;
}