using System.Diagnostics;

namespace Rewrite.Core;

#if DEBUG_VISITOR
[DebuggerStepThrough]
#endif
public class Cursor
{
    private readonly Cursor? _parent;
    public const string ROOT_VALUE = "root";

    private Dictionary<string, object>? _messages;

    public Cursor(Cursor? parent, object? value)
    {
        _parent = parent;
        Value = value;
    }

    public Cursor? GetParent(int levels = 1)
    {
        var cursor = this;
        for (var i = 0; i < levels && cursor != null; i++)
        {
            cursor = cursor._parent;
        }

        return cursor;
    }

    public bool IsRoot => Value?.ToString() == ROOT_VALUE;

    public Cursor? Parent => GetParent(1);
    public object? Value { get; init; }

    public Cursor GetRoot()
    {
        var c = this;
        while (c.Parent != null && c.Parent.Value?.ToString() != ROOT_VALUE)
        {
            c = c.Parent;
        }

        return c;
    }

    public T? GetValue<T>() where T : class
    {
        return Value as T;
    }
    public Cursor GetParentOrThrow(int levels = 1)
    {
        var parent = GetParent(levels);
        if (parent == null)
        {
            throw new InvalidOperationException("Expected to find a parent for " + this);
        }

        return parent;
    }

    public T FirstEnclosingOrThrow<T>()
    {
        return FirstEnclosing<T>() ??
               throw new InvalidOperationException("Expected to find enclosing " + typeof(T).Name);
    }

    private T? FirstEnclosing<T>()
    {
        var c = this;
        while (c != null)
        {
            if (c.Value is T value)
                return value;
            c = c.Parent;
        }

        return default;
    }

    public Cursor DropParentUntil(Func<object?, bool> predicate)
    {
        var c = this;
        while (c != null)
        {
            if (predicate(c.Value))
                return c;
            c = c.Parent;
        }

        throw new InvalidOperationException("Expected to find a matching parent for " + this);
    }

    public void PutMessage(string key, object value)
    {
        if (_messages == null)
        {
            _messages = new Dictionary<string, object>();
        }

        _messages[key] = value;
    }

    public T? GetMessage<T>(string key, T? defaultValue = default)
    {
        return _messages is null ? defaultValue :
            _messages.TryGetValue(key, out var value) ? (T?)value : defaultValue;
    }

    public void Deconstruct(out Cursor? Parent, out object? Value)
    {
        Parent = this.Parent;
        Value = this.Value;
    }


    /// <summary>
    /// Puts a message on the first enclosing instance of the specified type.
    /// </summary>
    /// <param name="key">The message key</param>
    /// <param name="value">The message value</param>
    public void PutMessageOnFirstEnclosing<T>(string key, object value) => PutMessageOnFirstEnclosing(typeof(T), key, value);

    /// <summary>
    /// Puts a message on the first enclosing instance of the specified type.
    /// </summary>
    /// <param name="enclosing">The enclosing type to search for</param>
    /// <param name="key">The message key</param>
    /// <param name="value">The message value</param>
    public void PutMessageOnFirstEnclosing(Type enclosing, string key, object value)
    {
        if (enclosing.IsInstanceOfType(this.Value))
        {
            PutMessage(key, value);
        }
        else if (Parent != null)
        {
            Parent.PutMessageOnFirstEnclosing(enclosing, key, value);
        }
    }


    /// <summary>
    /// Finds the closest message matching the provided key, leaving it in the message map for further access.
    /// </summary>
    /// <typeparam name="T">The expected value of the message.</typeparam>
    /// <param name="key">The message key to find.</param>
    /// <returns>The closest message matching the provided key in the cursor stack, or <c>null</c> if none.</returns>
    public T? GetNearestMessage<T>(string key)
    {
        T? t = this._messages == null ? default : (T)_messages[key];
        return t == null && Parent != null ? Parent.GetNearestMessage<T>(key) : t;
    }

}
