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
    public partial class AllowsConstraintClause(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<AllowsConstraint> expressions
    ) : Cs, Cs.TypeParameterConstraint, MutableTree<AllowsConstraintClause>
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
            return v.VisitAllowsConstraintClause(this, p);
        }

        public Guid Id => id;

        public AllowsConstraintClause WithId(Guid newId)
        {
            return newId == id ? this : new AllowsConstraintClause(newId, prefix, markers, _expressions);
        }
        public Space Prefix => prefix;

        public AllowsConstraintClause WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new AllowsConstraintClause(id, newPrefix, markers, _expressions);
        }
        public Markers Markers => markers;

        public AllowsConstraintClause WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new AllowsConstraintClause(id, prefix, newMarkers, _expressions);
        }
        private readonly JContainer<Cs.AllowsConstraint> _expressions = expressions;
        public IList<Cs.AllowsConstraint> Expressions => _expressions.GetElements();

        public AllowsConstraintClause WithExpressions(IList<Cs.AllowsConstraint> newExpressions)
        {
            return Padding.WithExpressions(JContainer<Cs.AllowsConstraint>.WithElements(_expressions, newExpressions));
        }
        public sealed record PaddingHelper(Cs.AllowsConstraintClause T)
        {
            public JContainer<Cs.AllowsConstraint> Expressions => T._expressions;

            public Cs.AllowsConstraintClause WithExpressions(JContainer<Cs.AllowsConstraint> newExpressions)
            {
                return T._expressions == newExpressions ? T : new Cs.AllowsConstraintClause(T.Id, T.Prefix, T.Markers, newExpressions);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is AllowsConstraintClause && other.Id == Id;
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