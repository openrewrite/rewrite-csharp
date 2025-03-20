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
    public partial class DoWhileLoop(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Statement> body,
    JLeftPadded<J.ControlParentheses<Expression>> whileCondition
    ) : J, Loop, J<DoWhileLoop>, MutableTree<DoWhileLoop>
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
            return v.VisitDoWhileLoop(this, p);
        }

        public Guid Id { get;  set; } = id;

        public DoWhileLoop WithId(Guid newId)
        {
            return newId == Id ? this : new DoWhileLoop(newId, Prefix, Markers, _body, _whileCondition);
        }
        public Space Prefix { get;  set; } = prefix;

        public DoWhileLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new DoWhileLoop(Id, newPrefix, Markers, _body, _whileCondition);
        }
        public Markers Markers { get;  set; } = markers;

        public DoWhileLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new DoWhileLoop(Id, Prefix, newMarkers, _body, _whileCondition);
        }
        private JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public DoWhileLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }
        private JLeftPadded<J.ControlParentheses<Expression>> _whileCondition = whileCondition;
        public J.ControlParentheses<Expression> WhileCondition => _whileCondition.Element;

        public DoWhileLoop WithWhileCondition(J.ControlParentheses<Expression> newWhileCondition)
        {
            return Padding.WithWhileCondition(_whileCondition.WithElement(newWhileCondition));
        }
        public sealed record PaddingHelper(J.DoWhileLoop T)
        {
            public JRightPadded<Statement> Body { get => T._body;  set => T._body = value; }

            public J.DoWhileLoop WithBody(JRightPadded<Statement> newBody)
            {
                return Body == newBody ? T : new J.DoWhileLoop(T.Id, T.Prefix, T.Markers, newBody, T._whileCondition);
            }

            public JLeftPadded<J.ControlParentheses<Expression>> WhileCondition { get => T._whileCondition;  set => T._whileCondition = value; }

            public J.DoWhileLoop WithWhileCondition(JLeftPadded<J.ControlParentheses<Expression>> newWhileCondition)
            {
                return WhileCondition == newWhileCondition ? T : new J.DoWhileLoop(T.Id, T.Prefix, T.Markers, T._body, newWhileCondition);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DoWhileLoop && other.Id == Id;
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