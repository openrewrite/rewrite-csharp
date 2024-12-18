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
    public partial class Keyword(
    Guid id,
    Space prefix,
    Markers markers,
    Keyword.KeywordKind kind
    ) : Cs, J<Keyword>, MutableTree<Keyword>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitKeyword(this, p);
        }

        public Guid Id => id;

        public Keyword WithId(Guid newId)
        {
            return newId == id ? this : new Keyword(newId, prefix, markers, kind);
        }
        public Space Prefix => prefix;

        public Keyword WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Keyword(id, newPrefix, markers, kind);
        }
        public Markers Markers => markers;

        public Keyword WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Keyword(id, prefix, newMarkers, kind);
        }
        public KeywordKind Kind => kind;

        public Keyword WithKind(KeywordKind newKind)
        {
            return newKind == kind ? this : new Keyword(id, prefix, markers, newKind);
        }
        public enum KeywordKind
        {
            Ref,
            Out,
            Await,
            Base,
            This,
            Break,
            Return,
            Not,
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Keyword && other.Id == Id;
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