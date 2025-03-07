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
    public partial class Synchronized(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> @lock,
    Block body
    ) : J, Statement, J<Synchronized>, MutableTree<Synchronized>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitSynchronized(this, p);
        }

        public Guid Id => id;

        public Synchronized WithId(Guid newId)
        {
            return newId == id ? this : new Synchronized(newId, prefix, markers, @lock, body);
        }
        public Space Prefix => prefix;

        public Synchronized WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Synchronized(id, newPrefix, markers, @lock, body);
        }
        public Markers Markers => markers;

        public Synchronized WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Synchronized(id, prefix, newMarkers, @lock, body);
        }
        public J.ControlParentheses<Expression> Lock => @lock;

        public Synchronized WithLock(J.ControlParentheses<Expression> newLock)
        {
            return ReferenceEquals(newLock, @lock) ? this : new Synchronized(id, prefix, markers, newLock, body);
        }
        public J.Block Body => body;

        public Synchronized WithBody(J.Block newBody)
        {
            return ReferenceEquals(newBody, body) ? this : new Synchronized(id, prefix, markers, @lock, newBody);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Synchronized && other.Id == Id;
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