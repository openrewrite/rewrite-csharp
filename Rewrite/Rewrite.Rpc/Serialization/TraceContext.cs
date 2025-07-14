using System.Text.RegularExpressions;
using NMica.Utils;

namespace Rewrite.Rpc.Serialization;

internal class TraceContext : IDisposable
{
    [ThreadStatic] private static TraceContext? _current;

    
    internal static TraceContext Current
    {
        get => _current ??= new TraceContext();
        set => _current = value;
    }

    public static TraceContext StartNew() => _current = new();
    
    private Stack<string> _stack { get; } = new();
    string GetPropertyNameFromExpression(string expression) => Regex.Replace(expression, $@"^[\w]+\s+=>\s+[\w]+\.", "").Replace("Padding.","");

    internal IDisposable CreateFrame(string propertyExpression)
    {
        var traceFrame = GetPropertyNameFromExpression(propertyExpression);
        _stack.Push(traceFrame);
        return DelegateDisposable.CreateBracket(cleanup: () => _stack.Pop());
    }


    public override string ToString()
    {
        var frames = _stack.Reverse().ToArray();
        if (frames.Length == 0)
            return "<root>";
        return string.Join(".", frames);

    }


    public void Dispose()
    {
        _stack.Clear();
    }
}