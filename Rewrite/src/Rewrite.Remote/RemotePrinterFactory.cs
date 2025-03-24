using Rewrite.Core;

namespace Rewrite.Remote;

public class RemotePrinterFactory(IRemotingClient client) : IPrinterFactory
{
    public TreeVisitor<Core.Tree, PrintOutputCapture<TP>> CreatePrinter<TP>()
    {
        return new RemotePrinter<TP>(client);
    }
}