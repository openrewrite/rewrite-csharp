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
    /// Represents a C# parenthesized pattern expression that groups a nested pattern.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple parenthesized pattern
    ///     if (obj is (string or int))
    ///     // With nested patterns
    ///     if (obj is not (null or ""))
    ///     // In complex pattern combinations
    ///     if (value is &gt; 0 and (int or double))
    ///     // In switch expressions
    ///     return value switch {
    ///         (&gt; 0 and &lt; 10) =&gt; "single digit",
    ///         (string or int) =&gt; "basic type",
    ///         _ =&gt; "other"
    ///     };
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class ParenthesizedPattern(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Pattern> pattern
    ) : Cs.Pattern, Expression<ParenthesizedPattern>, J<ParenthesizedPattern>, MutableTree<ParenthesizedPattern>
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
            return v.VisitParenthesizedPattern(this, p);
        }

        public Guid Id => id;

        public ParenthesizedPattern WithId(Guid newId)
        {
            return newId == id ? this : new ParenthesizedPattern(newId, prefix, markers, _pattern);
        }
        public Space Prefix => prefix;

        public ParenthesizedPattern WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ParenthesizedPattern(id, newPrefix, markers, _pattern);
        }
        public Markers Markers => markers;

        public ParenthesizedPattern WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ParenthesizedPattern(id, prefix, newMarkers, _pattern);
        }
        private readonly JContainer<Cs.Pattern> _pattern = pattern;
        public IList<Cs.Pattern> Pattern => _pattern.GetElements();

        public ParenthesizedPattern WithPattern(IList<Cs.Pattern> newPattern)
        {
            return Padding.WithPattern(JContainer<Cs.Pattern>.WithElements(_pattern, newPattern));
        }
        public sealed record PaddingHelper(Cs.ParenthesizedPattern T)
        {
            public JContainer<Cs.Pattern> Pattern => T._pattern;

            public Cs.ParenthesizedPattern WithPattern(JContainer<Cs.Pattern> newPattern)
            {
                return T._pattern == newPattern ? T : new Cs.ParenthesizedPattern(T.Id, T.Prefix, T.Markers, newPattern);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ParenthesizedPattern && other.Id == Id;
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