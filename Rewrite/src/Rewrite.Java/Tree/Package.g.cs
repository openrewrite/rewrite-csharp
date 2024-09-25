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
    public partial class Package(
    Guid id,
    Space prefix,
    Markers markers,
    Expression expression,
    IList<Annotation> annotations
    ) : Statement, J, MutableTree<Package>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitPackage(this, p);
        }

        public Guid Id => id;

        public Package WithId(Guid newId)
        {
            return newId == id ? this : new Package(newId, prefix, markers, expression, annotations);
        }
        public Space Prefix => prefix;

        public Package WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Package(id, newPrefix, markers, expression, annotations);
        }
        public Markers Markers => markers;

        public Package WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Package(id, prefix, newMarkers, expression, annotations);
        }
        public Expression Expression => expression;

        public Package WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Package(id, prefix, markers, newExpression, annotations);
        }
        public IList<J.Annotation> Annotations => annotations;

        public Package WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Package(id, prefix, markers, expression, newAnnotations);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Package && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}