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
    public partial class SwitchExpression(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> selector,
    Block cases,
    JavaType? type
    ) : J,Expression,TypedTree    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSwitchExpression(this, p);
        }

        public Guid Id { get;  set; } = id;

        public SwitchExpression WithId(Guid newId)
        {
            return newId == Id ? this : new SwitchExpression(newId, Prefix, Markers, Selector, Cases, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public SwitchExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new SwitchExpression(Id, newPrefix, Markers, Selector, Cases, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public SwitchExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new SwitchExpression(Id, Prefix, newMarkers, Selector, Cases, Type);
        }
        public J.ControlParentheses<Expression> Selector { get;  set; } = selector;

        public SwitchExpression WithSelector(J.ControlParentheses<Expression> newSelector)
        {
            return ReferenceEquals(newSelector, Selector) ? this : new SwitchExpression(Id, Prefix, Markers, newSelector, Cases, Type);
        }
        public J.Block Cases { get;  set; } = cases;

        public SwitchExpression WithCases(J.Block newCases)
        {
            return ReferenceEquals(newCases, Cases) ? this : new SwitchExpression(Id, Prefix, Markers, Selector, newCases, Type);
        }
        public JavaType? Type { get;  set; } = type;

        public SwitchExpression WithType(JavaType? newType)
        {
            return newType == Type ? this : new SwitchExpression(Id, Prefix, Markers, Selector, Cases, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchExpression && other.Id == Id;
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