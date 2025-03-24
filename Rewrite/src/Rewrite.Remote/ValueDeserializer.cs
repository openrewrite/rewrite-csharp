using System.Formats.Cbor;
using System.Text;
using Rewrite.Core.Marker;

namespace Rewrite.Remote;

public interface ValueDeserializer
{
    // TODO expectedType vs. actualType
    object? Deserialize(Type type, CborReader reader, DeserializationContext context);

    public static Dictionary<string, object?> ReadCborMap(CborReader reader)
    {
        reader.ReadStartMap();
        var dict = new Dictionary<string, object?>();

        while (reader.PeekState() != CborReaderState.EndMap)
        {
            var key = reader.ReadTextString();
            var value = ReadCborValue(reader);

            dict.TryAdd(key, value);
        }

        reader.ReadEndMap();

        return dict;
    }

    public static object? ReadCborValue(CborReader reader)
    {
        // Check the major type of the next CBOR data item
        switch (reader.PeekState())
        {
            case CborReaderState.Null:
                reader.ReadNull();
                return null;
            case CborReaderState.TextString:
                return reader.ReadTextString();
            case CborReaderState.UnsignedInteger:
            case CborReaderState.NegativeInteger:
                return reader.ReadInt64();
            case CborReaderState.Boolean:
                return reader.ReadBoolean();
            case CborReaderState.SimpleValue:
                return reader.ReadSimpleValue();
            case CborReaderState.ByteString:
                return reader.ReadByteString();
            case CborReaderState.Tag:
                return reader.ReadDecimal();
            case CborReaderState.StartArray:
                return ReadCborArray(reader);
            case CborReaderState.StartMap:
                return ReadCborMap(reader);
            // TODO: handle other types as necessary
            default:
                throw new NotSupportedException("Unsupported CBOR data type: " + reader.PeekState());
        }
    }

    public static List<object?> ReadCborArray(CborReader reader)
    {
        var count = reader.ReadStartArray();
        var list = new List<object?>(count ?? 4);
        while (reader.PeekState() != CborReaderState.EndArray)
        {
            var value = ReadCborValue(reader);
            list.Add(value);
        }

        reader.ReadEndArray();
        return list;
    }
}

public delegate T ValueDeserializer<out T>(Type type, CborReader reader, DeserializationContext context);

internal class DelegateBasedDeserializer<T>(ValueDeserializer<T> @delegate) : ValueDeserializer
{
    public object? Deserialize(Type type, CborReader reader, DeserializationContext context) =>
        @delegate(type, reader, context);
}

public class DeserializationContext(
    RemotingContext remotingContext,
    List<(Type, ValueDeserializer)> valueDeserializers)
{
    private static readonly DefaultValueDeserializer DefaultDeserializer = new();
    public RemotingContext RemotingContext => remotingContext;

    public T? Deserialize<T>(CborReader reader)
    {
        return (T?)Deserialize(typeof(T), reader);
    }
    public object? Deserialize(Type expectedType, CborReader reader)
    {
        if (reader.PeekState() == CborReaderState.Null)
        {
            reader.ReadNull();
            return null;
        }

        if (expectedType == typeof(int))
            return reader.ReadInt32();

        if (expectedType == typeof(Guid))
            return new Guid(reader.ReadByteString());

        if (expectedType == typeof(string))
            return reader.ReadTextString();

        if (expectedType == typeof(bool))
            return reader.ReadBoolean();

        if (expectedType == typeof(long))
            return reader.ReadInt64();

        if (expectedType == typeof(double) || expectedType == typeof(float))
            return reader.ReadDouble();


        var isNullable = expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(Nullable<>);
        var underlyingType = isNullable ? Nullable.GetUnderlyingType(expectedType)! : expectedType;
        if (underlyingType.IsEnum && reader.PeekState() == CborReaderState.UnsignedInteger)
            return Enum.GetValues(underlyingType).GetValue(reader.ReadInt32());

        switch (reader.PeekState())
        {
            case CborReaderState.Boolean:
                return reader.ReadBoolean();
            case CborReaderState.UnsignedInteger:
            case CborReaderState.NegativeInteger:
                var i = reader.ReadInt32();
                var objectById = RemotingContext.GetObject(i);
                return objectById ?? i;

            case CborReaderState.HalfPrecisionFloat:
            case CborReaderState.SinglePrecisionFloat:
            case CborReaderState.DoublePrecisionFloat:
                return reader.ReadDouble();
            case CborReaderState.TextString:
                var str = reader.ReadTextString();
                if (reader.PeekState() == CborReaderState.EndArray || reader.PeekState() == CborReaderState.EndMap)
                    return str;

                var concreteType = reader.PeekState() != CborReaderState.EndArray && reader.PeekState() != CborReaderState.EndMap
                    ? str
                    : expectedType?.FullName;

                if (concreteType == "org.openrewrite.FileAttributes")
                {
                    ValueDeserializer.ReadCborMap(reader);
                    return new Rewrite.Core.FileAttributes();
                }

                if (concreteType == "java.lang.String")
                    return reader.ReadTextString();

                if (concreteType == "java.lang.Boolean")
                    return reader.ReadBoolean();

                if (concreteType == "java.lang.Integer")
                    return reader.ReadInt32();

                if (concreteType == "java.lang.Character")
                    return reader.ReadTextString()[0];

                if (concreteType == "java.lang.Long")
                    return reader.ReadInt64();

                if (concreteType == "java.lang.Double")
                    return reader.ReadDouble();

                if (concreteType == "java.lang.Float")
                    return reader.ReadSingle();

                if (concreteType == "java.math.BigInteger")
                    return reader.ReadBigInteger();

                if (concreteType == "java.math.BigDecimal")
                    return reader.ReadDecimal();

                throw new NotImplementedException("No deserialization implemented for: " + concreteType);

            case CborReaderState.StartArray:
                reader.ReadStartArray();
                concreteType = reader.ReadTextString();
                var actualType = TypeUtils.GetType(concreteType);
                foreach (var (type, deserializer) in valueDeserializers)
                {
                    if (!type.IsAssignableFrom(actualType)) continue;
                    return deserializer.Deserialize(actualType, reader, this);
                }

                break;

            case CborReaderState.StartMap:
                if (typeof(Marker).IsAssignableFrom(expectedType))
                {
                    if (reader.PeekState() == CborReaderState.UnsignedInteger)
                        return RemotingContext.GetObject(reader.ReadInt64());

                    var markerMap = ValueDeserializer.ReadCborMap(reader);
                    Marker marker;
                    switch (markerMap["@c"])
                    {
                        case "org.openrewrite.marker.SearchResult":
                        case "Rewrite.Core.Marker.SearchResult":
                            var desc = markerMap.TryGetValue("description", out var value)
                                ? value as string
                                : null;
                            marker = new SearchResult(new Guid((markerMap["id"] as byte[])!), desc);
                            break;
                        default:
                            marker = new UnknownJavaMarker(new Guid((markerMap["id"] as byte[])!), markerMap!);
                            break;
                    }

                    if (markerMap.TryGetValue("@ref", out var id))
                        RemotingContext.Add((long)id!, marker);

                    return marker;
                    // concreteType = msg?.GetType().FullName;
                }
                reader.ReadStartMap();
                if (reader.ReadTextString() != "@c")
                    throw new NotImplementedException("Expected @c key");
                concreteType = reader.ReadTextString();
                actualType = TypeUtils.GetType(concreteType);
                foreach (var (type, deserializer) in valueDeserializers)
                {
                    if (!type.IsAssignableFrom(actualType)) continue;
                    return deserializer.Deserialize(actualType, reader, this);
                }

                break;
            default:
                throw new NotImplementedException("No deserialization implemented for: " +
                                                  expectedType);
        }

        // DefaultDeserializer.Deserialize(expectedType, reader, this);

        throw new NotImplementedException("No deserializer found for: " + ValueDeserializer.ReadCborMap(reader)
            .Aggregate(new StringBuilder(), (sb, kv) => sb.Append($"{kv.Key}: {kv.Value}, "), sb => sb.ToString()));
    }
}

internal class DefaultValueDeserializer : ValueDeserializer
{
    public object? Deserialize(Type? expectedType, CborReader reader, DeserializationContext context)
    {

        throw new NotImplementedException("No deserializer found for: " + ValueDeserializer.ReadCborMap(reader)
            .Aggregate(new StringBuilder(), (sb, kv) => sb.Append($"{kv.Key}: {kv.Value}, "), sb => sb.ToString()));
    }
}
