namespace Rewrite.Core;

public class DelegatingExecutionContext(ExecutionContext @delegate) : ExecutionContext
{
    public T? GetMessage<T>(string key, T? defaultValue = default)
    {
        return @delegate.GetMessage(key, defaultValue);
    }

    public void PutMessage<T>(string key, T? value)
    {
        @delegate.PutMessage(key, value);
    }
}