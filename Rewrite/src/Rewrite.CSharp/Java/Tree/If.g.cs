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
    /// <summary>
    /// Represents a Java if statement.
    /// <br/>Example:
    /// <code>{@code
    /// if (condition) {
    ///     // then
    /// } else {
    ///     // else
    /// }
    /// }</code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class If(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> ifCondition,
    JRightPadded<Statement> thenPart,
    If.Else? elsePart
    ) : J,Statement    {
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
            return v.VisitIf(this, p);
        }

        public Guid Id { get;  set; } = id;

        public If WithId(Guid newId)
        {
            return newId == Id ? this : new If(newId, Prefix, Markers, IfCondition, _thenPart, ElsePart);
        }
        public Space Prefix { get;  set; } = prefix;

        public If WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new If(Id, newPrefix, Markers, IfCondition, _thenPart, ElsePart);
        }
        public Markers Markers { get;  set; } = markers;

        public If WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new If(Id, Prefix, newMarkers, IfCondition, _thenPart, ElsePart);
        }
        public J.ControlParentheses<Expression> IfCondition { get;  set; } = ifCondition;

        public If WithIfCondition(J.ControlParentheses<Expression> newIfCondition)
        {
            return ReferenceEquals(newIfCondition, IfCondition) ? this : new If(Id, Prefix, Markers, newIfCondition, _thenPart, ElsePart);
        }
        private JRightPadded<Statement> _thenPart = thenPart;
        public Statement ThenPart => _thenPart.Element;

        public If WithThenPart(Statement newThenPart)
        {
            return Padding.WithThenPart(_thenPart.WithElement(newThenPart));
        }
        public Else? ElsePart { get;  set; } = elsePart;

        public If WithElsePart(Else? newElsePart)
        {
            return ReferenceEquals(newElsePart, ElsePart) ? this : new If(Id, Prefix, Markers, IfCondition, _thenPart, newElsePart);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public partial class Else(
    Guid id,
    Space prefix,
    Markers markers,
    JRightPadded<Statement> body
        ) : J, MutableTree
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
                return v.VisitElse(this, p);
            }

            public Guid Id { get;  set; } = id;

            public Else WithId(Guid newId)
            {
                return newId == Id ? this : new Else(newId, Prefix, Markers, _body);
            }
            public Space Prefix { get;  set; } = prefix;

            public Else WithPrefix(Space newPrefix)
            {
                return newPrefix == Prefix ? this : new Else(Id, newPrefix, Markers, _body);
            }
            public Markers Markers { get;  set; } = markers;

            public Else WithMarkers(Markers newMarkers)
            {
                return ReferenceEquals(newMarkers, Markers) ? this : new Else(Id, Prefix, newMarkers, _body);
            }
            private JRightPadded<Statement> _body = body;
            public Statement Body => _body.Element;

            public Else WithBody(Statement newBody)
            {
                return Padding.WithBody(_body.WithElement(newBody));
            }
            public sealed record PaddingHelper(J.If.Else T)
            {
                public JRightPadded<Statement> Body { get => T._body;  set => T._body = value; }

                public J.If.Else WithBody(JRightPadded<Statement> newBody)
                {
                    return Body == newBody ? T : new J.If.Else(T.Id, T.Prefix, T.Markers, newBody);
                }

            }

            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public bool Equals(Rewrite.Core.Tree? other)
            {
                return other is Else && other.Id == Id;
            }
            #if DEBUG_VISITOR
            [DebuggerStepThrough]
            #endif
            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }
        public sealed record PaddingHelper(J.If T)
        {
            public JRightPadded<Statement> ThenPart { get => T._thenPart;  set => T._thenPart = value; }

            public J.If WithThenPart(JRightPadded<Statement> newThenPart)
            {
                return ThenPart == newThenPart ? T : new J.If(T.Id, T.Prefix, T.Markers, T.IfCondition, newThenPart, T.ElsePart);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is If && other.Id == Id;
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