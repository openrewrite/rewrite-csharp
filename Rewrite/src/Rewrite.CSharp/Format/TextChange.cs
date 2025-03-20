using System.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Rewrite.RewriteCSharp.Format;


/// <summary>
/// Describes a single change when a particular span is replaced with a new text.
/// </summary>
public readonly struct TextChange : IEquatable<TextChange>
{
    /// <summary>
    /// The original span of the changed text.
    /// </summary>

    public TextSpan Span { get; }

    /// <summary>
    /// The old text.
    /// </summary>

    public string? OldText { get; }

    /// <summary>
    /// The new text.
    /// </summary>

    public string? NewText { get; }
    /// <summary>
    /// Initializes a new instance of <see cref="TextChange"/>
    /// </summary>
    /// <param name="span">The original span of the changed text.</param>
    /// <param name="newText">The new text.</param>
    public TextChange(TextSpan span, string oldText, string newText)
        : this()
    {
        if (newText == null)
        {
            throw new ArgumentNullException(nameof(newText));
        }

        this.Span = span;
        this.NewText = newText;
        this.OldText = oldText;
    }

    /// <summary>
    /// Provides a string representation for <see cref="TextChange"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(TextChange)}: {{ {Span}, Before: \"{OldText}\", NewText: \"{NewText}\" }}";
    }

    public override bool Equals(object? obj)
    {
        return obj is TextChange && this.Equals((TextChange)obj);
    }

    public bool Equals(TextChange other)
    {
        return
            EqualityComparer<TextSpan>.Default.Equals(this.Span, other.Span) &&
            EqualityComparer<string>.Default.Equals(this.OldText, other.OldText) &&
            EqualityComparer<string>.Default.Equals(this.NewText, other.NewText);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Span.GetHashCode(), this.NewText?.GetHashCode() ?? 0);
    }

    public static bool operator ==(TextChange left, TextChange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TextChange left, TextChange right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts a <see cref="TextChange"/> to a <see cref="TextChangeRange"/>.
    /// </summary>
    /// <param name="change"></param>
    public static implicit operator TextChangeRange(TextChange change)
    {
        Debug.Assert(change.NewText != null);
        return new TextChangeRange(change.Span, change.NewText.Length);
    }


    internal string GetDebuggerDisplay()
    {
        var newTextDisplay = NewText switch
        {
            null => "null",
            { Length: < 10 } => $"\"{NewText}\"",
            { Length: var length } => $"(NewLength = {length})"
        };
        return $"new TextChange(new TextSpan({Span.Start}, {Span.Length}), {newTextDisplay})";
    }
}

