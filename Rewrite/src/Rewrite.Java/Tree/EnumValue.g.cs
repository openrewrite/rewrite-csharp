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
    public partial class EnumValue(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> annotations,
    Identifier name,
    NewClass? initializer
    ) : J, MutableTree<EnumValue>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitEnumValue(this, p);
        }

        public Guid Id => id;

        public EnumValue WithId(Guid newId)
        {
            return newId == id ? this : new EnumValue(newId, prefix, markers, annotations, name, initializer);
        }
        public Space Prefix => prefix;

        public EnumValue WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new EnumValue(id, newPrefix, markers, annotations, name, initializer);
        }
        public Markers Markers => markers;

        public EnumValue WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new EnumValue(id, prefix, newMarkers, annotations, name, initializer);
        }
        public IList<J.Annotation> Annotations => annotations;

        public EnumValue WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new EnumValue(id, prefix, markers, newAnnotations, name, initializer);
        }
        public J.Identifier Name => name;

        public EnumValue WithName(J.Identifier newName)
        {
            return ReferenceEquals(newName, name) ? this : new EnumValue(id, prefix, markers, annotations, newName, initializer);
        }
        public J.NewClass? Initializer => initializer;

        public EnumValue WithInitializer(J.NewClass? newInitializer)
        {
            return ReferenceEquals(newInitializer, initializer) ? this : new EnumValue(id, prefix, markers, annotations, name, newInitializer);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is EnumValue && other.Id == Id;
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