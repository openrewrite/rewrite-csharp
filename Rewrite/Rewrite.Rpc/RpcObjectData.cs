using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Rewrite.Rpc;

public class RpcObjectData
{
    public const int ADDED_LIST_ITEM = -1;
    public RpcObjectData(ObjectState state, string? valueType, object? value, int? @ref, string? trace)
    {
        State = state;
        ValueType = valueType;
        Value = value;
        Ref = @ref;
        Trace = trace;
    }

    [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    public ObjectState State { get; set; }
    
    public string? ValueType { get; set; }
    
    public object? Value { get; set; }
    
    public int? Ref { get; set; }
    public string? Trace { get; set; }

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