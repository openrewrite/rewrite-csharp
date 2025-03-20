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
    public partial class Binary(
    Guid id,
    Space prefix,
    Markers markers,
    Expression left,
    JLeftPadded<Binary.OperatorType> @operator,
    Expression right,
    JavaType? type
    ) : Cs, Expression, TypedTree, Expression<Binary>, TypedTree<Binary>, J<Binary>, MutableTree<Binary>
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
            return v.VisitBinary(this, p);
        }

        public Guid Id { get;  set; } = id;

        public Binary WithId(Guid newId)
        {
            return newId == Id ? this : new Binary(newId, Prefix, Markers, Left, _operator, Right, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public Binary WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new Binary(Id, newPrefix, Markers, Left, _operator, Right, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public Binary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new Binary(Id, Prefix, newMarkers, Left, _operator, Right, Type);
        }
        public Expression Left { get;  set; } = left;

        public Binary WithLeft(Expression newLeft)
        {
            return ReferenceEquals(newLeft, Left) ? this : new Binary(Id, Prefix, Markers, newLeft, _operator, Right, Type);
        }
        private JLeftPadded<OperatorType> _operator = @operator;
        public OperatorType Operator => _operator.Element;

        public Binary WithOperator(OperatorType newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }
        public Expression Right { get;  set; } = right;

        public Binary WithRight(Expression newRight)
        {
            return ReferenceEquals(newRight, Right) ? this : new Binary(Id, Prefix, Markers, Left, _operator, newRight, Type);
        }
        public JavaType? Type { get;  set; } = type;

        public Binary WithType(JavaType? newType)
        {
            return newType == Type ? this : new Binary(Id, Prefix, Markers, Left, _operator, Right, newType);
        }
        public enum OperatorType
        {
            As,
            NullCoalescing,
        }
        public sealed record PaddingHelper(Cs.Binary T)
        {
            public JLeftPadded<Cs.Binary.OperatorType> Operator { get => T._operator;  set => T._operator = value; }

            public Cs.Binary WithOperator(JLeftPadded<Cs.Binary.OperatorType> newOperator)
            {
                return Operator == newOperator ? T : new Cs.Binary(T.Id, T.Prefix, T.Markers, T.Left, newOperator, T.Right, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Binary && other.Id == Id;
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