using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;
using static Rewrite.Rpc.RpcObjectData.ObjectState;

namespace Rewrite.Rpc.Serialization;

public class DeltaSerializer
{
    public RefTracker RefTracking { get; }
    private readonly CSharpDeltaSerializer _visitor = new();
    internal const int ADDED_LIST_ITEM = -1;

    public DeltaSerializer() : this(new RefTracker())
    {
    }

    public DeltaSerializer(RefTracker refTracking)
    {
        RefTracking = refTracking;
    }

    public List<RpcObjectData> Serialize(object? before, object? after)
    {
        using var traceContext = TraceContext.StartNew();
        if (before is null && after is null) throw new ArgumentException("Both before and after cannot be null");
        var list = new List<RpcObjectData>();
        var diffContext = new SerializationContext(before, after, this, list);
        Action<object, SerializationContext>? visitIfChanged = null;
        if(after is Tree tree)
            visitIfChanged = (o, context) => _visitor.Visit(tree, diffContext);
        diffContext.AppendDatum(before, after, visitIfChanged);
        
        diffContext.EndOfObject();
        return list;
    }

    
    //
    // public T Deserialize<T>(object before, List<RpcObjectData> changes)
    // {
    //     if(before is Tree tree)
    //         visitIfChanged = (o, context) => _visitor.Visit(tree, diffContext);
    //     
    //     return (T)Deserialize(before, changes);
    // }

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
  
    


}


