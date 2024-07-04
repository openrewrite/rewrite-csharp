namespace Rewrite.Core.Marker;

public interface Markup : Marker
{
    string Message { get; }

    string? Detail { get; }

    static T ErrorMarkup<T>(T t, Exception e) where T : class, MutableTree<T>
    {
        return AddMarkup(t, new Error(Tree.RandomId(), e.Message, e.StackTrace));
    }

    static T WarnMarkup<T>(T t, Exception e) where T : class, MutableTree<T>
    {
        return AddMarkup(t, new Warn(Tree.RandomId(), e.Message, e.StackTrace));
    }

    static T AddMarkup<T>(T tree, Markup markup) where T : class, MutableTree<T>
    {
        return tree.WithMarkers(tree.Markers.Add(markup));
    }

    public record Error(Guid Id, string Message, string? Detail) : Markup
    {
        public virtual bool Equals(Marker? other)
        {
            return other is Error && other.Id == Id;
        }
    }

    public record Warn(Guid Id, string Message, string? Detail) : Markup
    {
        public virtual bool Equals(Marker? other)
        {
            return other is Warn && other.Id == Id;
        }
    }
}