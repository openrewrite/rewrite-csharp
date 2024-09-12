namespace Rewrite.Core;

public interface Validated<out T>
{
    static Validated<T> None()
    {
        return new ValidatedNone<T>();
    }

    bool IsInvalid()
    {
        return !IsValid();
    }

    bool IsValid();
}

internal class ValidatedNone<T> : Validated<T>
{
    public bool IsValid()
    {
        return true;
    }
}