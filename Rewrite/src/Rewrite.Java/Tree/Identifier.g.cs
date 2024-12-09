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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public sealed partial class Identifier(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> annotations,
    string simpleName,
    JavaType? type,
    JavaType.Variable? fieldType
    ) : J, TypeTree, Expression, Expression<Identifier>, TypedTree<Identifier>, J<Identifier>, TypeTree<Identifier>, MutableTree<Identifier>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitIdentifier(this, p);
        }

        public Guid Id => id;

        public Identifier WithId(Guid newId)
        {
            return newId == id ? this : new Identifier(newId, prefix, markers, annotations, simpleName, type, fieldType);
        }
        public Space Prefix => prefix;

        public Identifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Identifier(id, newPrefix, markers, annotations, simpleName, type, fieldType);
        }
        public Markers Markers => markers;

        public Identifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Identifier(id, prefix, newMarkers, annotations, simpleName, type, fieldType);
        }
        public IList<J.Annotation> Annotations => annotations;

        public Identifier WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Identifier(id, prefix, markers, newAnnotations, simpleName, type, fieldType);
        }
        public string SimpleName => simpleName;

        public Identifier WithSimpleName(string newSimpleName)
        {
            return newSimpleName == simpleName ? this : new Identifier(id, prefix, markers, annotations, newSimpleName, type, fieldType);
        }
        public JavaType? Type => type;

        public Identifier WithType(JavaType? newType)
        {
            return newType == type ? this : new Identifier(id, prefix, markers, annotations, simpleName, newType, fieldType);
        }
        public JavaType.Variable? FieldType => fieldType;

        public Identifier WithFieldType(JavaType.Variable? newFieldType)
        {
            return newFieldType == fieldType ? this : new Identifier(id, prefix, markers, annotations, simpleName, type, newFieldType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Identifier && other.Id == Id;
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