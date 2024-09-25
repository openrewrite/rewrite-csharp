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
    public partial class Continue(
    Guid id,
    Space prefix,
    Markers markers,
    Identifier? label
    ) : J, Statement, MutableTree<Continue>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitContinue(this, p);
        }

        public Guid Id => id;

        public Continue WithId(Guid newId)
        {
            return newId == id ? this : new Continue(newId, prefix, markers, label);
        }
        public Space Prefix => prefix;

        public Continue WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Continue(id, newPrefix, markers, label);
        }
        public Markers Markers => markers;

        public Continue WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Continue(id, prefix, newMarkers, label);
        }
        public J.Identifier? Label => label;

        public Continue WithLabel(J.Identifier? newLabel)
        {
            return ReferenceEquals(newLabel, label) ? this : new Continue(id, prefix, markers, newLabel);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Continue && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}