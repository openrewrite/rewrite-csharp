using System.Collections;
using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Rpc.Serialization;

public class SerializationContext(object? before, object? after, DeltaSerializer serializer, List<RpcObjectData> output) : ContextBase(before, serializer)
{
    public object? After { get; init; } = after;
    public List<RpcObjectData> Output { get; } = output;

    public SerializationContext<T> As<T>(T? after)
    {
        if (!ReferenceEquals(after, After)) throw new InvalidOperationException($"Argument must be same as property {nameof(After)}");
        return new SerializationContext<T>((T?)Before, after, Serializer, Output);
    }


    internal void Add(
        object? afterValue) => Add<object>(afterValue);

    internal void Add<TProp>(
        object? afterValue,
        Action<TProp, SerializationContext>? visitIfChanged = null)
        => AppendDatum(RpcObjectData.ObjectState.ADD, null, afterValue, visitIfChanged);

    internal void NoChange(
        object obj
    )
        => AppendDatum<object>(RpcObjectData.ObjectState.NO_CHANGE, null, obj, null);

    internal void Delete(object? beforeValue) => Delete<object>(beforeValue);

    internal void Delete<TProp>(
        object? beforeValue,
        Action<TProp, SerializationContext>? visitIfChanged = null)
        => AppendDatum(RpcObjectData.ObjectState.DELETE, beforeValue, null, visitIfChanged);

    internal void Change(
        object? beforeValue,
        object? afterValue) => Change<object>(beforeValue, afterValue);

    internal void Change<TProp>(
        object? beforeValue,
        object? afterValue,
        Action<TProp, SerializationContext>? visitIfChanged = null)
        => AppendDatum(RpcObjectData.ObjectState.CHANGE, beforeValue, afterValue, visitIfChanged);

    internal void EndOfObject()
        => AppendDatum<object>(RpcObjectData.ObjectState.END_OF_OBJECT, null, null, null);

    internal void AppendDatum<TProp>(
        object? beforeValue,
        object? afterValue,
        Action<TProp, SerializationContext>? visitIfChanged = null)
    {
        var state = GetObjectState(beforeValue, afterValue);

        AppendDatum(state, beforeValue, afterValue, visitIfChanged);
    }

    internal void AppendDatum<TProp>(
        RpcObjectData.ObjectState state,
        object? beforeValue,
        object? afterValue,
        Action<TProp, SerializationContext>? visitIfChanged = null)
    {
        object? value = null;

        var (isNew, refNum) = Serializer.RefTracking.GetOrAddRef(afterValue);
        if (isNew && state is RpcObjectData.ObjectState.ADD or RpcObjectData.ObjectState.CHANGE && visitIfChanged == null)
        {
            value = afterValue;
        }

        var @event = new RpcObjectData()
        {
            State = state,
            Value = value,
            ValueType = GetSerializedValueType(afterValue),
            Ref = refNum,
            // Trace = Serializer.GetTrace(ParentType, callingMethodName, callingArgumentExpression)
            Trace = TraceContext.Current.ToString()
        };
        Output.Add(@event);
        if (state is RpcObjectData.ObjectState.ADD or RpcObjectData.ObjectState.CHANGE && visitIfChanged != null && isNew)
        {
            var childContext = new SerializationContext(beforeValue!, afterValue, Serializer, Output);
            visitIfChanged.Invoke((TProp?)afterValue!, childContext);
        }
    }


    internal RpcObjectData.ObjectState GetObjectState(object? beforeValue, object? afterValue)
    {
        RpcObjectData.ObjectState state;
        var isTree = beforeValue is Tree || afterValue is Tree;
        if (beforeValue == null && afterValue != null)
        {
            state = RpcObjectData.ObjectState.ADD;
        }
        else if (beforeValue != null && afterValue == null)
        {
            state = RpcObjectData.ObjectState.DELETE;
        }
        else if (!(isTree ? ReferenceEquals(beforeValue, afterValue) : Equals(beforeValue, afterValue)))
        {
            state = RpcObjectData.ObjectState.CHANGE;
        }
        else
        {
            state = RpcObjectData.ObjectState.NO_CHANGE;
        }

        return state;
    }

    private string? GetSerializedValueType<TProp>(TProp afterValue)
    {
        if (afterValue == null)
        {
            return null;
        }

        string GetTypeName(object value)
        {
            if (value is IList && value.GetType() is { IsGenericType: true } type)
            {
                var elementType = type.GetGenericArguments()[0];
                return $"List<{elementType.AssemblyQualifiedNameWithoutVersion()}>";
            }

            return afterValue.GetType().AssemblyQualifiedNameWithoutVersion();
        }

        return afterValue switch
        {
            int => "int",
            long => "long",
            string => "string",
            bool => "bool",
            Guid => "Guid",
            IList<int> => "List<int>",
            IList<long> => "List<int>",
            IList<string> => "List<string>",
            IList<bool> => "List<bool>",
            _ => GetTypeName(afterValue)
        };
    }
}

public class SerializationContext<T>(T? before, T? after, DeltaSerializer serializer, List<RpcObjectData> output)
    : SerializationContext(before, after, serializer, output)
{
    public void SerializeList<TProp>(
        Func<T, ICollection<JRightPadded<TProp>>> selector,
        Action<JRightPadded<TProp>, SerializationContext>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "") where TProp : IHasId<Guid>
    {
        SerializeList(selector, x => x.Element.Id, visitIfChanged, callingMethodName, callingArgumentExpression);
    }

    public void SerializeList<TProp>(
        Func<T, ICollection<TProp>?> selector,
        Action<TProp, SerializationContext>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "") where TProp : IHasId<Guid>
    {
        SerializeList(selector, x => x.Id, visitIfChanged, callingMethodName, callingArgumentExpression);
    }

    public void SerializeList<TProp>(
        Func<T, ICollection<TProp>?> selector,
        Func<TProp, object> keySelector,
        Action<TProp, SerializationContext>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "")
    {
        var elementType = typeof(TProp);
        var isElementPolymorphic = elementType.IsInterface || elementType.IsAbstract || elementType.IsGenericType;
        using var propertyTrace = TraceContext.Current.CreateFrame(callingArgumentExpression);
        var beforeValue = Before != null ? selector((T)Before) : default;
        var afterValue = After != null ? selector((T)After) : default;
        if (afterValue == null)
        {
            throw new InvalidOperationException("A DELETE event should have been sent.");
        }

        if (Equals(beforeValue, afterValue) || (IsNullOrEmpty(beforeValue) && IsNullOrEmpty(afterValue)))
        {
            NoChange(afterValue);
        }
        else
        {
            var beforeIdx = beforeValue?
                .Select((item, index) => (Id: keySelector(item), Index: index))
                .ToDictionary(x => x.Id, x => x.Index) ?? new();

            var positions = afterValue
                .Select(t =>
                {
                    if (!beforeIdx.TryGetValue(keySelector(t), out var beforePos))
                    {
                        beforePos = DeltaSerializer.ADDED_LIST_ITEM;
                    }

                    return beforePos;
                })
                .ToList();
            Change<List<int>>(null, positions);

            int i = 0;
            foreach (var anAfter in afterValue)
            {
                // var elementTypeQualifier = isElementPolymorphic ? anAfter!.GetType().Name : "";
                using var indexTrace = TraceContext.Current.CreateFrame($"[{i}]");
                if (!beforeIdx.TryGetValue(keySelector(anAfter), out var beforePos))
                {
                    Add(anAfter, visitIfChanged);
                }
                else
                {
                    var beforeList = (IList<TProp>)beforeValue!;
                    TProp? aBefore = beforeValue == null ? default : beforeList[beforePos];
                    if (ReferenceEquals(aBefore, anAfter))
                    {
                        NoChange(afterValue);
                    }
                    else
                    {
                        Change(aBefore, anAfter, visitIfChanged);
                    }
                }

                i++;
            }
        }
    }

    static bool IsNullOrEmpty<TList>(IEnumerable<TList>? source) => source == null || !source.Any();


    public void SerializeProperty<TProp>(
        Func<T, TProp?> selector,
        Action<TProp, SerializationContext>? visitIfChanged = null,
        [CallerMemberName] string callingMethodName = "",
        [CallerArgumentExpression(nameof(selector))]
        string callingArgumentExpression = "")
    {
        using var propertyTrace = TraceContext.Current.CreateFrame(callingArgumentExpression);
        object? beforeValue = Before != null ? selector((T)Before) : null;
        object? afterValue = After != null ? selector((T)After) : null;
        var state = GetObjectState(beforeValue, afterValue);

        AppendDatum(state, beforeValue, afterValue, visitIfChanged);
    }
}