using System.Diagnostics.CodeAnalysis;

namespace Rewrite.Rpc;

public class Reference
{
    
    private object? value;

    /// <summary>
    /// Gets the value stored in this reference.
    /// </summary>
    public object? Value => value;

    /// <summary>
    /// Creates a reference wrapper for any instance.
    /// </summary>
    /// <param name="t">Any instance.</param>
    /// <returns>A reference wrapper, which assists the sender to know when to pass by reference rather than by value.</returns>
    public static Reference AsRef(object? t)
    {
        return new Reference() { value = t };
    }

    /// <summary>
    /// Gets the value from a reference or returns the value itself if it is not a reference.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="maybeRef">A reference (or not).</param>
    /// <returns>The value of the reference, or the value itself if it is not a reference.</returns>
    public static T? GetValue<T>(object? maybeRef)
    {
        return (T?)(maybeRef is Reference reference ? reference.Value : maybeRef);
    }

    /// <summary>
    /// Gets the non-null value from a reference or returns the value itself if it is not a reference.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="maybeRef">A reference (or not).</param>
    /// <returns>The value of the reference, or the value itself if it is not a reference.</returns>
    public static T GetValueNonNull<T>(object? maybeRef)
    {
        return GetValue<T>(maybeRef) ?? throw new ArgumentNullException(nameof(maybeRef), "Value cannot be null");
    }
}