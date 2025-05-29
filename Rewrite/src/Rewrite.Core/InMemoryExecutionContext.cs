namespace Rewrite.Core;

public class InMemoryExecutionContext : IExecutionContext
{
    private readonly Dictionary<string, object?> _messages = new();
    
    public InMemoryExecutionContext() : this(e => { })
    {
    }

    public InMemoryExecutionContext(Action<Exception> onError)
    {
        OnError = onError;
    }

    public Action<Exception> OnError { get; set; }
    
    
    public T? GetMessage<T>(string key, T? defaultValue = default)
    {
        return _messages.TryGetValue(key, out var value) ? (T?)value : defaultValue;
    }

    public void PutMessage<T>(string key, T? value)
    {
        _messages[key] = value;
    }
}