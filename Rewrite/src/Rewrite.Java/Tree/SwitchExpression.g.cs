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
    public partial class SwitchExpression(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> selector,
    Block cases
    ) : J, Expression, TypedTree, MutableTree<SwitchExpression>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSwitchExpression(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public SwitchExpression WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public SwitchExpression WithId(Guid newId)
        {
            return newId == id ? this : new SwitchExpression(newId, prefix, markers, selector, cases);
        }
        public Space Prefix => prefix;

        public SwitchExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new SwitchExpression(id, newPrefix, markers, selector, cases);
        }
        public Markers Markers => markers;

        public SwitchExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new SwitchExpression(id, prefix, newMarkers, selector, cases);
        }
        public J.ControlParentheses<Expression> Selector => selector;

        public SwitchExpression WithSelector(J.ControlParentheses<Expression> newSelector)
        {
            return ReferenceEquals(newSelector, selector) ? this : new SwitchExpression(id, prefix, markers, newSelector, cases);
        }
        public J.Block Cases => cases;

        public SwitchExpression WithCases(J.Block newCases)
        {
            return ReferenceEquals(newCases, cases) ? this : new SwitchExpression(id, prefix, markers, selector, newCases);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchExpression && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}