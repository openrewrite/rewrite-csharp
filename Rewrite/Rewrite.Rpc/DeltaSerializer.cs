using Rewrite.Core;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using static Rewrite.Rpc.RpcObjectData.ObjectState;

namespace Rewrite.Rpc;

public class DeltaSerializer
{
    public List<RpcObjectData> Serialize(object? before, object? after)
    {
        return Serialize(before, after, after);
    }
    private List<RpcObjectData> Serialize(object? before, object? after, object? root)
    {
        var list = new List<RpcObjectData>();
        bool shouldWriteEndOfObject = ReferenceEquals(after, root);

        if (after == null)
        {
            list.Add(new() { State = DELETE });
            if(shouldWriteEndOfObject)
                list.Add(new() { State = END_OF_OBJECT });
            return list;
        }

        if (before == null)
        {
            // ADD case — send type and value
            list.Add(new()
            {
                State = ADD,
                ValueType = after.GetType().AssemblyQualifiedName,
                Value = after,
            });
            if(shouldWriteEndOfObject)
                list.Add(new() { State = END_OF_OBJECT });
            return list;
        }

        if (!Equals(before, after))
        {
            list.Add(new()
            {
                State = CHANGE,
                ValueType = after.GetType().AssemblyQualifiedName,
                Value = after,
            });
            if(shouldWriteEndOfObject)
                list.Add(new() { State = END_OF_OBJECT });
            return list;
        }

        // If no change:
        list.Add(new() { State = NO_CHANGE });
        if(shouldWriteEndOfObject)
            list.Add(new() { State = END_OF_OBJECT });
        return list;
    }

    public T Deserialize<T>(object before, List<RpcObjectData> changes)
    {
        return (T)Deserialize(before, changes);
    }

    public object Deserialize(object before, List<RpcObjectData> changes)
    {
        foreach (var change in changes)
        {
            switch (change.State)
            {
                case DELETE:
                    return null!;
                case NO_CHANGE:
                    return before;
                case ADD:
                case CHANGE:
                    return change.Value!;
                case END_OF_OBJECT:
                    continue;
                default:
                    throw new InvalidOperationException($"Unsupported state: {change.State}");
            }
        }

        return before;
    }
  
    public abstract class DiffContext
    {
        public DeltaSerializer Serializer { get; }
        public List<RpcObjectData> Output { get; }

        protected DiffContext(DeltaSerializer serializer, List<RpcObjectData> output)
        {
            Serializer = serializer;
            Output = output;
        }

        public DiffContext<T> For<T>(T? after, T? before = default)
        {
            return new DiffContext<T>(before, after, Serializer, Output);
        }
    }
    public class DiffContext<T> : DiffContext
    {
        public T? Before { get; }
        public T? After { get; }

        public DiffContext(T? before, T? after, DeltaSerializer serializer, List<RpcObjectData> output)
            : base(serializer, output)
        {
            Before = before;
            After = after;
        }

        public void SerializeProperty<TProp>(
            Func<T, TProp?> selector,
            Action<TProp, DiffContext> visitIfChanged)
        {
            var beforeValue = Before != null ? selector(Before) : default;
            var afterValue = After != null ? selector(After) : default;

            if (Equals(beforeValue, afterValue))
            {
                Output.Add(new() { State = NO_CHANGE });
            }
            else
            {
                Output.Add(new() { State = CHANGE });
                var childContext = new DiffContext<TProp>(beforeValue, afterValue, Serializer, Output);
                visitIfChanged(afterValue!, childContext);
            }
        }

        public void SerializeLeaf<TProp>(Func<T, TProp?> selector)
        {
            var value = selector(After!);
            var beforeValue = Before != null ? selector(Before) : default;

            if (Equals(value, beforeValue))
            {
                Output.Add(new() { State = NO_CHANGE });
            }
            else
            {
                Output.Add(new()
                {
                    State = CHANGE,
                    Value = value,
                    ValueType = value?.GetType().AssemblyQualifiedName
                });
            }
        }
    }

}

public class CSharpDeltaSerializer : CSharpVisitor<DeltaSerializer.DiffContext>
{
    public override J? VisitClassDeclaration(Cs.ClassDeclaration node, DeltaSerializer.DiffContext p)
    {
        var ctx = p.For(node);
        ctx.SerializeProperty(x => x.Name, (after, context) => VisitIdentifier(after, context));
        return node;
    }
} 