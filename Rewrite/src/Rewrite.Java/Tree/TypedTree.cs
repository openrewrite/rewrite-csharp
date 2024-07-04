using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface TypedTree : J
{
    public JavaType? Type => Extensions.GetJavaType(this);

    public T WithType<T>(JavaType? type) where T : J
    {
        return Extensions.WithType(this as dynamic, type);
    }
}