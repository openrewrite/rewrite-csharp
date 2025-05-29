using Rewrite.Core;

namespace Rewrite.Remote;

public class RemotingExecutionContextView(IExecutionContext @delegate) : DelegatingExecutionContext(@delegate)
{
    private readonly IExecutionContext _delegate = @delegate;

    public static RemotingExecutionContextView View(IExecutionContext ctx)
    {
        if (ctx is RemotingExecutionContextView)
        {
            return (RemotingExecutionContextView)ctx;
        }

        return new RemotingExecutionContextView(ctx);
    }

    public IRemotingContext? RemotingContext
    {
        get => _delegate.GetMessage<IRemotingContext>("remoting") ?? IRemotingContext.Current();
        set
        {
            value?.SetCurrent();
            _delegate.PutMessage("remoting", value);
        }
    }
}
