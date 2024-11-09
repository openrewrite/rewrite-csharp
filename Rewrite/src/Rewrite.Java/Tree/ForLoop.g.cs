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
    public partial class ForLoop(
    Guid id,
    Space prefix,
    Markers markers,
    ForLoop.Control loopControl,
    JRightPadded<Statement> body
    ) : J, Loop, MutableTree<ForLoop>
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
            return v.VisitForLoop(this, p);
        }

        public Guid Id => id;

        public ForLoop WithId(Guid newId)
        {
            return newId == id ? this : new ForLoop(newId, prefix, markers, loopControl, _body);
        }
        public Space Prefix => prefix;

        public ForLoop WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ForLoop(id, newPrefix, markers, loopControl, _body);
        }
        public Markers Markers => markers;

        public ForLoop WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ForLoop(id, prefix, newMarkers, loopControl, _body);
        }
        public Control LoopControl => loopControl;

        public ForLoop WithLoopControl(Control newLoopControl)
        {
            return ReferenceEquals(newLoopControl, loopControl) ? this : new ForLoop(id, prefix, markers, newLoopControl, _body);
        }
        private readonly JRightPadded<Statement> _body = body;
        public Statement Body => _body.Element;

        public ForLoop WithBody(Statement newBody)
        {
            return Padding.WithBody(_body.WithElement(newBody));
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Control(
    Guid id,
    Space prefix,
    Markers markers,
    IList<JRightPadded<Statement>> init,
    JRightPadded<Expression> condition,
    IList<JRightPadded<Statement>> update
        ) : J, MutableTree<Control>
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
                return v.VisitForControl(this, p);
            }

            public Guid Id => id;

            public Control WithId(Guid newId)
            {
                return newId == id ? this : new Control(newId, prefix, markers, _init, _condition, _update);
            }
            public Space Prefix => prefix;

            public Control WithPrefix(Space newPrefix)
            {
                return newPrefix == prefix ? this : new Control(id, newPrefix, markers, _init, _condition, _update);
            }
            public Markers Markers => markers;

            public Control WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, markers) ? this : new Control(id, prefix, newMarkers, _init, _condition, _update);
            }
            private readonly IList<JRightPadded<Statement>> _init = init;
            public IList<Statement> Init => _init.Elements();

            public Control WithInit(IList<Statement> newInit)
            {
                return Padding.WithInit(_init.WithElements(newInit));
            }
            private readonly JRightPadded<Expression> _condition = condition;
            public Expression Condition => _condition.Element;

            public Control WithCondition(Expression newCondition)
            {
                return Padding.WithCondition(_condition.WithElement(newCondition));
            }
            private readonly IList<JRightPadded<Statement>> _update = update;
            public IList<Statement> Update => _update.Elements();

            public Control WithUpdate(IList<Statement> newUpdate)
            {
                return Padding.WithUpdate(_update.WithElements(newUpdate));
            }
            public sealed record PaddingHelper(J.ForLoop.Control T)
            {
                public IList<JRightPadded<Statement>> Init => T._init;

                public J.ForLoop.Control WithInit(IList<JRightPadded<Statement>> newInit)
                {
                    return T._init == newInit ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, newInit, T._condition, T._update);
                }

                public JRightPadded<Expression> Condition => T._condition;

                public J.ForLoop.Control WithCondition(JRightPadded<Expression> newCondition)
                {
                    return T._condition == newCondition ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, T._init, newCondition, T._update);
                }

                public IList<JRightPadded<Statement>> Update => T._update;

                public J.ForLoop.Control WithUpdate(IList<JRightPadded<Statement>> newUpdate)
                {
                    return T._update == newUpdate ? T : new J.ForLoop.Control(T.Id, T.Prefix, T.Markers, T._init, T._condition, newUpdate);
                }

            }

            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Control && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public sealed record PaddingHelper(J.ForLoop T)
        {
            public JRightPadded<Statement> Body => T._body;

            public J.ForLoop WithBody(JRightPadded<Statement> newBody)
            {
                return T._body == newBody ? T : new J.ForLoop(T.Id, T.Prefix, T.Markers, T.LoopControl, newBody);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ForLoop && other.Id == Id;
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