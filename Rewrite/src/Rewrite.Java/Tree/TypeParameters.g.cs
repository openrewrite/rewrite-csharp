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
    public partial class TypeParameters(
    Guid id,
    Space prefix,
    Markers markers,
    IList<Annotation> annotations,
    IList<JRightPadded<TypeParameter>> parameters
    ) : J, J<TypeParameters>, MutableTree<TypeParameters>
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
            return v.VisitTypeParameters(this, p);
        }

        public Guid Id => id;

        public TypeParameters WithId(Guid newId)
        {
            return newId == id ? this : new TypeParameters(newId, prefix, markers, annotations, _parameters);
        }
        public Space Prefix => prefix;

        public TypeParameters WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new TypeParameters(id, newPrefix, markers, annotations, _parameters);
        }
        public Markers Markers => markers;

        public TypeParameters WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new TypeParameters(id, prefix, newMarkers, annotations, _parameters);
        }
        public IList<J.Annotation> Annotations => annotations;

        public TypeParameters WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new TypeParameters(id, prefix, markers, newAnnotations, _parameters);
        }
        private readonly IList<JRightPadded<J.TypeParameter>> _parameters = parameters;
        public IList<J.TypeParameter> Parameters => _parameters.Elements();

        public TypeParameters WithParameters(IList<J.TypeParameter> newParameters)
        {
            return Padding.WithParameters(_parameters.WithElements(newParameters));
        }
        public sealed record PaddingHelper(J.TypeParameters T)
        {
            public IList<JRightPadded<J.TypeParameter>> Parameters => T._parameters;

            public J.TypeParameters WithParameters(IList<JRightPadded<J.TypeParameter>> newParameters)
            {
                return T._parameters == newParameters ? T : new J.TypeParameters(T.Id, T.Prefix, T.Markers, T.Annotations, newParameters);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeParameters && other.Id == Id;
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