using System.Runtime;
using System.Text;

namespace Rewrite.Core;

public class PrintOutputCapture<P>(P p, PrintOutputCapture<P>.IMarkerPrinter markerPrinter)
{
    public P Context { get; } = p;

    public IMarkerPrinter MarkerPrinter { get; } = markerPrinter;

    public StringBuilder Out { get; } = new StringBuilder();

    public PrintOutputCapture(P p) : this(p, IMarkerPrinter.Default)
    {
    }

    public string GetOut()
    {
        return Out.ToString();
    }

    public PrintOutputCapture<P> Append(string? text)
    {
        if (text is { Length: > 0 })
        {
            Out.Append(text);
        }

        return this;
    }

    public PrintOutputCapture<P> Append(char c)
    {
        Out.Append(c);
        return this;
    }

    public PrintOutputCapture<P> Clone()
    {
        return new PrintOutputCapture<P>(Context, MarkerPrinter);
    }

    public interface IMarkerPrinter
    {
        static readonly IMarkerPrinter Default = new DefaultMarkerPrinter();

        internal class DefaultMarkerPrinter : PrintOutputCapture<P>.IMarkerPrinter
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
}
