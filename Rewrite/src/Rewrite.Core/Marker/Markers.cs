using System.Collections;
using System.Collections.Immutable;
using System.Text;

namespace Rewrite.Core.Marker;

public partial record Markers(Guid Id, IList<Marker> MarkerList) : IReadOnlyCollection<Marker>
{
    public static readonly Markers EMPTY = new(Tree.RandomId(), ImmutableList<Marker>.Empty);

    public static Markers Create(params Marker[] markers)
    {
        return new Markers( Core.Tree.RandomId(), markers.ToImmutableList());
    }

    public Markers WithId(Guid id)
    {
        return id == Id ? this : this with { Id = id };
    }

    public Markers WithMarkers(IList<Marker> markers)
    {
        return ReferenceEquals(markers, MarkerList) ? this : this with { MarkerList = markers };
    }

    public virtual bool Equals(Markers? other)
    {
        return other != null && other.Id == Id;
    }

    public IEnumerator<Marker> GetEnumerator()
    {
        return MarkerList.GetEnumerator();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)MarkerList).GetEnumerator();
    }

    public Markers Add<T>(T marker) where T : Marker
    {
        foreach (var m in MarkerList) {
            if (ReferenceEquals(marker, m)) {
                return this;
            }
        }
        IList<Marker> updatedMarker = new List<Marker>(MarkerList);
        updatedMarker.Add(marker);
        return this with { MarkerList = updatedMarker };
    }
    public Markers AddIfAbsent<T>(T marker) where T : Marker
    {
        foreach (var m in MarkerList) {
            if (m is T) {
                return this;
            }
        }
        IList<Marker> updated = new List<Marker>(MarkerList);
        updated.Add(marker);
        return this with { MarkerList = updated };
    }

    public T? FindFirst<T>() where T : Marker
    {
        return MarkerList.OfType<T>().FirstOrDefault();
    }
    public T? FindFirst<T>(Func<T, bool> predicate) where T : Marker
    {
        return MarkerList.OfType<T>().FirstOrDefault(predicate);
    }

    public bool Contains<T>() where T : Marker => FindFirst<T>() != null;
    public bool Contains<T>(Func<T, bool> predicate) where T : Marker => FindFirst(predicate) != null;

    public Markers SetByType(Marker marker)
    {
        throw new NotImplementedException();
    }

    public int Count => MarkerList.Count;

    public override string ToString()
    {
        var sb = new StringBuilder($"{Id}");
        if (MarkerList.Count == 0)
        {
            return sb.ToString();
        }
        if (MarkerList.Count <= 3)
        {
            sb.Append(string.Join(", ", MarkerList.Select(x => x.GetType().Name)));
        }
        else
        {
            sb.Append($"[{MarkerList.Count} markers");
        }

        return sb.ToString();
    }
}
