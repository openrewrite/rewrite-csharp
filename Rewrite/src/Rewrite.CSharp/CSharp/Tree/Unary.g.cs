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
    public partial class Unary(
    Guid id,
    Space prefix,
    Markers markers,
    JLeftPadded<Unary.Types> @operator,
    Expression expression,
    JavaType? type
    ) : Cs,Statement,Expression,TypedTree    {
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
            return v.VisitUnary(this, p);
        }

        public Guid Id { get;  set; } = id;

        public Unary WithId(Guid newId)
        {
            return newId == Id ? this : new Unary(newId, Prefix, Markers, _operator, Expression, Type);
        }
        public Space Prefix { get;  set; } = prefix;

        public Unary WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new Unary(Id, newPrefix, Markers, _operator, Expression, Type);
        }
        public Markers Markers { get;  set; } = markers;

        public Unary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new Unary(Id, Prefix, newMarkers, _operator, Expression, Type);
        }
        private JLeftPadded<Types> _operator = @operator;
        public Types Operator => _operator.Element;

        public Unary WithOperator(Types newOperator)
        {
            return Padding.WithOperator(_operator.WithElement(newOperator));
        }
        public Expression Expression { get;  set; } = expression;

        public Unary WithExpression(Expression newExpression)
        {
            return ReferenceEquals(newExpression, Expression) ? this : new Unary(Id, Prefix, Markers, _operator, newExpression, Type);
        }
        public JavaType? Type { get;  set; } = type;

        public Unary WithType(JavaType? newType)
        {
            return newType == Type ? this : new Unary(Id, Prefix, Markers, _operator, Expression, newType);
        }
        public enum Types
        {
            SuppressNullableWarning,
            PointerIndirection,
            PointerType,
            AddressOf,
            Spread,
            FromEnd,
        }
        public sealed record PaddingHelper(Cs.Unary T)
        {
            public JLeftPadded<Cs.Unary.Types> Operator { get => T._operator;  set => T._operator = value; }

            public Cs.Unary WithOperator(JLeftPadded<Cs.Unary.Types> newOperator)
            {
                return Operator == newOperator ? T : new Cs.Unary(T.Id, T.Prefix, T.Markers, newOperator, T.Expression, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Unary && other.Id == Id;
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