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
    public partial class CollectionExpression(
    Guid id,
    Space prefix,
    Markers markers,
    IList<JRightPadded<Expression>> elements,
    JavaType? type
    ) : Cs,Expression,TypedTree    {
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
            return v.VisitCollectionExpression(this, p);
        }

        public Guid Id { get;  set; } = id;

        public CollectionExpression WithId(Guid newId)
        {
            return newId == Id ? this : new CollectionExpression(newId, Prefix, Markers, _elements, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public CollectionExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new CollectionExpression(Id, newPrefix, Markers, _elements, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public CollectionExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new CollectionExpression(Id, Prefix, newMarkers, _elements, Type);
        }
        private IList<JRightPadded<Expression>> _elements = elements;
        public IList<Expression> Elements => _elements.Elements();

        public CollectionExpression WithElements(IList<Expression> newElements)
        {
            return Padding.WithElements(_elements.WithElements(newElements));
        }
        public JavaType? Type { get;  set; } = type;

        public CollectionExpression WithType(JavaType? newType)
        {
            return newType == Type ? this : new CollectionExpression(Id, Prefix, Markers, _elements, newType);
        }
        public sealed record PaddingHelper(Cs.CollectionExpression T)
        {
            public IList<JRightPadded<Expression>> Elements { get => T._elements;  set => T._elements = value; }

            public Cs.CollectionExpression WithElements(IList<JRightPadded<Expression>> newElements)
            {
                return Elements == newElements ? T : new Cs.CollectionExpression(T.Id, T.Prefix, T.Markers, newElements, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CollectionExpression && other.Id == Id;
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