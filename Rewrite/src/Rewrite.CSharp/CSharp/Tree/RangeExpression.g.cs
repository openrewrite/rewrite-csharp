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
    /// Represents a C# range expression which creates a Range value representing a sequence of indices.
    /// Range expressions use the '..' operator to specify start and end bounds, and can use '^' to specify
    /// indices from the end.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Full range
    ///     arr[..]
    ///     // Range with start index
    ///     arr[2..]
    ///     // Range with end index
    ///     arr[..5]
    ///     // Range with both indices
    ///     arr[2..5]
    ///     // Range with end-relative indices using '^'
    ///     arr[..^1]     // excludes last element
    ///     arr[1..^1]    // from index 1 to last-1
    ///     arr[^2..^1]   // second-to-last to last-but-one
    ///     // Standalone range expressions
    ///     Range r1 = 1..4;
    ///     Range r2 = ..^1;
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class RangeExpression(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Expression>? start,
    Expression? end
    ) : Cs,Expression    {
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
            return v.VisitRangeExpression(this, p);
        }

        public Guid Id { get;  set; } = id;

        public RangeExpression WithId(Guid newId)
        {
            return newId == Id ? this : new RangeExpression(newId, Prefix, Markers, _start, End);
        }
        public Space Prefix { get;  set; } = prefix;

        public RangeExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new RangeExpression(Id, newPrefix, Markers, _start, End);
        }
        public Markers Markers { get;  set; } = markers;

        public RangeExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new RangeExpression(Id, Prefix, newMarkers, _start, End);
        }
        private JRightPadded<Expression>? _start = start;
        public Expression? Start => _start?.Element;

        public RangeExpression WithStart(Expression? newStart)
        {
            return Padding.WithStart(JRightPadded<Expression>.WithElement(_start, newStart));
        }
        public Expression? End { get;  set; } = end;

        public RangeExpression WithEnd(Expression? newEnd)
        {
            return ReferenceEquals(newEnd, End) ? this : new RangeExpression(Id, Prefix, Markers, _start, newEnd);
        }
        public sealed record PaddingHelper(Cs.RangeExpression T)
        {
            public JRightPadded<Expression>? Start { get => T._start;  set => T._start = value; }

            public Cs.RangeExpression WithStart(JRightPadded<Expression>? newStart)
            {
                return Start == newStart ? T : new Cs.RangeExpression(T.Id, T.Prefix, T.Markers, newStart, T.End);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is RangeExpression && other.Id == Id;
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