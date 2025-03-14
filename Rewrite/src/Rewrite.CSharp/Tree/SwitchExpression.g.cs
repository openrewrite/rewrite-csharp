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
    /// <summary>
    /// Represents a C# switch expression which provides a concise way to handle multiple patterns with corresponding expressions.
    /// <br/>
    /// For example:
    /// <code>
    /// var description = size switch {
    ///     &lt; 0 =&gt; "negative",
    ///     0 =&gt; "zero",
    ///     &gt; 0 =&gt; "positive"
    /// };
    /// var color = (r, g, b) switch {
    ///     var (r, g, b) when r == g && g == b =&gt; "grayscale",
    ///     ( &gt; 128, _, _) =&gt; "bright red",
    ///     _ =&gt; "other"
    /// };
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class SwitchExpression(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression> expression,
    JContainer<SwitchExpressionArm> arms
    ) : Cs, Expression, Expression<SwitchExpression>, J<SwitchExpression>, MutableTree<SwitchExpression>
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
            return v.VisitSwitchExpression(this, p);
        }

        public Guid Id => id;

        public SwitchExpression WithId(Guid newId)
        {
            return newId == id ? this : new SwitchExpression(newId, prefix, markers, _expression, _arms);
        }
        public Space Prefix => prefix;

        public SwitchExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new SwitchExpression(id, newPrefix, markers, _expression, _arms);
        }
        public Markers Markers => markers;

        public SwitchExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new SwitchExpression(id, prefix, newMarkers, _expression, _arms);
        }
        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public SwitchExpression WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }
        private readonly JContainer<Cs.SwitchExpressionArm> _arms = arms;
        public IList<Cs.SwitchExpressionArm> Arms => _arms.GetElements();

        public SwitchExpression WithArms(IList<Cs.SwitchExpressionArm> newArms)
        {
            return Padding.WithArms(JContainer<Cs.SwitchExpressionArm>.WithElements(_arms, newArms));
        }
        public sealed record PaddingHelper(Cs.SwitchExpression T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public Cs.SwitchExpression WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.SwitchExpression(T.Id, T.Prefix, T.Markers, newExpression, T._arms);
            }

            public JContainer<Cs.SwitchExpressionArm> Arms => T._arms;

            public Cs.SwitchExpression WithArms(JContainer<Cs.SwitchExpressionArm> newArms)
            {
                return T._arms == newArms ? T : new Cs.SwitchExpression(T.Id, T.Prefix, T.Markers, T._expression, newArms);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchExpression && other.Id == Id;
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