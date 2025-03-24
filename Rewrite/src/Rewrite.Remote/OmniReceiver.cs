using Rewrite.Core;

namespace Rewrite.Remote;

internal class OmniReceiver : Receiver
{
    public ReceiverContext Fork(ReceiverContext ctx)
    {
        throw new NotImplementedException("Cannot fork OmniReceiver");
    }

    public object Receive<T>(T? before, ReceiverContext ctx) where T : Tree
    {
        var visitor = new Visitor();
        return visitor.Visit(before, ctx);
    }

    private class Visitor : TreeVisitor<Tree, ReceiverContext>
    {
        public override Tree Visit(Tree? tree, ReceiverContext ctx)
        {
            Cursor = new Cursor(Cursor, tree!);

            tree = ctx.PolymorphicReceiveTree(tree);

            Cursor = Cursor.Parent!;
            return tree!;
        }
    }
}
