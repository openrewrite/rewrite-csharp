using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Rewrite.Rpc.Serialization;

public class DeserializationContext(object? before, Stack<RpcObjectData> events, Stack<object> deserializedValues, DeltaSerializer serializer) : ContextBase(before, serializer)
{
    public Stack<RpcObjectData> Events { get; } = events;
    /// <summary>
    /// Since many objects we're deserializing are instantiated with every property passed into through constructor,
    /// we need to rehydrate all their constitutes first as we need to reassemble the tree in "depth-first" order
    /// </summary>
    public Stack<object> DeserializedValues { get; } = deserializedValues;

    public DeserializationContext<T> As<T>(T? before)
    {
        if(!ReferenceEquals(before, Before)) throw new InvalidOperationException($"Argument must be same as property {nameof(Before)}");
        return new DeserializationContext<T>((T?)Before, Events, DeserializedValues, Serializer);
    }
}

public class DeserializationContext<T>(object? before,  Stack<RpcObjectData> events, Stack<object> deserializedValues, DeltaSerializer serializer) : DeserializationContext(before, events, deserializedValues, serializer)
{
    public TProp DeserializeProperty<TProp>(Func<T, TProp> propertySelector)
    {
        
    }
    public static TProp CreateInstance<TProp>()
    {
        return (TProp)CreateInstance(typeof(TProp));
    }
    public static object CreateInstance(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type)!;

        if (type.IsAbstract || type.IsInterface)
            throw new InvalidOperationException($"Cannot create instance of abstract type or interface: {type.FullName}");

        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .OrderBy(c => c.GetParameters().Length);

        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var args = parameters.Select(p => GetDefault(p.ParameterType)).ToArray();

            try
            {
                return ctor.Invoke(args);
            }
            catch
            {
                // Try next constructor
            }
        }

        throw new InvalidOperationException($"No suitable constructor found for type: {type.FullName}");
    }
    
    private static object? GetDefault(Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);

        return null;
    }
}