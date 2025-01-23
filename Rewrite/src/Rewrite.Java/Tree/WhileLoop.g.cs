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
    public partial class WhileLoop(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> condition,
    JRightPadded<Statement> body
    ) : J, Loop, J<WhileLoop>, MutableTree<WhileLoop>
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
            return v.VisitWhileLoop(this, p);
        }

        public Guid Id => id;

        public WhileLoop WithId(Guid newId)
        {
            return newId == id ? this : new WhileLoop(newId, prefix, markers, condition, _body);
        }
        public Space Prefix => prefix;

        public WhileLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new WhileLoop(id, newPrefix, markers, condition, _body);
        }
        public Markers Markers => markers;

        public WhileLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new WhileLoop(id, prefix, newMarkers, condition, _body);
        }
        public J.ControlParentheses<Expression> Condition => condition;

        public WhileLoop WithCondition(J.ControlParentheses<Expression> newCondition)
        {
            return ReferenceEquals(newCondition, condition) ? this : new WhileLoop(id, prefix, markers, newCondition, _body);
        }
        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public WhileLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }
        public sealed record PaddingHelper(J.WhileLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.WhileLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.WhileLoop(T.Id, T.Prefix, T.Markers, T.Condition, newBody);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is WhileLoop && other.Id == Id;
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