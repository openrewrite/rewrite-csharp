using Rewrite.Core;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Remote;

public class RemotingExecutionContextView(ExecutionContext @delegate) : DelegatingExecutionContext(@delegate)
{
    private readonly ExecutionContext _delegate = @delegate;

    public static RemotingExecutionContextView View(ExecutionContext ctx)
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
