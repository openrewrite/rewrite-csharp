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
    public partial class TypeCast(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<TypeTree> clazz,
    Expression expression
    ) : J, Expression, TypedTree, MutableTree<TypeCast>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitTypeCast(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public TypeCast WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public TypeCast WithId(Guid newId)
        {
            return newId == id ? this : new TypeCast(newId, prefix, markers, clazz, expression);
        }
        public Space Prefix => prefix;

        public TypeCast WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeCast(id, newPrefix, markers, clazz, expression);
        }
        public Markers Markers => markers;

        public TypeCast WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeCast(id, prefix, newMarkers, clazz, expression);
        }
        public J.ControlParentheses<TypeTree> Clazz => clazz;

        public TypeCast WithClazz(J.ControlParentheses<TypeTree> newClazz)
        {
            return ReferenceEquals(newClazz, clazz) ? this : new TypeCast(id, prefix, markers, newClazz, expression);
        }
        public Expression Expression => expression;

        public TypeCast WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new TypeCast(id, prefix, markers, clazz, newExpression);
        }
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeCast && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}