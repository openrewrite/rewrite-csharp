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
    public partial class NullSafeExpression(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression> expression
    ) : Cs, Expression, Expression<NullSafeExpression>, J<NullSafeExpression>, MutableTree<NullSafeExpression>
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
            return v.VisitNullSafeExpression(this, p);
        }

        public Guid Id => id;

        public NullSafeExpression WithId(Guid newId)
        {
            return newId == id ? this : new NullSafeExpression(newId, prefix, markers, _expression);
        }
        public Space Prefix => prefix;

        public NullSafeExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NullSafeExpression(id, newPrefix, markers, _expression);
        }
        public Markers Markers => markers;

        public NullSafeExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NullSafeExpression(id, prefix, newMarkers, _expression);
        }
        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public NullSafeExpression WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }
        public sealed record PaddingHelper(Cs.NullSafeExpression T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public Cs.NullSafeExpression WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.NullSafeExpression(T.Id, T.Prefix, T.Markers, newExpression);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NullSafeExpression && other.Id == Id;
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