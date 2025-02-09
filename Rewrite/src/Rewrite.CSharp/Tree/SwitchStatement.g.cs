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
    /// Represents a C# switch statement for control flow based on pattern matching and case labels.
    /// <br/>
    /// For example:
    /// <code>
    /// switch(value) {
    ///     case 1:
    ///         Console.WriteLine("one");
    ///         break;
    ///     case int n when n &gt; 0:
    ///         Console.WriteLine("positive");
    ///         break;
    ///     case Person { Age: &gt; 18 }:
    ///         Console.WriteLine("adult");
    ///         break;
    ///     case string s:
    ///         Console.WriteLine($"string: {s}");
    ///         break;
    ///     default:
    ///         Console.WriteLine("default");
    ///         break;
    /// }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class SwitchStatement(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Expression> expression,
    JContainer<SwitchSection> sections
    ) : Cs, Statement, J<SwitchStatement>, MutableTree<SwitchStatement>
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
            return v.VisitSwitchStatement(this, p);
        }

        public Guid Id => id;

        public SwitchStatement WithId(Guid newId)
        {
            return newId == id ? this : new SwitchStatement(newId, prefix, markers, _expression, _sections);
        }
        public Space Prefix => prefix;

        public SwitchStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new SwitchStatement(id, newPrefix, markers, _expression, _sections);
        }
        public Markers Markers => markers;

        public SwitchStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new SwitchStatement(id, prefix, newMarkers, _expression, _sections);
        }
        private readonly JContainer<Expression> _expression = expression;
        public IList<Expression> Expression => _expression.GetElements();

        public SwitchStatement WithExpression(IList<Expression> newExpression)
        {
            return Padding.WithExpression(JContainer<Expression>.WithElements(_expression, newExpression));
        }
        private readonly JContainer<Cs.SwitchSection> _sections = sections;
        public IList<Cs.SwitchSection> Sections => _sections.GetElements();

        public SwitchStatement WithSections(IList<Cs.SwitchSection> newSections)
        {
            return Padding.WithSections(JContainer<Cs.SwitchSection>.WithElements(_sections, newSections));
        }
        public sealed record PaddingHelper(Cs.SwitchStatement T)
        {
            public JContainer<Expression> Expression => T._expression;

            public Cs.SwitchStatement WithExpression(JContainer<Expression> newExpression)
            {
                return T._expression == newExpression ? T : new Cs.SwitchStatement(T.Id, T.Prefix, T.Markers, newExpression, T._sections);
            }

            public JContainer<Cs.SwitchSection> Sections => T._sections;

            public Cs.SwitchStatement WithSections(JContainer<Cs.SwitchSection> newSections)
            {
                return T._sections == newSections ? T : new Cs.SwitchStatement(T.Id, T.Prefix, T.Markers, T._expression, newSections);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchStatement && other.Id == Id;
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