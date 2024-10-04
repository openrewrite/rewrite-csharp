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
    public partial class Return(
    Guid id,
    Space prefix,
    Markers markers,
    Expression? expression
    ) : J, Statement, MutableTree<Return>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitReturn(this, p);
        }

        public Guid Id => id;

        public Return WithId(Guid newId)
        {
            return newId == id ? this : new Return(newId, prefix, markers, expression);
        }
        public Space Prefix => prefix;

        public Return WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Return(id, newPrefix, markers, expression);
        }
        public Markers Markers => markers;

        public Return WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Return(id, prefix, newMarkers, expression);
        }
        public Expression? Expression => expression;

        public Return WithExpression(Expression? newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new Return(id, prefix, markers, newExpression);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Return && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}