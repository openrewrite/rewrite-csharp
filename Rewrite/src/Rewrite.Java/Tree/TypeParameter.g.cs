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
    public partial class TypeParameter(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> annotations,
    IList<Modifier> modifiers,
    Expression name,
    JContainer<TypeTree>? bounds
    ) : J, MutableTree<TypeParameter>
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
            return v.VisitTypeParameter(this, p);
        }

        public Guid Id => id;

        public TypeParameter WithId(Guid newId)
        {
            return newId == id ? this : new TypeParameter(newId, prefix, markers, annotations, modifiers, name, _bounds);
        }
        public Space Prefix => prefix;

        public TypeParameter WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeParameter(id, newPrefix, markers, annotations, modifiers, name, _bounds);
        }
        public Markers Markers => markers;

        public TypeParameter WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeParameter(id, prefix, newMarkers, annotations, modifiers, name, _bounds);
        }
        public IList<J.Annotation> Annotations => annotations;

        public TypeParameter WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new TypeParameter(id, prefix, markers, newAnnotations, modifiers, name, _bounds);
        }
        public IList<J.Modifier> Modifiers => modifiers;

        public TypeParameter WithModifiers(IList<J.Modifier> newModifiers)
        {
            return newModifiers == modifiers ? this : new TypeParameter(id, prefix, markers, annotations, newModifiers, name, _bounds);
        }
        public Expression Name => name;

        public TypeParameter WithName(Expression newName)
        {
            return ReferenceEquals(newName, name) ? this : new TypeParameter(id, prefix, markers, annotations, modifiers, newName, _bounds);
        }
        private readonly JContainer<TypeTree>? _bounds = bounds;
        public IList<TypeTree>? Bounds => _bounds?.GetElements();

        public TypeParameter WithBounds(IList<TypeTree>? newBounds)
        {
            return Padding.WithBounds(JContainer<TypeTree>.WithElementsNullable(_bounds, newBounds));
        }
        public sealed record PaddingHelper(J.TypeParameter T)
        {
            public JContainer<TypeTree>? Bounds => T._bounds;

            public J.TypeParameter WithBounds(JContainer<TypeTree>? newBounds)
            {
                return T._bounds == newBounds ? T : new J.TypeParameter(T.Id, T.Prefix, T.Markers, T.Annotations, T.Modifiers, T.Name, newBounds);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeParameter && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}