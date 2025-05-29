namespace Rewrite.Core;

public interface IValidated<out T>
{
    static IValidated<T> None { get; } =  new ValidatedNone<T>();

    bool IsInvalid()
    {
        return !IsValid();
    }

    bool IsValid();
}

internal class ValidatedNone<T> : IValidated<T>
{
    public bool IsValid()
    {
        return true;
    }
}