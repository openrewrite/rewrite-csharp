using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Rewrite.Core;

#if DEBUG_VISITOR
[DebuggerStepThrough]
#endif
public class Cursor : IEnumerable<object>
{
    protected Cursor? _parent;
    public const string ROOT_VALUE = "root";
    internal virtual Dictionary<string, object>? Messages { get; set; } = null;

    public virtual object? Value { get; init; }
    public string VisitMethod  { get; init; }
    public string ValueExpression  { get; init; }

    public Cursor([CallerMemberName] string callingMethodName = "") : this(null, ROOT_VALUE, callingMethodName)
    {
        
    }

    public Cursor(Cursor? parent, object? value, [CallerMemberName] string callingMethodName = "", [CallerArgumentExpression(nameof(value))] string callingArgumentExpression = "")
    {
        _parent = parent;
        Value = value;
        VisitMethod = callingMethodName;
        ValueExpression = callingArgumentExpression;
    }

    public virtual Cursor? GetParent(int levels = 1)
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

    public Cursor GetRoot()
    {
        var c = this;
        while (c.Parent != null && c.Parent.Value?.ToString() != ROOT_VALUE)
        {
            c = c.Parent;
        }

        return c;
    }

    public bool TryGetValue<T>(out T? value) where T : class
    {
        value = GetValue<T>();
        return value != null;
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

    public T? FirstEnclosing<T>()
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

    // public Cursor DropParentUntil(Func<object?, bool> predicate)
    // {
    //     var c = this;
    //     while (c != null)
    //     {
    //         if (predicate(c.Value))
    //             return c;
    //         c = c.Parent;
    //     }
    //
    //     throw new InvalidOperationException("Expected to find a matching parent for " + this);
    // }

    public void PutMessage(string key, object value)
    {
        if (Messages == null)
        {
            Messages = new Dictionary<string, object>();
        }

        Messages[key] = value;
    }

    public T? GetMessage<T>(string key, T? defaultValue = default)
    {
        return Messages is null ? defaultValue :
            Messages.TryGetValue(key, out var value) ? (T?)value : defaultValue;
    }

    public void Deconstruct(out Cursor? Parent, out object? Value)
    {
        Parent = this.Parent;
        Value = this.Value;
    }
    /// <summary>
    /// Drops parents until a predicate matches.
    /// </summary>
    /// <param name="valuePredicate">The predicate to test against parent values.</param>
    /// <returns>The first parent cursor that matches the predicate.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching parent is found.</exception>
    public Cursor DropParentUntil(Predicate<object?> valuePredicate)
    {
        Cursor? cursor = Parent;
        while (cursor != null && !valuePredicate(cursor.Value))
        {
            cursor = cursor.Parent;
        }
        if (cursor == null)
        {
            throw new InvalidOperationException("Expected to find a matching parent for " + this);
        }
        return cursor;
    }

    /// <summary>
    /// Drops parents while a predicate matches.
    /// </summary>
    /// <param name="valuePredicate">The predicate to test against parent values.</param>
    /// <returns>The first parent cursor that doesn't match the predicate.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no non-matching parent is found.</exception>
    public Cursor DropParentWhile(Predicate<object?> valuePredicate)
    {
        Cursor? cursor = Parent;
        while (cursor != null && valuePredicate(cursor.Value))
        {
            cursor = cursor.Parent;
        }
        if (cursor == null)
        {
            throw new InvalidOperationException("Expected to find a matching parent for " + this);
        }
        return cursor;
    }
    /// <summary>
    /// Return the first parent of the current cursor which points to an AST element, or the root cursor if the current
    /// cursor already points to the root AST element. This skips over non-tree Padding elements.
    /// <br/>
    /// If you do want to access Padding elements, use Parent or ParentOrThrow properties, which do not skip over these elements.
    /// </summary>
    /// <returns>A cursor which either points at the first non-padding parent of the current element</returns>
    public Cursor GetParentTreeCursor()
    {
        return DropParentUntil(it => it is Tree || Equals(it, ROOT_VALUE));
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
        T? t = this.Messages == null ? default : (T)Messages[key];
        return t == null && Parent != null ? Parent.GetNearestMessage<T>(key) : t;
    }

    public IEnumerator<object> GetEnumerator()
    {
        var current = this._parent;
        while (current != null)
        {
            yield return current;
            current = this.Parent;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Cursor<T> As<T>() => new (this);

    public T ComputeMessageIfAbsent<T>(string key, Func<string, T> mappingFunction)
    {
        Messages ??= new Dictionary<string, object>();
    
        if (!Messages.TryGetValue(key, out object? value))
        {
            value = mappingFunction(key);
            Messages[key] = value!;
        }
    
        return (T)value!;
    }
}



public class Cursor<T> : Cursor
{
    // private readonly Cursor _delegated;

    internal Cursor(Cursor cursor)
    {
        if (cursor.Value is not T value) throw new InvalidOperationException($"Cannot create {GetType()} when original cursor Value is {cursor.Value?.GetType()}");
        base.Value = cursor.Value;
        Messages = cursor.Messages;
        _parent = cursor.Parent;
    }

    public new T? Value => (T?)base.Value;
    // public override Cursor? GetParent(int levels = 1) => _delegated.GetParent(levels);
    // internal override Dictionary<string, object>? Messages
    // {
    //     get => _delegated.Messages;
    //     set => _delegated.Messages = value;
    // }
}
