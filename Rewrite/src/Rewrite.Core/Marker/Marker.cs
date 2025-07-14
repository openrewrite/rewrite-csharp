namespace Rewrite.Core.Marker;

public interface Marker : IEquatable<Marker>, IHasId<Guid>
{
    string Print(Cursor cursor, Func<string, string> commentWrapper, bool verbose)
    {
        return "";
    }
}