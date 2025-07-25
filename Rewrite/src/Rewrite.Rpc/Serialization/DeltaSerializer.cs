using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;
using static Rewrite.Rpc.RpcObjectData.ObjectState;

namespace Rewrite.Rpc.Serialization;

public class DeltaSerializer
{
    public RefTracker RefTracking { get; }
    private readonly CSharpDeltaSerializer _serializeVisitor = new();
    private readonly CSharpDeltaDeserializer _deserializeVisitor = new();
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
            visitIfChanged = (o, context) => _serializeVisitor.Visit(tree, diffContext);
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

    // public object Deserialize(object before, List<RpcObjectData> changes)
    // {
    //     
    //     foreach (var change in changes)
    //     {
    //         switch (change.State)
    //         {
    //             case DELETE:
    //                 return null!;
    //             case NO_CHANGE:
    //                 return before;
    //             case ADD:
    //             case CHANGE:
    //                 return change.Value!;
    //             case END_OF_OBJECT:
    //                 continue;
    //             default:
    //                 throw new InvalidOperationException($"Unsupported state: {change.State}");
    //         }
    //     }
    //
    //     return before;
    // }
    //
    


    
    
    public T Deserialize<T>(T? before, List<RpcObjectData> changes)
    {
        return (T)Deserialize((object)before!, changes)!;
    }
    
    public object? Deserialize(object? before, List<RpcObjectData> changes)
    {
        using var traceContext = TraceContext.StartNew();
        var context = new DeserializationContext(before, this, changes.GetEnumerator());
        
        if (!context.MoveNext())
        {
            throw new InvalidOperationException("No events in change stream");
        }
        
        var firstEvent = context.Current;
        object? result = null;
        
        switch (firstEvent.State)
        {
            case RpcObjectData.ObjectState.DELETE:
                result = null;
                break;
                
            case RpcObjectData.ObjectState.NO_CHANGE:
                result = before;
                break;
                
            case RpcObjectData.ObjectState.ADD:
            case RpcObjectData.ObjectState.CHANGE:
                if (firstEvent.Value != null)
                {
                    // Simple value
                    result = firstEvent.Value;
                    RegisterRef(result, firstEvent.Ref);
                }
                else if (firstEvent.Ref.HasValue && RefTracking.TryGetObject(firstEvent.Ref.Value, out var existing))
                {
                    // Reference to existing object
                    result = existing;
                }
                else
                {
                    // Complex object that needs decomposition
                    if (before is Tree || (firstEvent.ValueType != null && IsTreeType(firstEvent.ValueType)))
                    {
                        result = _deserializeVisitor.Visit((Tree?)before, context);
                    }
                    else
                    {
                        result = DeserializeComplex(before, firstEvent, context);
                    }
                    RegisterRef(result, firstEvent.Ref);
                }
                break;
                
            case RpcObjectData.ObjectState.END_OF_OBJECT:
                return before;
                
            default:
                throw new InvalidOperationException($"Unexpected state: {firstEvent.State}");
        }
        
        // Consume END_OF_OBJECT if present
        if (context.HasNext() && context.Peek().State == RpcObjectData.ObjectState.END_OF_OBJECT)
        {
            context.MoveNext();
        }
        
        return result;
    }
    
    private object? DeserializeComplex(object? before, RpcObjectData currentEvent, DeserializationContext context)
    {
        // Handle non-tree complex objects like JRightPadded, Markers, etc.
        var type = GetTypeFromEvent(currentEvent, before);
        
        if (type == null)
        {
            throw new InvalidOperationException($"Cannot determine type for deserialization. Trace: {currentEvent.Trace}");
        }
        
        // Create instance if needed
        var instance = currentEvent.State == RpcObjectData.ObjectState.ADD 
            ? Activator.CreateInstance(type) 
            : before;
            
        // The visitor pattern will handle property deserialization
        return instance;
    }
    
    private Type? GetTypeFromEvent(RpcObjectData evt, object? before)
    {
        if (evt.ValueType != null)
        {
            return Type.GetType(evt.ValueType);
        }
        
        return before?.GetType();
    }
    
    private bool IsTreeType(string typeName)
    {
        var type = Type.GetType(typeName);
        return type != null && typeof(Tree).IsAssignableFrom(type);
    }
    
    private void RegisterRef(object? obj, long? refId)
    {
        if (obj != null && refId.HasValue)
        {
            RefTracking.AddRef(obj, refId.Value);
        }
    }
    
    internal string GetTrace(Type? parentType, string callingMethodName, string callingArgumentExpression)
    {
        return TraceContext.Current.ToString();
    }
}


