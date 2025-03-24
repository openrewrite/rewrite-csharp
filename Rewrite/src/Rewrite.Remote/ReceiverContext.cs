using System.Collections;
using System.Collections.Specialized;
using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

public class ReceiverContext
{
    private static readonly SortedDictionary<Type, Func<Receiver>> Registry = new(new TypeComparer());

    public TreeReceiver Receiver { get; }
    public ITreeVisitor<Tree, ReceiverContext>? Visitor { get; }
    private ReceiverFactory? Factory { get; }

    public ReceiverContext(TreeReceiver receiver)
    {
        Receiver = receiver;
    }

    private ReceiverContext(TreeReceiver receiver, ITreeVisitor<Tree, ReceiverContext> visitor,
        ReceiverFactory factory)
    {
        Receiver = receiver;
        Visitor = visitor;
        Factory = factory;
    }

    public ReceiverContext Fork<T>(ITreeVisitor<T, ReceiverContext> visitor, ReceiverFactory factory)
        where T : class, Tree
    {
        return new ReceiverContext(Receiver, visitor, factory);
    }

    public static T ReceiveTree<T>(T? before, JsonReceiver jsonReceiver) where T : Tree
    {
        var ctx = new ReceiverContext(jsonReceiver);
        return ctx.ReceiveTree(before);
    }

    public T ReceiveTree<T>(T? before) where T : Tree
    {
        return (T)new OmniReceiver().Receive(before, this);
    }

    public T ReceiveTree<T>(T? before, string? type, ReceiverContext ctx) where T : Tree
    {
        return before != null ? (T?)before.Accept(Visitor!, ctx)! : (T)Factory!.Create<T>(type!, ctx);
    }

    public Tree? PolymorphicReceiveTree(Tree? before)
    {
        var diffEvent = Receiver.ReceiveNode();
        switch (diffEvent.EventType)
        {
            case EventType.Add:
            case EventType.Update:
                var treeReceiver = NewReceiver(diffEvent.ConcreteType ?? before!.GetType().FullName!);
                var forked = treeReceiver.Fork(this);
                return forked.ReceiveTree(diffEvent.EventType == EventType.Add ? null : before, diffEvent.ConcreteType, forked);

            case EventType.Delete:
                return null;

            default:
                return before;
        }
    }

    private Receiver NewReceiver(string typeName)
    {
        var type = TypeUtils.GetType(typeName);
        try
        {
            return Registry.First(entry => entry.Key.IsAssignableFrom(type)).Value.Invoke();
        }
        catch (Exception e)
        {
            throw new ArgumentException("Unsupported receiver type: " + typeName, e);
        }
    }

    public T? ReceiveNode<T>(T? before, DetailsReceiver<T> details)
    {
        var evt = Receiver.ReceiveNode();
        switch (evt.EventType)
        {
            case EventType.Delete:
                return default;
            case EventType.Add:
                return details(default, evt.ConcreteType, this);
            case EventType.Update:
                return details(before, evt.ConcreteType, this);
        }

        return before;
    }

    public Markers ReceiveMarkers(Markers? before, string? type, ReceiverContext ctx)
    {
        var id = ReceiveValue(before?.Id ?? default);

        var afterMarkers = RemoteUtils.ReceiveValues(before?.MarkerList, ctx);
        return before != null ? before.WithId(id).WithMarkers(afterMarkers!) : new Markers(id, afterMarkers!);
    }

    public IList<T>? ReceiveNodes<T>(IList<T>? before, DetailsReceiver<T> details)
    {
        return RemoteUtils.ReceiveNodes(before, details, this);
    }

    public IList<T> ReceiveValues<T>(IList<T>? before)
    {
        return RemoteUtils.ReceiveValues(before, this)!;
    }

    public T? ReceiveValue<T>(T? before)
    {
        return ReceiveValue0(before);
    }

    private T? ReceiveValue0<T>(T? before)
    {
        var evt = Receiver.ReceiveValue(typeof(T));

        switch (evt.EventType)
        {
            case EventType.Update:
            case EventType.Add:
                return (T?)evt.Msg;
            case EventType.Delete:
                return default;
            case EventType.NoChange:
            default:
                return before;
        }
    }

    public static void Register(Type type, Func<Receiver> receiverFactory)
    {
        Registry.TryAdd(type, receiverFactory);
    }

    public delegate T DetailsReceiver<T>(T? before, string? type, ReceiverContext ctx);
}
