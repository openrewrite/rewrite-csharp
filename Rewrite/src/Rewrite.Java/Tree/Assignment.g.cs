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
    public partial class Assignment(
    Guid id,
    Space prefix,
    Markers markers,
    Expression variable,
    JLeftPadded<Expression> expression,
    JavaType? type
    ) : J, Statement, Expression, TypedTree, MutableTree<Assignment>
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
            return v.VisitAssignment(this, p);
        }

        public Guid Id => id;

        public Assignment WithId(Guid newId)
        {
            return newId == id ? this : new Assignment(newId, prefix, markers, variable, _expression, type);
        }
        public Space Prefix => prefix;

        public Assignment WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Assignment(id, newPrefix, markers, variable, _expression, type);
        }
        public Markers Markers => markers;

        public Assignment WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Assignment(id, prefix, newMarkers, variable, _expression, type);
        }
        public Expression Variable => variable;

        public Assignment WithVariable(Expression newVariable)
        {
            return ReferenceEquals(newVariable, variable) ? this : new Assignment(id, prefix, markers, newVariable, _expression, type);
        }
        private readonly JLeftPadded<Expression> _expression = expression;
        public Expression Expression => _expression.Element;

        public Assignment WithExpression(Expression newExpression)
        {
            return Padding.WithExpression(_expression.WithElement(newExpression));
        }
        public JavaType? Type => type;

        public Assignment WithType(JavaType? newType)
        {
            return newType == type ? this : new Assignment(id, prefix, markers, variable, _expression, newType);
        }
        public sealed record PaddingHelper(J.Assignment T)
        {
            public JLeftPadded<Expression> Expression => T._expression;

            public J.Assignment WithExpression(JLeftPadded<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new J.Assignment(T.Id, T.Prefix, T.Markers, T.Variable, newExpression, T.Type);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Assignment && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}