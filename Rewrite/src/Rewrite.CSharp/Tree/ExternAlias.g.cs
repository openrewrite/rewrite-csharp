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
    public partial class ExternAlias(
    Guid id,
    Space prefix,
    Markers markers,
    JLeftPadded<J.Identifier> identifier
    ) : Cs, Statement, J<ExternAlias>, MutableTree<ExternAlias>
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

        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitExternAlias(this, p);
        }

        public Guid Id => id;

        public ExternAlias WithId(Guid newId)
        {
            return newId == id ? this : new ExternAlias(newId, prefix, markers, _identifier);
        }
        public Space Prefix => prefix;

        public ExternAlias WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ExternAlias(id, newPrefix, markers, _identifier);
        }
        public Markers Markers => markers;

        public ExternAlias WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ExternAlias(id, prefix, newMarkers, _identifier);
        }
        private readonly JLeftPadded<J.Identifier> _identifier = identifier;
        public J.Identifier Identifier => _identifier.Element;

        public ExternAlias WithIdentifier(J.Identifier newIdentifier)
        {
            return Padding.WithIdentifier(_identifier.WithElement(newIdentifier));
        }
        public sealed record PaddingHelper(Cs.ExternAlias T)
        {
            public JLeftPadded<J.Identifier> Identifier => T._identifier;

            public Cs.ExternAlias WithIdentifier(JLeftPadded<J.Identifier> newIdentifier)
            {
                return T._identifier == newIdentifier ? T : new Cs.ExternAlias(T.Id, T.Prefix, T.Markers, newIdentifier);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ExternAlias && other.Id == Id;
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