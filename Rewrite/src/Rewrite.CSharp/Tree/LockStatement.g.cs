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
    /// Represents a C# lock statement which provides thread synchronization.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple lock statement
    ///     lock (syncObject) {
    ///         // protected code
    ///     }
    ///     // Lock with local variable
    ///     lock (this.lockObj) {
    ///         sharedResource.Modify();
    ///     }
    ///     // Lock with property
    ///     lock (SyncRoot) {
    ///         // thread-safe operations
    ///     }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class LockStatement(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<Expression> expression,
    JRightPadded<Statement> statement
    ) : Cs, Statement, J<LockStatement>, MutableTree<LockStatement>
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
            return v.VisitLockStatement(this, p);
        }

        public Guid Id { get;  set; } = id;

        public LockStatement WithId(Guid newId)
        {
            return newId == Id ? this : new LockStatement(newId, Prefix, Markers, Expression, _statement);
        }
        public Space Prefix { get;  set; } = prefix;

        public LockStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new LockStatement(Id, newPrefix, Markers, Expression, _statement);
        }
        public Markers Markers { get;  set; } = markers;

        public LockStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new LockStatement(Id, Prefix, newMarkers, Expression, _statement);
        }
        public J.ControlParentheses<Expression> Expression { get;  set; } = expression;

        public LockStatement WithExpression(J.ControlParentheses<Expression> newExpression)
        {
            return ReferenceEquals(newExpression, Expression) ? this : new LockStatement(Id, Prefix, Markers, newExpression, _statement);
        }
        private JRightPadded<Statement> _statement = statement;
        public Statement Statement => _statement.Element;

        public LockStatement WithStatement(Statement newStatement)
        {
            return Padding.WithStatement(_statement.WithElement(newStatement));
        }
        public sealed record PaddingHelper(Cs.LockStatement T)
        {
            public JRightPadded<Statement> Statement { get => T._statement;  set => T._statement = value; }

            public Cs.LockStatement WithStatement(JRightPadded<Statement> newStatement)
            {
                return Statement == newStatement ? T : new Cs.LockStatement(T.Id, T.Prefix, T.Markers, T.Expression, newStatement);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is LockStatement && other.Id == Id;
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