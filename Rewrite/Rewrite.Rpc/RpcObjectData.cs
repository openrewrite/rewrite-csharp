using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Rewrite.Rpc;

public class RpcObjectData
{
    public const int ADDED_LIST_ITEM = -1;
    

    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    public required ObjectState State { get; init; }
    
    public string? ValueType { get; init; }
    
    public object? Value { get; init; }
    
    public int? Ref { get; init; }
    public string? Trace { get; init; }

    public T? GetValue<T>()
    {
        if (Value is null)
            return default;
        var jObj = (JObject)Value;
        return jObj.ToObject<T>();
    }
    
    public enum ObjectState {
        NO_CHANGE,
        ADD,
        DELETE,
        CHANGE,
        END_OF_OBJECT
    }
}