using System.Net.Sockets;
using System.Runtime.CompilerServices;
using PeterO.Cbor;
using Rewrite.Core;

namespace Rewrite.Remote;

public record RemotingContext : IRemotingContext
{
    private static readonly Dictionary<Type, ValueSerializer> ValueSerializers = new();
    private static readonly List<(Type, ValueDeserializer)> ValueDeserializers = [];

    private readonly Dictionary<object, long> _objectToIdMap = new(new ReferenceEqualityComparer());
    private readonly Dictionary<long, object> _idToObjectMap = new();


    public IRemotingClient? Client { get; private set; }

    public void Connect(Socket socket) => Client = new RemotingClient(this, socket);

    public bool TryGetId(object key, out long value) => _objectToIdMap.TryGetValue(key, out value);

    public long Add(object value)
    {
        // FIXME make sure the ID is not already used
        var id = _objectToIdMap.Count;
        _objectToIdMap[value] = id;
        _idToObjectMap[id] = value;
        return id;
    }

    public void Add(long key, object value)
    {
        _idToObjectMap[key] = value;
        _objectToIdMap[value] = key;
    }

    public object? GetObject(long key) => _idToObjectMap.GetValueOrDefault(key);

    public void Reset()
    {
        _idToObjectMap.Clear();
        _objectToIdMap.Clear();
    }

    public SenderContext NewSenderContext(Stream outputStream) =>
        new(new JsonSender(outputStream, new SerializationContext(this, ValueSerializers)));

    public ReceiverContext NewReceiverContext(MemoryStream inputStream) =>
        new(new JsonReceiver(inputStream, new DeserializationContext(this, ValueDeserializers)));

    public RemotingContext Copy() => new();

    public Recipe NewRecipe(string recipeId, CBORObject recipeOptions) =>
        IRemotingContext.RecipeFactories[recipeId](recipeOptions);

    public static void RegisterValueSerializer<T>(ValueSerializer<T> serializer) =>
        ValueSerializers[typeof(T)] = new DelegateBasedSerializer<T>(serializer);

    public static void RegisterValueDeserializer<T>(ValueDeserializer<T> deserializer)
    {
        for (var i = 0; i < ValueDeserializers.Count; i++)
        {
            var type = ValueDeserializers[i].Item1;
            if (type != typeof(T)) continue;
            ValueDeserializers[i] = (type, new DelegateBasedDeserializer<T>(deserializer));
            return;
        }

        ValueDeserializers.Add((typeof(T), new DelegateBasedDeserializer<T>(deserializer)));
    }
}

internal class ReferenceEqualityComparer : IEqualityComparer<object>
{
    public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);

    public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
}
