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
    /// <summary>
    /// Represents a C# ref expression used to pass variables by reference.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Method call with ref argument
    ///     Process(ref value);
    ///     // Return ref value
    ///     return ref field;
    ///     // Local ref assignment
    ///     ref int x = ref field;
    ///     // Ref property return
    ///     public ref int Property =&gt; ref field;
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class RefExpression(
    Guid id,
    Space prefix,
    Markers markers,
    Expression expression
    ) : Cs, Expression, Expression<RefExpression>, J<RefExpression>, MutableTree<RefExpression>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitRefExpression(this, p);
        }

        public Guid Id => id;

        public RefExpression WithId(Guid newId)
        {
            return newId == id ? this : new RefExpression(newId, prefix, markers, expression);
        }
        public Space Prefix => prefix;

        public RefExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new RefExpression(id, newPrefix, markers, expression);
        }
        public Markers Markers => markers;

        public RefExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new RefExpression(id, prefix, newMarkers, expression);
        }
        public Expression Expression => expression;

        public RefExpression WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, expression) ? this : new RefExpression(id, prefix, markers, newExpression);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is RefExpression && other.Id == Id;
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