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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class Ternary(
    Guid id,
    Space prefix,
    Markers markers,
    Expression condition,
    JLeftPadded<Expression> truePart,
    JLeftPadded<Expression> falsePart,
    JavaType? type
    ) : J, Expression, Statement, TypedTree, Expression<Ternary>, TypedTree<Ternary>, MutableTree<Ternary>
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

        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitTernary(this, p);
        }

        public Guid Id => id;

        public Ternary WithId(Guid newId)
        {
            return newId == id ? this : new Ternary(newId, prefix, markers, condition, _truePart, _falsePart, type);
        }
        public Space Prefix => prefix;

        public Ternary WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Ternary(id, newPrefix, markers, condition, _truePart, _falsePart, type);
        }
        public Markers Markers => markers;

        public Ternary WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Ternary(id, prefix, newMarkers, condition, _truePart, _falsePart, type);
        }
        public Expression Condition => condition;

        public Ternary WithCondition(Expression newCondition)
        {
            return ReferenceEquals(newCondition, condition) ? this : new Ternary(id, prefix, markers, newCondition, _truePart, _falsePart, type);
        }
        private readonly JLeftPadded<Expression> _truePart = truePart;
        public Expression TruePart => _truePart.Element;

        public Ternary WithTruePart(Expression newTruePart)
        {
            return Padding.WithTruePart(_truePart.WithElement(newTruePart));
        }
        private readonly JLeftPadded<Expression> _falsePart = falsePart;
        public Expression FalsePart => _falsePart.Element;

        public Ternary WithFalsePart(Expression newFalsePart)
        {
            return Padding.WithFalsePart(_falsePart.WithElement(newFalsePart));
        }
        public JavaType? Type => type;

        public Ternary WithType(JavaType? newType)
        {
            return newType == type ? this : new Ternary(id, prefix, markers, condition, _truePart, _falsePart, newType);
        }
        public sealed record PaddingHelper(J.Ternary T)
        {
            public JLeftPadded<Expression> TruePart => T._truePart;

            public J.Ternary WithTruePart(JLeftPadded<Expression> newTruePart)
            {
                return T._truePart == newTruePart ? T : new J.Ternary(T.Id, T.Prefix, T.Markers, T.Condition, newTruePart, T._falsePart, T.Type);
            }

            public JLeftPadded<Expression> FalsePart => T._falsePart;

            public J.Ternary WithFalsePart(JLeftPadded<Expression> newFalsePart)
            {
                return T._falsePart == newFalsePart ? T : new J.Ternary(T.Id, T.Prefix, T.Markers, T.Condition, T._truePart, newFalsePart, T.Type);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Ternary && other.Id == Id;
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