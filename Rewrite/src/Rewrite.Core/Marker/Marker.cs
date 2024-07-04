namespace Rewrite.Core.Marker;

public interface Marker : IEquatable<Marker>
{
    Guid Id { get; }

    string Print(Cursor cursor, Func<string, string> commentWrapper, bool verbose)
    {
        return "";
    }
}