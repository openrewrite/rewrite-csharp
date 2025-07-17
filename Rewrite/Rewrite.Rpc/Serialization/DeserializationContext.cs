using System.Collections;
using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Rpc.Serialization;

public class DeserializationContext : ContextBase
{
    private readonly IEnumerator<RpcObjectData> _events;
    private RpcObjectData? _current;
    private RpcObjectData? _peeked;
    private bool _hasPeeked;

    public DeserializationContext(object? before, DeltaSerializer deserializer, IEnumerator<RpcObjectData> events)
        : base(before, deserializer)
    {
        _events = events;
    }

    public RpcObjectData Current => _current ?? throw new InvalidOperationException("No current event");

    public bool MoveNext()
    {
        if (_hasPeeked)
        {
            _current = _peeked;
            _hasPeeked = false;
            return true;
        }

        if (_events.MoveNext())
        {
            _current = _events.Current;
            LogTrace($"[Event] {_current.State} Path: {_current.Trace} Val: {_current.Value} Ref: {_current.Ref} ");
            return true;
        }

        return false;
    }

    public bool HasNext()
    {
        if (_hasPeeked)
            return true;

        if (_events.MoveNext())
        {
            _peeked = _events.Current;
            _hasPeeked = true;
            return true;
        }

        return false;
    }

    public RpcObjectData Peek()
    {
        if (!_hasPeeked && !HasNext())
            throw new InvalidOperationException("No more events to peek");

        return _peeked!;
    }

    public DeserializationContext<T> As<T>(T? before)
    {
        return new DeserializationContext<T>(before, Serializer, this._events);
    }

    private void LogTrace(string message)
    {
        // For debugging - could be enhanced with proper logging
        System.Diagnostics.Debug.WriteLine($"[DESERIALIZE] {message}");
        // Console.WriteLine($"[DESERIALIZE] {message}");
    }
}

public class DeserializationContext<T> : DeserializationContext
{
    
    internal DeserializationContext(T? before, DeltaSerializer deserializer, IEnumerator<RpcObjectData> events)
        : base(before, deserializer, events)
    {
    }
    
    public new T? Before => (T?)base.Before;
    
    public TProp? DeserializeProperty<TProp>(
        Func<T, TProp?> selector,
        Func<TProp?, DeserializationContext, object?>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "")
    {
        using var propertyTrace = TraceContext.Current.CreateFrame(callingArgumentExpression);
        
        if (!MoveNext())
        {
            throw new InvalidOperationException($"Expected event for property {callingArgumentExpression}");
        }
        
        var evt = Current;
        var beforeValue = Before != null ? selector(Before) : default;
        
        switch (evt.State)
        {
            case RpcObjectData.ObjectState.NO_CHANGE:
                return beforeValue;
                
            case RpcObjectData.ObjectState.DELETE:
                return default;
                
            case RpcObjectData.ObjectState.ADD:
            case RpcObjectData.ObjectState.CHANGE:
                if (evt.Value != null)
                {
                    // Simple value
                    RegisterRef(evt.Value, evt.Ref);
                    return (TProp)evt.Value;
                }
                else if (evt.Ref.HasValue && Serializer.RefTracking.TryGetObject(evt.Ref.Value, out var existing))
                {
                    // Reference to existing object
                    return (TProp)existing!;
                }
                else if (visitIfChanged != null)
                {
                    // Complex object
                    
                    var result = visitIfChanged(beforeValue, this);
                    RegisterRef(result, evt.Ref);
                    return (TProp?)result;
                }
                else
                {
                    throw new InvalidOperationException($"Cannot deserialize complex object without visitor at {evt.Trace}");
                }
                
            default:
                throw new InvalidOperationException($"Unexpected state {evt.State} at {evt.Trace}");
        }
    }
    
    public IList<TProp> DeserializeList<TProp>(
        Func<T, IList<TProp>> selector,
        Func<TProp?, DeserializationContext, object?>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "") where TProp : IHasId<Guid>
    {
        return DeserializeList(selector, x => x.Id, visitIfChanged, callingMethodName, callingArgumentExpression);
    }
    
    public IList<JRightPadded<TProp>> DeserializeList<TProp>(
        Func<T, IList<JRightPadded<TProp>>> selector,
        Func<JRightPadded<TProp>?, DeserializationContext, JRightPadded<TProp>?>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "") where TProp : IHasId<Guid>
    {
        return DeserializeList(selector, x => x.Element.Id, visitIfChanged, callingMethodName, callingArgumentExpression);
    }
    
    public IList<TProp> DeserializeList<TProp>(
        Func<T, IList<TProp>> selector,
        Func<TProp, object> keySelector,
        Func<TProp?, DeserializationContext, object?>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "")
    {
        using var propertyTrace = TraceContext.Current.CreateFrame(callingArgumentExpression);
        
        if (!MoveNext())
        {
            throw new InvalidOperationException($"Expected event for list {callingArgumentExpression}");
        }
        
        var listEvent = Current;
        
        switch (listEvent.State)
        {
            case RpcObjectData.ObjectState.NO_CHANGE:
                return Before != null ? selector(Before) : new List<TProp>();
                
            case RpcObjectData.ObjectState.DELETE:
                return new List<TProp>();
                
            case RpcObjectData.ObjectState.ADD:
            case RpcObjectData.ObjectState.CHANGE:
                // Read positions array
                // if (!MoveNext())
                // {
                //     throw new InvalidOperationException($"Expected positions event for list {callingArgumentExpression}");
                // }
                
                var positionsEvent = Current;
                if (positionsEvent.State != RpcObjectData.ObjectState.CHANGE || positionsEvent.Value == null)
                {
                    throw new InvalidOperationException($"Expected CHANGE event with positions array at {positionsEvent.Trace}");
                }
                
                var positions = positionsEvent.GetValue<List<int>>();
                if (positions == null)
                {
                    throw new InvalidOperationException($"Failed to deserialize positions array at {positionsEvent.Trace}");
                }
                
                // Get before list as array for indexed access
                var beforeList = Before != null ? selector(Before)?.ToList() : null;
                var resultList = new List<TProp>(positions.Count);
                
                // Process each item
                for (int i = 0; i < positions.Count; i++)
                {
                    using var indexTrace = TraceContext.Current.CreateFrame($"[{i}]");
                    
                    if (!MoveNext())
                    {
                        throw new InvalidOperationException($"Expected event for list item {i}");
                    }
                    
                    var itemEvent = Current;
                    var beforeItem = positions[i] >= 0 && beforeList != null ? beforeList[positions[i]] : default;
                    
                    TProp? item;
                    switch (itemEvent.State)
                    {
                        case RpcObjectData.ObjectState.NO_CHANGE:
                            item = beforeItem;
                            break;
                            
                        case RpcObjectData.ObjectState.ADD:
                        case RpcObjectData.ObjectState.CHANGE:
                            if (itemEvent.Value != null)
                            {
                                item = (TProp)itemEvent.Value;
                                RegisterRef(item, itemEvent.Ref);
                            }
                            else if (itemEvent.Ref.HasValue && Serializer.RefTracking.TryGetObject(itemEvent.Ref.Value, out var existing))
                            {
                                item = (TProp)existing!;
                            }
                            else if (visitIfChanged != null)
                            {
                                item = (TProp)visitIfChanged(beforeItem, this)!;
                                RegisterRef(item, itemEvent.Ref);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Cannot deserialize complex list item without visitor at {itemEvent.Trace}");
                            }
                            break;
                            
                        default:
                            throw new InvalidOperationException($"Unexpected state {itemEvent.State} for list item at {itemEvent.Trace}");
                    }
                    
                    if (item != null)
                    {
                        resultList.Add(item);
                    }
                }
                
                RegisterRef(resultList, listEvent.Ref);
                return resultList;
                
            default:
                throw new InvalidOperationException($"Unexpected state {listEvent.State} for list at {listEvent.Trace}");
        }
    }
    
    private bool RegisterRef(object? obj, long? refId)
    {
        if (obj != null && refId.HasValue)
        {
            return Serializer.RefTracking.AddRef(obj, refId.Value);
        }

        return false;
    }
}