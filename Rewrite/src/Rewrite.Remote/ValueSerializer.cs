using System.Formats.Cbor;
using System.Numerics;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

public interface ValueSerializer
{
    void Serialize(object value, string? typeName, CborWriter writer, SerializationContext context);

    public static void WriteObjectUsingReflection(object value, string? typeName, bool withId, CborWriter writer,
        SerializationContext context)
    {
        if (withId && context.RemotingContext.TryGetId(value, out var id))
        {
            writer.WriteInt64(id);
            return;
        }

        writer.WriteStartMap(null);
        writer.WriteTextString("@c");
        var type = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
        writer.WriteTextString(typeName ?? TypeUtils.ToJavaTypeName(type));
        if (withId)
        {
            writer.WriteTextString("@ref");
            id = context.RemotingContext.Add(value);
            writer.WriteInt64(id);
        }

        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            if (property.GetMethod == null || property.GetMethod.IsStatic)
                continue;
            var name = property.Name;
            writer.WriteTextString(char.ToLower(name[0]) + name[1..]);
            context.Serialize(property.GetValue(value), null, writer);
        }

        writer.WriteEndMap();
    }
}

public delegate void ValueSerializer<T>(T value, string? typeName, CborWriter writer,
    SerializationContext context);

internal class DelegateBasedSerializer<T>(ValueSerializer<T> @delegate) : ValueSerializer
{
    public void Serialize(object value, string? typeName, CborWriter writer, SerializationContext context)
    {
        @delegate((T)value, typeName, writer, context);
    }
}

public class SerializationContext(
    RemotingContext remotingContext,
    Dictionary<Type, ValueSerializer>? valueSerializers = default)
{
    private static readonly DefaultValueSerializer DefaultSerializer = new();
    public RemotingContext RemotingContext => remotingContext;

    public void Serialize(object? value, string? typeName, CborWriter writer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        if (valueSerializers != null)
        {
            foreach (var (type, serializer) in valueSerializers)
            {
                if (!type.IsInstanceOfType(value)) continue;
                serializer.Serialize(value, typeName, writer, this);
                return;
            }
        }

        DefaultSerializer.Serialize(value, typeName, writer, this);
    }
}

internal class DefaultValueSerializer : ValueSerializer
{
    public void Serialize(object value, string? typeName, CborWriter writer, SerializationContext context)
    {
        switch (value)
        {
            case Guid guid:
                writer.WriteByteString(guid.ToByteArray());
                break;
            case string s:
                writer.WriteTextString(s);
                break;
            case Markers m:
            {
                if (context.RemotingContext.TryGetId(m, out var id))
                {
                    writer.WriteInt64(id);
                }
                else
                {
                    id = context.RemotingContext.Add(m);

                    writer.WriteStartMap(3);
                    writer.WriteTextString("id");
                    context.Serialize(m.Id, null, writer);
                    writer.WriteTextString("@ref");
                    writer.WriteInt64(id);
                    writer.WriteTextString("markers");
                    writer.WriteStartArray(m.MarkerList.Count);
                    foreach (var marker in m.MarkerList)
                    {
                        context.Serialize(marker, null, writer);
                    }

                    writer.WriteEndArray();
                    writer.WriteEndMap();
                }

                break;
            }
            case int i:
                writer.WriteInt32(i);
                break;
            case uint ui:
                writer.WriteUInt32(ui);
                break;
            case long l:
                writer.WriteInt64(l);
                break;
            case ulong ul:
                writer.WriteUInt64(ul);
                break;
            case bool b:
                writer.WriteBoolean(b);
                break;
            case Enum:
                writer.WriteInt32((int)value);
                break;
            case double d:
                writer.WriteDouble(d);
                break;
            case char c:
                writer.WriteTextString(c.ToString());
                break;
            case decimal de:
                writer.WriteDecimal(de);
                break;
            case float f:
                writer.WriteSingle(f);
                break;
            case BigInteger bi:
                writer.WriteBigInteger(bi);
                break;
            case System.Collections.IList list:
            {
                writer.WriteStartArray(list.Count);
                foreach (var o in list)
                {
                    context.Serialize(o, null, writer);
                }

                writer.WriteEndArray();
                break;
            }
            case System.Collections.IDictionary dictionary:
            {
                writer.WriteStartMap(dictionary.Count);
                foreach (var key in dictionary.Keys)
                {
                    context.Serialize(key, null, writer);
                    context.Serialize(dictionary[key], null, writer);
                }

                writer.WriteEndMap();
                break;
            }
            case Marker:
            {
                if (context.RemotingContext.TryGetId(value, out var id))
                {
                    writer.WriteInt64(id);
                }
                else
                {
                    id = context.RemotingContext.Add(value);

                    writer.WriteStartMap(null);
                    writer.WriteTextString("@c");
                    var type = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
                    writer.WriteTextString(typeName ?? TypeUtils.ToJavaTypeName(type));
                    writer.WriteTextString("@ref");
                    writer.WriteInt64(id);

                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        var name = property.Name;
                        writer.WriteTextString(char.ToLower(name[0]) + name[1..]);
                        context.Serialize(property.GetValue(value), null, writer);
                    }

                    writer.WriteEndMap();
                }

                break;
            }
            default:
            {
                ValueSerializer.WriteObjectUsingReflection(value, typeName, false, writer, context);
                break;
            }
        }
    }
}