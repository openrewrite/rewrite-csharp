using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Rewrite.Rpc;

public sealed class RpcObjectData
{
    public const int ADDED_LIST_ITEM = -1;
    

    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    public required ObjectState State { get; init; }
    
    public string? ValueType { get; init; }
    
    public object? Value { get; init; }
    
    public long? Ref { get; init; }
    public string? Trace { get; init; }

    public T? GetValue<T>()
    {
        if (Value is null)
            return default;
        if(Value is JObject jObj)
        {
            return jObj.ToObject<T>();
        }

        if (Value is T t)
        {
            return t;
        }
        throw new InvalidOperationException($"Can't deserialize value {Value.GetType()}: {Value} to type " + typeof(T).Name);
    }

    

    public enum ObjectState {
        
        NO_CHANGE,
        ADD,
        DELETE,
        CHANGE,
        END_OF_OBJECT
    }
}