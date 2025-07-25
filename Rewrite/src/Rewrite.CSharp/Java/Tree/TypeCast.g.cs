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
    public partial class TypeCast(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<TypeTree> clazz,
    Expression expression
    ) : J,Expression,TypedTree    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitTypeCast(this, p);
        }

        public Guid Id { get;  set; } = id;

        public TypeCast WithId(Guid newId)
        {
            return newId == Id ? this : new TypeCast(newId, Prefix, Markers, Clazz, Expression);
        }
        public Space Prefix { get;  set; } = prefix;

        public TypeCast WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new TypeCast(Id, newPrefix, Markers, Clazz, Expression);
        }
        public Markers Markers { get;  set; } = markers;

        public TypeCast WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new TypeCast(Id, Prefix, newMarkers, Clazz, Expression);
        }
        public J.ControlParentheses<TypeTree> Clazz { get;  set; } = clazz;

        public TypeCast WithClazz(J.ControlParentheses<TypeTree> newClazz)
        {
            return ReferenceEquals(newClazz, Clazz) ? this : new TypeCast(Id, Prefix, Markers, newClazz, Expression);
        }
        public Expression Expression { get;  set; } = expression;

        public TypeCast WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, Expression) ? this : new TypeCast(Id, Prefix, Markers, Clazz, newExpression);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeCast && other.Id == Id;
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