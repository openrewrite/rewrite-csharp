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
    /// Represents a single case arm in a switch expression, consisting of a pattern, optional when clause, and result expression.
    /// <br/>
    /// For example:
    /// <code>
    /// case &lt; 0 when IsValid() =&gt; "negative",
    /// &gt; 0 =&gt; "positive",
    /// _ =&gt; "zero"
    /// // With complex patterns and conditions
    /// (age, role) switch {
    ///     ( &gt; 21, "admin") when HasPermission() =&gt; "full access",
    ///     ( &gt; 18, _) =&gt; "basic access",
    ///     _ =&gt; "no access"
    /// }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class SwitchExpressionArm(
    Guid id,
    Space prefix,
    Markers markers,
    Pattern pattern,
    JLeftPadded<Expression>? whenExpression,
    JLeftPadded<Expression> expression
    ) : Cs, J<SwitchExpressionArm>, MutableTree<SwitchExpressionArm>
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
            return v.VisitSwitchExpressionArm(this, p);
        }

        public Guid Id { get;  set; } = id;

        public SwitchExpressionArm WithId(Guid newId)
        {
            return newId == Id ? this : new SwitchExpressionArm(newId, Prefix, Markers, Pattern, _whenExpression, _expression);
        }
        public Space Prefix { get;  set; } = prefix;

        public SwitchExpressionArm WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new SwitchExpressionArm(Id, newPrefix, Markers, Pattern, _whenExpression, _expression);
        }
        public Markers Markers { get;  set; } = markers;

        public SwitchExpressionArm WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new SwitchExpressionArm(Id, Prefix, newMarkers, Pattern, _whenExpression, _expression);
        }
        public Cs.Pattern Pattern { get;  set; } = pattern;

        public SwitchExpressionArm WithPattern(Cs.Pattern newPattern)
        {
            return ReferenceEquals(newPattern, Pattern) ? this : new SwitchExpressionArm(Id, Prefix, Markers, newPattern, _whenExpression, _expression);
        }
        private JLeftPadded<Expression>? _whenExpression = whenExpression;
        public Expression? WhenExpression => _whenExpression?.Element;

        public SwitchExpressionArm WithWhenExpression(Expression? newWhenExpression)
        {
            return Padding.WithWhenExpression(JLeftPadded<Expression>.WithElement(_whenExpression, newWhenExpression));
        }
        private JLeftPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public SwitchExpressionArm WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }
        public sealed record PaddingHelper(Cs.SwitchExpressionArm T)
        {
            public JLeftPadded<Expression>? WhenExpression { get => T._whenExpression;  set => T._whenExpression = value; }

            public Cs.SwitchExpressionArm WithWhenExpression(JLeftPadded<Expression>? newWhenExpression)
            {
                return WhenExpression == newWhenExpression ? T : new Cs.SwitchExpressionArm(T.Id, T.Prefix, T.Markers, T.Pattern, newWhenExpression, T._expression);
            }

            public JLeftPadded<Expression> Expression { get => T._expression;  set => T._expression = value; }

            public Cs.SwitchExpressionArm WithExpression(JLeftPadded<Expression> newExpression)
            {
                return Expression == newExpression ? T : new Cs.SwitchExpressionArm(T.Id, T.Prefix, T.Markers, T.Pattern, T._whenExpression, newExpression);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchExpressionArm && other.Id == Id;
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