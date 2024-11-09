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
    public partial class ClassDeclaration(
    Guid id,
    Space prefix,
    Markers markers,
    J.ClassDeclaration classDeclarationCore,
    JContainer<TypeParameterConstraintClause> typeParameterConstraintClauses
    ) : Cs, Statement, MutableTree<ClassDeclaration>
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
            return v.VisitClassDeclaration(this, p);
        }

        public Guid Id => id;

        public ClassDeclaration WithId(Guid newId)
        {
            return newId == id ? this : new ClassDeclaration(newId, prefix, markers, classDeclarationCore, _typeParameterConstraintClauses);
        }
        public Space Prefix => prefix;

        public ClassDeclaration WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ClassDeclaration(id, newPrefix, markers, classDeclarationCore, _typeParameterConstraintClauses);
        }
        public Markers Markers => markers;

        public ClassDeclaration WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ClassDeclaration(id, prefix, newMarkers, classDeclarationCore, _typeParameterConstraintClauses);
        }
        public J.ClassDeclaration ClassDeclarationCore => classDeclarationCore;

        public ClassDeclaration WithClassDeclarationCore(J.ClassDeclaration newClassDeclarationCore)
        {
            return ReferenceEquals(newClassDeclarationCore, classDeclarationCore) ? this : new ClassDeclaration(id, prefix, markers, newClassDeclarationCore, _typeParameterConstraintClauses);
        }
        private readonly JContainer<Cs.TypeParameterConstraintClause> _typeParameterConstraintClauses = typeParameterConstraintClauses;
        public IList<Cs.TypeParameterConstraintClause> TypeParameterConstraintClauses => _typeParameterConstraintClauses.GetElements();

        public ClassDeclaration WithTypeParameterConstraintClauses(IList<Cs.TypeParameterConstraintClause> newTypeParameterConstraintClauses)
        {
            return Padding.WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause>.WithElements(_typeParameterConstraintClauses, newTypeParameterConstraintClauses));
        }
        public sealed record PaddingHelper(Cs.ClassDeclaration T)
        {
            public JContainer<Cs.TypeParameterConstraintClause> TypeParameterConstraintClauses => T._typeParameterConstraintClauses;

            public Cs.ClassDeclaration WithTypeParameterConstraintClauses(JContainer<Cs.TypeParameterConstraintClause> newTypeParameterConstraintClauses)
            {
                return T._typeParameterConstraintClauses == newTypeParameterConstraintClauses ? T : new Cs.ClassDeclaration(T.Id, T.Prefix, T.Markers, T.ClassDeclarationCore, newTypeParameterConstraintClauses);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ClassDeclaration && other.Id == Id;
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