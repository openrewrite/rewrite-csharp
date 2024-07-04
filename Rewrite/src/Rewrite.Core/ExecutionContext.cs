namespace Rewrite.Core;

public interface ExecutionContext
{
    const string RequirePrintEqualsInput = "org.openrewrite.requirePrintEqualsInput";
    public T? GetMessage<T>(string key, T? defaultValue = default);

    void PutMessage<T>(string key, T? value);
}