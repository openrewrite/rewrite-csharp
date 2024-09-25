//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    public partial class ArrayType(
    Guid id,
    Space prefix,
    Markers markers,
    TypeTree elementType,
    IList<Annotation>? annotations,
    JLeftPadded<Space>? dimension,
    JavaType type
    ) : J, TypeTree, Expression, MutableTree<ArrayType>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitArrayType(this, p);
        }

        public Guid Id => id;

        public ArrayType WithId(Guid newId)
        {
            return newId == id ? this : new ArrayType(newId, prefix, markers, elementType, annotations, dimension, type);
        }
        public Space Prefix => prefix;

        public ArrayType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayType(id, newPrefix, markers, elementType, annotations, dimension, type);
        }
        public Markers Markers => markers;

        public ArrayType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayType(id, prefix, newMarkers, elementType, annotations, dimension, type);
        }
        public TypeTree ElementType => elementType;

        public ArrayType WithElementType(TypeTree newElementType)
        {
            return ReferenceEquals(newElementType, elementType) ? this : new ArrayType(id, prefix, markers, newElementType, annotations, dimension, type);
        }
        public IList<J.Annotation>? Annotations => annotations;

        public ArrayType WithAnnotations(IList<J.Annotation>? newAnnotations)
        {
            return newAnnotations == annotations ? this : new ArrayType(id, prefix, markers, elementType, newAnnotations, dimension, type);
        }
        public JLeftPadded<Space>? Dimension => dimension;

        public ArrayType WithDimension(JLeftPadded<Space>? newDimension)
        {
            return newDimension == dimension ? this : new ArrayType(id, prefix, markers, elementType, annotations, newDimension, type);
        }
        public JavaType Type => type;

        public ArrayType WithType(JavaType newType)
        {
            return newType == type ? this : new ArrayType(id, prefix, markers, elementType, annotations, dimension, newType);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}