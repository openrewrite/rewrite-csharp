namespace Rewrite.Remote;

public record DiffEvent(EventType EventType, string? ConcreteType, object? Msg);

public enum EventType
{
    NoChange,
    Update,
    Add,
    Delete,
    Move,
    StartList,
    EndList,
}