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
    public partial class AwaitExpression(
    Guid id,
    Space prefix,
    Markers markers,
    Expression expression,
    JavaType? type
    ) : Cs, Expression, Expression<AwaitExpression>, MutableTree<AwaitExpression>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitAwaitExpression(this, p);
        }

        public Guid Id => id;

        public AwaitExpression WithId(Guid newId)
        {
            return newId == id ? this : new AwaitExpression(newId, prefix, markers, expression, type);
        }
        public Space Prefix => prefix;

        public AwaitExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AwaitExpression(id, newPrefix, markers, expression, type);
        }
        public Markers Markers => markers;

        public AwaitExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AwaitExpression(id, prefix, newMarkers, expression, type);
        }
        public Expression Expression => expression;

        public AwaitExpression WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new AwaitExpression(id, prefix, markers, newExpression, type);
        }
        public JavaType? Type => type;

        public AwaitExpression WithType(JavaType? newType)
        {
            return newType == type ? this : new AwaitExpression(id, prefix, markers, expression, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AwaitExpression && other.Id == Id;
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