using Rewrite.Core;

namespace Rewrite.Remote;

public class RemotePrinter<P>(IRemotingClient client) : TreeVisitor<Tree, PrintOutputCapture<P>>
{
    public override Tree? Visit(Tree? tree, PrintOutputCapture<P> p)
    {
        Cursor = new Cursor(Cursor, tree ?? throw new InvalidOperationException($"Parameter {nameof(tree)} should not be null"));
        p.Append(client.Print(Cursor));
        Cursor = Cursor.Parent!;
        return tree;
    }
}
