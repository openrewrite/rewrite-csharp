using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

public interface TypeTree<T> : TypeTree, TypedTree<T> where T : TypeTree
{
    // public new T WithType(JavaType? type);
    // TypeTree TypeTree.WithType(JavaType? type) => WithType(type);
}
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public partial interface TypeTree : NameTree
{
    // public new TypeTree WithType(JavaType? type) => (TypeTree)((TypedTree)this).WithType(type);
}
