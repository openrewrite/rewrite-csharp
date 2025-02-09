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
    public partial class MultiCatch(
    Guid id,
    Space prefix,
    Markers markers,
    IList<JRightPadded<NameTree>> alternatives
    ) : J, TypeTree, TypedTree<MultiCatch>, J<MultiCatch>, TypeTree<MultiCatch>, MutableTree<MultiCatch>
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
            return v.VisitMultiCatch(this, p);
        }

        public Guid Id => id;

        public MultiCatch WithId(Guid newId)
        {
            return newId == id ? this : new MultiCatch(newId, prefix, markers, _alternatives);
        }
        public Space Prefix => prefix;

        public MultiCatch WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new MultiCatch(id, newPrefix, markers, _alternatives);
        }
        public Markers Markers => markers;

        public MultiCatch WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new MultiCatch(id, prefix, newMarkers, _alternatives);
        }
        private readonly IList<JRightPadded<NameTree>> _alternatives = alternatives;
        public IList<NameTree> Alternatives => _alternatives.Elements();

        public MultiCatch WithAlternatives(IList<NameTree> newAlternatives)
        {
            return Padding.WithAlternatives(_alternatives.WithElements(newAlternatives));
        }
        public sealed record PaddingHelper(J.MultiCatch T)
        {
            public IList<JRightPadded<NameTree>> Alternatives => T._alternatives;

            public J.MultiCatch WithAlternatives(IList<JRightPadded<NameTree>> newAlternatives)
            {
                return T._alternatives == newAlternatives ? T : new J.MultiCatch(T.Id, T.Prefix, T.Markers, newAlternatives);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is MultiCatch && other.Id == Id;
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