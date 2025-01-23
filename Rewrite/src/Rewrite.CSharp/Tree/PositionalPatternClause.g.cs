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
    /// Represents a positional pattern clause in C# pattern matching, which matches the deconstructed parts of an object.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple positional pattern
    ///     if (point is (0, 0))
    ///     // With variable declarations
    ///     if (point is (int x, int y))
    ///     // With nested patterns
    ///     if (point is (&gt; 0, &lt; 100))
    ///     // In switch expressions
    ///     return point switch {
    ///         (0, 0) =&gt; "origin",
    ///         (var x, var y) when x == y =&gt; "on diagonal",
    ///         _ =&gt; "other"
    ///     };
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class PositionalPatternClause(
    Guid id,
    Space prefix,
    Markers markers,
    JContainer<Subpattern> subpatterns
    ) : Cs, J<PositionalPatternClause>, MutableTree<PositionalPatternClause>
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
            return v.VisitPositionalPatternClause(this, p);
        }

        public Guid Id => id;

        public PositionalPatternClause WithId(Guid newId)
        {
            return newId == id ? this : new PositionalPatternClause(newId, prefix, markers, _subpatterns);
        }
        public Space Prefix => prefix;

        public PositionalPatternClause WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new PositionalPatternClause(id, newPrefix, markers, _subpatterns);
        }
        public Markers Markers => markers;

        public PositionalPatternClause WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new PositionalPatternClause(id, prefix, newMarkers, _subpatterns);
        }
        private readonly JContainer<Cs.Subpattern> _subpatterns = subpatterns;
        public IList<Cs.Subpattern> Subpatterns => _subpatterns.GetElements();

        public PositionalPatternClause WithSubpatterns(IList<Cs.Subpattern> newSubpatterns)
        {
            return Padding.WithSubpatterns(JContainer<Cs.Subpattern>.WithElements(_subpatterns, newSubpatterns));
        }
        public sealed record PaddingHelper(Cs.PositionalPatternClause T)
        {
            public JContainer<Cs.Subpattern> Subpatterns => T._subpatterns;

            public Cs.PositionalPatternClause WithSubpatterns(JContainer<Cs.Subpattern> newSubpatterns)
            {
                return T._subpatterns == newSubpatterns ? T : new Cs.PositionalPatternClause(T.Id, T.Prefix, T.Markers, newSubpatterns);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is PositionalPatternClause && other.Id == Id;
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