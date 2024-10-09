//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
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
    public partial class Break(
    Guid id,
    Space prefix,
    Markers markers,
    Identifier? label
    ) : J, Statement, MutableTree<Break>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitBreak(this, p);
        }

        public Guid Id => id;

        public Break WithId(Guid newId)
        {
            return newId == id ? this : new Break(newId, prefix, markers, label);
        }
        public Space Prefix => prefix;

        public Break WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Break(id, newPrefix, markers, label);
        }
        public Markers Markers => markers;

        public Break WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Break(id, prefix, newMarkers, label);
        }
        public J.Identifier? Label => label;

        public Break WithLabel(J.Identifier? newLabel)
        {
            return ReferenceEquals(newLabel, label) ? this : new Break(id, prefix, markers, newLabel);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Break && other.Id == Id;
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