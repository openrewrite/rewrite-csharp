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
    public partial class Wildcard(
    Guid id,
    Space prefix,
    Markers markers,
    JLeftPadded<Wildcard.Bound>? wildcardBound,
    NameTree? boundedType
    ) : J, Expression, TypeTree, Expression<Wildcard>, TypedTree<Wildcard>, J<Wildcard>, TypeTree<Wildcard>, MutableTree<Wildcard>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitWildcard(this, p);
        }

        public Guid Id => id;

        public Wildcard WithId(Guid newId)
        {
            return newId == id ? this : new Wildcard(newId, prefix, markers, _wildcardBound, boundedType);
        }
        public Space Prefix => prefix;

        public Wildcard WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Wildcard(id, newPrefix, markers, _wildcardBound, boundedType);
        }
        public Markers Markers => markers;

        public Wildcard WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Wildcard(id, prefix, newMarkers, _wildcardBound, boundedType);
        }
        private readonly JLeftPadded<Bound>? _wildcardBound = wildcardBound;
        public Bound? WildcardBound => _wildcardBound?.Element;

        public Wildcard WithWildcardBound(Bound? newWildcardBound)
        {
            return Padding.WithWildcardBound(newWildcardBound == null ? null : JLeftPadded<Bound>.WithElement(_wildcardBound, newWildcardBound.Value));
        }
        public NameTree? BoundedType => boundedType;

        public Wildcard WithBoundedType(NameTree? newBoundedType)
        {
            return ReferenceEquals(newBoundedType, boundedType) ? this : new Wildcard(id, prefix, markers, _wildcardBound, newBoundedType);
        }
        public enum Bound
        {
            Extends,
            Super,
        }
        public sealed record PaddingHelper(J.Wildcard T)
        {
            public JLeftPadded<J.Wildcard.Bound>? WildcardBound => T._wildcardBound;

            public J.Wildcard WithWildcardBound(JLeftPadded<J.Wildcard.Bound>? newWildcardBound)
            {
                return T._wildcardBound == newWildcardBound ? T : new J.Wildcard(T.Id, T.Prefix, T.Markers, newWildcardBound, T.BoundedType);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Wildcard && other.Id == Id;
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