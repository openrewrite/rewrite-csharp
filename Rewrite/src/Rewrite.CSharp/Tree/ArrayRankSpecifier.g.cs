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
    public partial class ArrayRankSpecifier(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Expression> sizes
    ) : Cs, Expression, MutableTree<ArrayRankSpecifier>
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
            return v.VisitArrayRankSpecifier(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public ArrayRankSpecifier WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public ArrayRankSpecifier WithId(Guid newId)
        {
            return newId == id ? this : new ArrayRankSpecifier(newId, prefix, markers, _sizes);
        }
        public Space Prefix => prefix;

        public ArrayRankSpecifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ArrayRankSpecifier(id, newPrefix, markers, _sizes);
        }
        public Markers Markers => markers;

        public ArrayRankSpecifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ArrayRankSpecifier(id, prefix, newMarkers, _sizes);
        }
        private readonly JContainer<Expression> _sizes = sizes;
        public IList<Expression> Sizes => _sizes.GetElements();

        public ArrayRankSpecifier WithSizes(IList<Expression> newSizes)
        {
            return Padding.WithSizes(JContainer<Expression>.WithElements(_sizes, newSizes));
        }
        public sealed record PaddingHelper(Cs.ArrayRankSpecifier T)
        {
            public JContainer<Expression> Sizes => T._sizes;

            public Cs.ArrayRankSpecifier WithSizes(JContainer<Expression> newSizes)
            {
                return T._sizes == newSizes ? T : new Cs.ArrayRankSpecifier(T.Id, T.Prefix, T.Markers, newSizes);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ArrayRankSpecifier && other.Id == Id;
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