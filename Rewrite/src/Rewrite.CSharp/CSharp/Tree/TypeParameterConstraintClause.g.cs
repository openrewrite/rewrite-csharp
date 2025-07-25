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
    public partial class TypeParameterConstraintClause(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<J.Identifier> typeParameter,
    JContainer<TypeParameterConstraint> typeParameterConstraints
    ) : Cs    {
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
            return v.VisitTypeParameterConstraintClause(this, p);
        }

        public Guid Id { get;  set; } = id;

        public TypeParameterConstraintClause WithId(Guid newId)
        {
            return newId == Id ? this : new TypeParameterConstraintClause(newId, Prefix, Markers, _typeParameter, _typeParameterConstraints);
        }
        public Space Prefix { get;  set; } = prefix;

        public TypeParameterConstraintClause WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new TypeParameterConstraintClause(Id, newPrefix, Markers, _typeParameter, _typeParameterConstraints);
        }
        public Markers Markers { get;  set; } = markers;

        public TypeParameterConstraintClause WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new TypeParameterConstraintClause(Id, Prefix, newMarkers, _typeParameter, _typeParameterConstraints);
        }
        private JRightPadded<J.Identifier> _typeParameter = typeParameter;
        public J.Identifier TypeParameter => _typeParameter.Element;

        public TypeParameterConstraintClause WithTypeParameter(J.Identifier newTypeParameter)
        {
            return Padding.WithTypeParameter(_typeParameter.WithElement(newTypeParameter));
        }
        private JContainer<Cs.TypeParameterConstraint> _typeParameterConstraints = typeParameterConstraints;
        public IList<Cs.TypeParameterConstraint> TypeParameterConstraints => _typeParameterConstraints.GetElements();

        public TypeParameterConstraintClause WithTypeParameterConstraints(IList<Cs.TypeParameterConstraint> newTypeParameterConstraints)
        {
            return Padding.WithTypeParameterConstraints(JContainer<Cs.TypeParameterConstraint>.WithElements(_typeParameterConstraints, newTypeParameterConstraints));
        }
        public sealed record PaddingHelper(Cs.TypeParameterConstraintClause T)
        {
            public JRightPadded<J.Identifier> TypeParameter { get => T._typeParameter;  set => T._typeParameter = value; }

            public Cs.TypeParameterConstraintClause WithTypeParameter(JRightPadded<J.Identifier> newTypeParameter)
            {
                return TypeParameter == newTypeParameter ? T : new Cs.TypeParameterConstraintClause(T.Id, T.Prefix, T.Markers, newTypeParameter, T._typeParameterConstraints);
            }

            public JContainer<Cs.TypeParameterConstraint> TypeParameterConstraints { get => T._typeParameterConstraints;  set => T._typeParameterConstraints = value; }

            public Cs.TypeParameterConstraintClause WithTypeParameterConstraints(JContainer<Cs.TypeParameterConstraint> newTypeParameterConstraints)
            {
                return TypeParameterConstraints == newTypeParameterConstraints ? T : new Cs.TypeParameterConstraintClause(T.Id, T.Prefix, T.Markers, T._typeParameter, newTypeParameterConstraints);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TypeParameterConstraintClause && other.Id == Id;
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