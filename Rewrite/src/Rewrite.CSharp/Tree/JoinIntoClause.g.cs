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
    /// Represents the 'into' portion of a group join clause in C# LINQ syntax.
    /// Used to specify the identifier that will hold the grouped results.
    /// <br/>
    /// For example:
    /// <code>
    /// // Group join using into clause
    /// join category in categories
    ///    on product.CategoryId equals category.Id
    ///    into productCategories
    /// // Multiple group joins
    /// join orders in db.Orders
    ///    on customer.Id equals orders.CustomerId
    ///    into customerOrders
    /// join returns in db.Returns
    ///    on customer.Id equals returns.CustomerId
    ///    into customerReturns
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class JoinIntoClause(
    Guid id,
    Space prefix,
    Markers markers,
    J.Identifier identifier
    ) : Cs, Cs.QueryClause, J<JoinIntoClause>, MutableTree<JoinIntoClause>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitJoinIntoClause(this, p);
        }

        public Guid Id => id;

        public JoinIntoClause WithId(Guid newId)
        {
            return newId == id ? this : new JoinIntoClause(newId, prefix, markers, identifier);
        }
        public Space Prefix => prefix;

        public JoinIntoClause WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new JoinIntoClause(id, newPrefix, markers, identifier);
        }
        public Markers Markers => markers;

        public JoinIntoClause WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new JoinIntoClause(id, prefix, newMarkers, identifier);
        }
        public J.Identifier Identifier => identifier;

        public JoinIntoClause WithIdentifier(J.Identifier newIdentifier)
        {
            return ReferenceEquals(newIdentifier, identifier) ? this : new JoinIntoClause(id, prefix, markers, newIdentifier);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is JoinIntoClause && other.Id == Id;
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