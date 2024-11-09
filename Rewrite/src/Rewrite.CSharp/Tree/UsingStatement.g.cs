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
    public partial class UsingStatement(
    Guid id,
    Space prefix,
    Markers markers,
    Keyword? awaitKeyword,
    JContainer<Expression> expression,
    Statement statement
    ) : Cs, Statement, MutableTree<UsingStatement>
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
            return v.VisitUsingStatement(this, p);
        }

        public Guid Id => id;

        public UsingStatement WithId(Guid newId)
        {
            return newId == id ? this : new UsingStatement(newId, prefix, markers, awaitKeyword, _expression, statement);
        }
        public Space Prefix => prefix;

        public UsingStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new UsingStatement(id, newPrefix, markers, awaitKeyword, _expression, statement);
        }
        public Markers Markers => markers;

        public UsingStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new UsingStatement(id, prefix, newMarkers, awaitKeyword, _expression, statement);
        }
        public Cs.Keyword? AwaitKeyword => awaitKeyword;

        public UsingStatement WithAwaitKeyword(Cs.Keyword? newAwaitKeyword)
        {
            return ReferenceEquals(newAwaitKeyword, awaitKeyword) ? this : new UsingStatement(id, prefix, markers, newAwaitKeyword, _expression, statement);
        }
        private readonly JContainer<Expression> _expression = expression;
        public IList<Expression> Expression => _expression.GetElements();

        public UsingStatement WithExpression(IList<Expression> newExpression)
        {
            return Padding.WithExpression(JContainer<Expression>.WithElements(_expression, newExpression));
        }
        public Statement Statement => statement;

        public UsingStatement WithStatement(Statement newStatement)
        {
            return ReferenceEquals(newStatement, statement) ? this : new UsingStatement(id, prefix, markers, awaitKeyword, _expression, newStatement);
        }
        public sealed record PaddingHelper(Cs.UsingStatement T)
        {
            public JContainer<Expression> Expression => T._expression;

            public Cs.UsingStatement WithExpression(JContainer<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.UsingStatement(T.Id, T.Prefix, T.Markers, T.AwaitKeyword, newExpression, T.Statement);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is UsingStatement && other.Id == Id;
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