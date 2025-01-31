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
    public partial class Interpolation(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression> expression,
    JRightPadded<Expression>? alignment,
    JRightPadded<Expression>? format
    ) : Cs, Expression, Expression<Interpolation>, J<Interpolation>, MutableTree<Interpolation>
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
            return v.VisitInterpolation(this, p);
        }

        public Guid Id => id;

        public Interpolation WithId(Guid newId)
        {
            return newId == id ? this : new Interpolation(newId, prefix, markers, _expression, _alignment, _format);
        }
        public Space Prefix => prefix;

        public Interpolation WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Interpolation(id, newPrefix, markers, _expression, _alignment, _format);
        }
        public Markers Markers => markers;

        public Interpolation WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Interpolation(id, prefix, newMarkers, _expression, _alignment, _format);
        }
        private readonly JRightPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public Interpolation WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }
        private readonly JRightPadded<Expression>? _alignment = alignment;
        public Expression? Alignment => _alignment?.Element;

        public Interpolation WithAlignment(Expression? newAlignment)
        {
            return Padding.WithAlignment(JRightPadded<Expression>.WithElement(_alignment, newAlignment));
        }
        private readonly JRightPadded<Expression>? _format = format;
        public Expression? Format => _format?.Element;

        public Interpolation WithFormat(Expression? newFormat)
        {
            return Padding.WithFormat(JRightPadded<Expression>.WithElement(_format, newFormat));
        }
        public sealed record PaddingHelper(Cs.Interpolation T)
        {
            public JRightPadded<Expression> Expression => T._expression;

            public Cs.Interpolation WithExpression(JRightPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, newExpression, T._alignment, T._format);
            }

            public JRightPadded<Expression>? Alignment => T._alignment;

            public Cs.Interpolation WithAlignment(JRightPadded<Expression>? newAlignment)
            {
                return T._alignment == newAlignment ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, T._expression, newAlignment, T._format);
            }

            public JRightPadded<Expression>? Format => T._format;

            public Cs.Interpolation WithFormat(JRightPadded<Expression>? newFormat)
            {
                return T._format == newFormat ? T : new Cs.Interpolation(T.Id, T.Prefix, T.Markers, T._expression, T._alignment, newFormat);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Interpolation && other.Id == Id;
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