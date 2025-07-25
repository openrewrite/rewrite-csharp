namespace Rewrite.Rpc.Serialization;

public class ContextBase(object? before, DeltaSerializer serializer)
{
    public object? Before { get; init; } = before;

    public DeltaSerializer Serializer { get; } = serializer;
}