using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

public class SenderContext
{
    private static readonly SortedDictionary<Type, Func<Sender>> Registry = new(new TypeComparer());
    public TreeSender Sender { get; }
    private ITreeVisitor<Tree, SenderContext>? Visitor { get; }
    private object? Before { get; set; }

    public SenderContext(TreeSender sender)
    {
        Sender = sender;
        Visitor = null;
        Before = null;
    }

    private SenderContext(TreeSender sender, ITreeVisitor<Tree, SenderContext> visitor, object? before)
    {
        Sender = sender;
        Visitor = visitor;
        Before = before;
    }

    public Sender NewSender(Type type)
    {
        try
        {
            return Registry.First(entry => entry.Key.IsAssignableFrom(type)).Value.Invoke();
        }
        catch (Exception e)
        {
            throw new ArgumentException("Unsupported sender type: " + type, e);
        }
    }

    public SenderContext Fork<T>(ITreeVisitor<T, SenderContext> visitor, object? before) where T : class, Tree
    {
        return new SenderContext(Sender, visitor, before);
    }

    private void Visit<V>(Action<V, SenderContext> consumer, V after, object? before)
    {
        var saveBefore = Before;
        Before = before;
        consumer(after, this);
        Before = saveBefore;
    }

    public static void SendTree<T>(T after, T? before, JsonSender jsonSender) where T : Tree
    {
        new SenderContext(jsonSender).SendTree(after, before);
    }

    public void SendTree<T>(T after, T? before) where T : Tree
    {
        new OmniSender().Send(after, before, this);
    }

    public void SendNode<T, V>(T owner, Func<T, V?> extractor, Action<V, SenderContext> details)
    {
        SendNode(extractor(owner), Before != null ? extractor((T)Before) : default, details);
    }

    public void SendValue<T, V>(T owner, Func<T, V?> valueExtractor)
    {
        var afterValue = valueExtractor(owner);
        var beforeValue = Before != null ? valueExtractor((T)Before) : default;
        SendValue(afterValue, beforeValue);
    }

    public void SendTypedValue<T, V>(T owner, Func<T, V> valueExtractor)
    {
        var afterValue = valueExtractor(owner);
        var beforeValue = Before != null ? valueExtractor((T)Before) : default;
        SendTypedValue(afterValue, beforeValue);
    }

    private void SendNode<V>(V? after, V? before, Action<V, SenderContext> details)
    {
        DiffEvent evt;
        if (AreEqual(after, before))
        {
            evt = new DiffEvent(EventType.NoChange, null, null);
        }
        else if (before == null)
        {
            // FIXME this should not be `Markers` specific; should the type be passed in as a parameter?
            var concreteType = typeof(V) != typeof(Markers) ? TypeUtils.ToJavaTypeName(after!.GetType()) : null;
            evt = new DiffEvent(EventType.Add, concreteType, null);
        }
        else if (after == null)
        {
            evt = new DiffEvent(EventType.Delete, null, null);
        }
        else
        {
            evt = new DiffEvent(EventType.Update, null, null);
        }

        Sender.SendNode(evt, _ => Visit(details!, after, before));
    }

    private void SendValue<V>(V after, V? before)
    {
        DiffEvent evt;
        if (Before != null && AreEqual(after, before))
        {
            evt = new DiffEvent(EventType.NoChange, null, null);
        }
        else if (Before == null || before == null)
        {
            // FIXME we need a better mechanism to map types
            var concreteType = after is Marker ? TypeUtils.ToJavaTypeName(after) : null;
            evt = new DiffEvent(EventType.Add, concreteType, after);
        }
        else if (after == null)
        {
            evt = new DiffEvent(EventType.Delete, null, null);
        }
        else
        {
            evt = new DiffEvent(EventType.Update, null, after);
        }

        Sender.SendValue<V>(evt);
    }

    private void SendTypedValue<V>(V? after, V? before)
    {
        DiffEvent evt;
        if (Before != null && AreEqual(after, before))
        {
            evt = new DiffEvent(EventType.NoChange, null, null);
        }
        else if (Before == null || before == null)
        {
            var concreteType = after != null ? TypeUtils.ToJavaTypeName(after.GetType()) : null;
            evt = new DiffEvent(EventType.Add, concreteType, after);
        }
        else if (after == null)
        {
            evt = new DiffEvent(EventType.Delete, null, null);
        }
        else
        {
            evt = new DiffEvent(EventType.Update, null, after);
        }

        Sender.SendValue<V>(evt);
    }

    public void SendNodes<T, V, I>(T owner, Func<T, IList<V>?> elementExtractor, Action<V, SenderContext> details,
        Func<V, I> idFunction) where V : class where I : notnull
    {
        var afterList = elementExtractor(owner) ?? [];
        var beforeList = Before == null ? null : elementExtractor((T)Before);

        if (SendListEvent(afterList, beforeList))
        {
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.StartList, null, null));
            }

            RemoteUtils.CalculateListDiff(beforeList ?? [], afterList, idFunction,
                (op, _, _, beforeValue, afterValue) =>
                {
                    switch (op)
                    {
                        case RemoteUtils.Operation.Delete:
                        case RemoteUtils.Operation.NoChange:
                        case RemoteUtils.Operation.Add:
                        case RemoteUtils.Operation.Update:
                            SendNode(afterValue, beforeValue, details);
                            break;
                        case RemoteUtils.Operation.Move:
                            // FIXME
                            throw new NotImplementedException("Unexpected operation: " + op);
                    }
                });
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.EndList, null, null));
            }
        }
    }

    public void SendValues<T, V, I>(T owner, Func<T, IList<V>?> valueExtractor, Func<V, I> idFunction) where V : class where I : notnull
    {
        var afterList = valueExtractor(owner);
        var beforeList = Before == null ? null : valueExtractor((T)Before);

        if (SendListEvent(afterList, beforeList))
        {
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.StartList, null, null));
            }

            RemoteUtils.CalculateListDiff(beforeList ?? [], afterList ?? [], idFunction,
                (op, _, _, beforeValue, afterValue) =>
                {
                    switch (op)
                    {
                        case RemoteUtils.Operation.Delete:
                        case RemoteUtils.Operation.NoChange:
                        case RemoteUtils.Operation.Add:
                        case RemoteUtils.Operation.Update:
                            SendValue(afterValue, beforeValue);
                            break;
                        case RemoteUtils.Operation.Move:
                            // FIXME
                            throw new NotImplementedException("Unexpected operation: " + op);
                    }
                });
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.EndList, null, null));
            }
        }
    }

    public void SendTypedValues<T, V, I>(T owner, Func<T, IList<V>> valueExtractor, Func<V, I> idFunction)
        where V : class where I : notnull
    {
        var afterList = valueExtractor(owner);
        var beforeList = Before == null ? null : valueExtractor((T)Before);

        if (SendListEvent(afterList, beforeList))
        {
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.StartList, null, null));
            }

            RemoteUtils.CalculateListDiff(beforeList ?? [], afterList, idFunction,
                (op, _, _, beforeValue, afterValue) =>
                {
                    switch (op)
                    {
                        case RemoteUtils.Operation.Delete:
                        case RemoteUtils.Operation.NoChange:
                        case RemoteUtils.Operation.Add:
                        case RemoteUtils.Operation.Update:
                            SendTypedValue(afterValue, beforeValue);
                            break;
                        case RemoteUtils.Operation.Move:
                            // FIXME
                            throw new NotImplementedException("Unexpected operation: " + op);
                    }
                });
            if (beforeList != null)
            {
                Sender.SendValue<IList<V>>(new DiffEvent(EventType.EndList, null, null));
            }
        }
    }

    private bool SendListEvent<V>(IList<V>? after, IList<V>? before)
    {
        DiffEvent evt;
        if (after == before)
        {
            evt = new DiffEvent(EventType.NoChange, null, null);
        }
        else if (before == null)
        {
            evt = new DiffEvent(EventType.Add, null, after!.Count);
        }
        else if (after == null)
        {
            evt = new DiffEvent(EventType.Delete, null, null);
        }
        else
        {
            evt = new DiffEvent(EventType.Update, null, after.Count);
        }

        Sender.SendValue<int>(evt);
        return evt.EventType != EventType.NoChange && evt.EventType != EventType.Delete;
    }

    public void SendMarkers(Markers markers, SenderContext ignore)
    {
        SendValue(markers, ms => ms.Id);
        SendValues(markers, ms => ms.MarkerList, ms => ms.Id);
    }

    public void SendTree<T>(T after, SenderContext ctx) where T : Tree
    {
        after.Accept(Visitor!, ctx);
    }

    private static bool AreEqual<V>(V? after, V? before)
    {
        if (after == null || before == null)
        {
            return after is null && before is null;
        }

        return typeof(V).IsClass || typeof(V).IsAssignableTo(typeof(Tree)) || typeof(V).IsAssignableTo(typeof(Marker))
            ? ReferenceEquals(after, before)
            : EqualityComparer<V>.Default.Equals(after, before);
    }

    public static void Register(Type type, Func<Sender> senderFactory)
    {
        Registry.TryAdd(type, senderFactory);
    }
}
