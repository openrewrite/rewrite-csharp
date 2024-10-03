using System.Runtime;
using System.Text;

namespace Rewrite.Core;

public class PrintOutputCapture<TState>(TState p, PrintOutputCapture<TState>.IMarkerPrinter markerPrinter)
{
    public TState Context { get; } = p;

    public IMarkerPrinter MarkerPrinter { get; } = markerPrinter;

    public StringBuilder Out { get; } = new StringBuilder();

    public PrintOutputCapture(TState p) : this(p, IMarkerPrinter.Default)
    {
    }

    public string GetOut()
    {
        return Out.ToString();
    }

    public PrintOutputCapture<TState> Append(string? text)
    {
        if (text is { Length: > 0 })
        {
            Out.Append(text);
        }

        return this;
    }

    public PrintOutputCapture<TState> Append(char c)
    {
        Out.Append(c);
        return this;
    }

    public PrintOutputCapture<TState> Clone()
    {
        return new PrintOutputCapture<TState>(Context, MarkerPrinter);
    }

    public interface IMarkerPrinter
    {
        static readonly IMarkerPrinter Default = new DefaultMarkerPrinter();

        internal class DefaultMarkerPrinter : PrintOutputCapture<TState>.IMarkerPrinter
        {
            public string BeforeSyntax(Marker.Marker marker, Cursor cursor, Func<string, string> commentWrapper)
            {
                return marker.Print(cursor, commentWrapper, false);
            }
        }

        string BeforePrefix(Marker.Marker marker, Cursor cursor, Func<string, string> commentWrapper)
        {
            return "";
        }

        string BeforeSyntax(Marker.Marker marker, Cursor cursor, Func<string, string> commentWrapper)
        {
            return "";
        }

        string AfterSyntax(Marker.Marker marker, Cursor cursor, Func<string, string> commentWrapper)
        {
            return "";
        }
    }

    public override string ToString() => Out.ToString();
}
