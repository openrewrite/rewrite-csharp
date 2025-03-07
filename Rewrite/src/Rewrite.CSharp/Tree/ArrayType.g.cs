//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108 // 'member1' hides inherited member 'member2'. Use the new keyword if hiding was intended.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Cs : J
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class ArrayType(
    Guid id,
    Space prefix,
    Markers markers,
    TypeTree? typeExpression,
    IList<J.ArrayDimension> dimensions,
    JavaType? type
    ) : Cs, Expression, TypeTree, Expression<ArrayType>, TypedTree<ArrayType>, J<ArrayType>, TypeTree<ArrayType>, MutableTree<ArrayType>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitArrayType(this, p);
        }

        public Guid Id => id;

        public ArrayType WithId(Guid newId)
        {
            return newId == id ? this : new ArrayType(newId, prefix, markers, typeExpression, dimensions, type);
        }
        public Space Prefix => prefix;

        public ArrayType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayType(id, newPrefix, markers, typeExpression, dimensions, type);
        }
        public Markers Markers => markers;

        public ArrayType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayType(id, prefix, newMarkers, typeExpression, dimensions, type);
        }
        public TypeTree? TypeExpression => typeExpression;

        public ArrayType WithTypeExpression(TypeTree? newTypeExpression)
        {
            return ReferenceEquals(newTypeExpression, typeExpression) ? this : new ArrayType(id, prefix, markers, newTypeExpression, dimensions, type);
        }
        public IList<J.ArrayDimension> Dimensions => dimensions;

        public ArrayType WithDimensions(IList<J.ArrayDimension> newDimensions)
        {
            return newDimensions == dimensions ? this : new ArrayType(id, prefix, markers, typeExpression, newDimensions, type);
        }
        public JavaType? Type => type;

        public ArrayType WithType(JavaType? newType)
        {
            return newType == type ? this : new ArrayType(id, prefix, markers, typeExpression, dimensions, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayType && other.Id == Id;
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}