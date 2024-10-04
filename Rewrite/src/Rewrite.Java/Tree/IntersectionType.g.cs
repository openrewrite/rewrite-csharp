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
    public partial class IntersectionType(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<TypeTree> bounds
    ) : J, TypeTree, Expression, MutableTree<IntersectionType>
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
            return v.VisitIntersectionType(this, p);
        }

        public JavaType? Type => Extensions.GetJavaType(this);

        public IntersectionType WithType(JavaType newType)
        {
            return Extensions.WithJavaType(this, newType);
        }
        public Guid Id => id;

        public IntersectionType WithId(Guid newId)
        {
            return newId == id ? this : new IntersectionType(newId, prefix, markers, _bounds);
        }
        public Space Prefix => prefix;

        public IntersectionType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new IntersectionType(id, newPrefix, markers, _bounds);
        }
        public Markers Markers => markers;

        public IntersectionType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new IntersectionType(id, prefix, newMarkers, _bounds);
        }
        private readonly JContainer<TypeTree> _bounds = bounds;
        public IList<TypeTree> Bounds => _bounds.GetElements();

        public IntersectionType WithBounds(IList<TypeTree> newBounds)
        {
            return Padding.WithBounds(JContainer<TypeTree>.WithElements(_bounds, newBounds));
        }
        public sealed record PaddingHelper(J.IntersectionType T)
        {
            public JContainer<TypeTree> Bounds => T._bounds;

            public J.IntersectionType WithBounds(JContainer<TypeTree> newBounds)
            {
                return T._bounds == newBounds ? T : new J.IntersectionType(T.Id, T.Prefix, T.Markers, newBounds);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is IntersectionType && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}