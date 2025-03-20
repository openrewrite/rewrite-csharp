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
    /// Represents an initializer expression that consists of a list of expressions, typically used in array
    /// or collection initialization contexts. The expressions are contained within delimiters like curly braces.
    /// <br/>
    /// For example:
    /// <code>
    /// new int[] { 1, 2, 3 }
    ///            ^^^^^^^^^
    /// new List<string> { "a", "b", "c" }
    ///                   ^^^^^^^^^^^^^^^
    /// </code>
    /// The JContainer wrapper captures whitespace before the opening brace, while also preserving whitespace
    /// after each expression (before commas) through its internal JRightPadded elements.
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class InitializerExpression(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Expression> expressions
    ) : Cs, Expression, Expression<InitializerExpression>, J<InitializerExpression>, MutableTree<InitializerExpression>
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
            return v.VisitInitializerExpression(this, p);
        }

        public Guid Id { get;  set; } = id;

        public InitializerExpression WithId(Guid newId)
        {
            return newId == Id ? this : new InitializerExpression(newId, Prefix, Markers, _expressions);
        }
        public Space Prefix { get;  set; } = prefix;

        public InitializerExpression WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new InitializerExpression(Id, newPrefix, Markers, _expressions);
        }
        public Markers Markers { get;  set; } = markers;

        public InitializerExpression WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new InitializerExpression(Id, Prefix, newMarkers, _expressions);
        }
        private JContainer<Expression> _expressions = expressions;
        public IList<Expression> Expressions => _expressions.GetElements();

        public InitializerExpression WithExpressions(IList<Expression> newExpressions)
        {
            return Padding.WithExpressions(JContainer<Expression>.WithElements(_expressions, newExpressions));
        }
        public sealed record PaddingHelper(Cs.InitializerExpression T)
        {
            public JContainer<Expression> Expressions { get => T._expressions;  set => T._expressions = value; }

            public Cs.InitializerExpression WithExpressions(JContainer<Expression> newExpressions)
            {
                return Expressions == newExpressions ? T : new Cs.InitializerExpression(T.Id, T.Prefix, T.Markers, newExpressions);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is InitializerExpression && other.Id == Id;
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