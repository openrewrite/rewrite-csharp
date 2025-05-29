using Rewrite.Core.Marker;

namespace Rewrite.RewriteJava.Tree;
//
// public partial interface J<T> : J  where T : J
// {
//     public new T WithPrefix(Space prefix);
//     J J.WithPrefix(Space prefix) => WithPrefix(prefix);
//     public new T WithMarkers(Markers markers);
//     J J.WithMarkers(Markers markers) => WithMarkers(markers);
// }
public partial interface J : IHasPrefix
{
    public J WithPrefix(Space prefix);
    public J WithMarkers(Markers markers);

    public IList<Comment> Comments => Prefix.Comments;
    // public J WithComments(IList<Comment> comments) => WithPrefix(Prefix.WithComments(comments));
}


public static class JExtensions
{
    public static J WithComments<T>(this T j, IList<Comment> comments) where T : J => j.WithPrefix(j.Prefix.WithComments(comments));
}