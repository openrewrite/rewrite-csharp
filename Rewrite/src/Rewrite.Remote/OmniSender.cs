using Rewrite.Core;

namespace Rewrite.Remote;

internal class OmniSender : Sender
{
    public void Send<T>(T after, T? before, SenderContext ctx) where T : Tree
    {
        var sender = ctx.NewSender(after.GetType());
        sender.Send(after, before, ctx);
    }
}