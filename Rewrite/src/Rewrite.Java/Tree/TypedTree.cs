using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;


public interface TypedTree<T> : TypedTree where T : TypedTree
{
    public new T WithType(JavaType? type);
    TypedTree TypedTree.WithType(JavaType? type) => WithType(type);
}
/// <summary>
/// A tree with type attribution information. Unlike <see cref="TypeTree"/>,
/// this does not necessarily mean the tree is the name of a type. So for
/// example, a <see cref="J.MethodInvocation"/> is a <see cref="TypedTree"/> but
/// not a <see cref="TypeTree"/>.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface TypedTree : J
{
    public JavaType? Type { get; }

    public TypedTree WithType(JavaType? type);
}
