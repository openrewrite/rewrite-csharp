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
    /// Represents a subpattern in C# pattern matching, which can appear in property patterns or positional patterns.
    /// Each subpattern consists of an optional name with a corresponding pattern.
    /// <br/>
    /// For example:
    /// <code>
    ///     // In property patterns
    ///     if (obj is { Name: "test", Age: &gt; 18 })
    ///                  ^^^^^^^^^^^^  ^^^^^^^^^
    ///     // In positional patterns
    ///     if (point is (x: &gt; 0, y: &gt; 0))
    ///                   ^^^^^^  ^^^^^^
    ///     // With variable declarations
    ///     if (person is { Id: var id, Name: string name })
    ///                     ^^^^^^^^^^  ^^^^^^^^^^^^^^^^^
    ///     // Nested patterns
    ///     if (obj is { Address: { City: "NY" } })
    ///                  ^^^^^^^^^^^^^^^^^^^^^^^
    ///     // In switch expressions
    ///     return shape switch {
    ///         { Radius: var r } =&gt; Math.PI * r * r,
    ///           ^^^^^^^^^^^
    ///         { Width: var w, Height: var h } =&gt; w * h,
    ///           ^^^^^^^^^^^^  ^^^^^^^^^^^^^
    ///         _ =&gt; 0
    ///     };
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class Subpattern(
    Guid id,
    Space prefix,
    Markers markers,
    Expression? name,
    JLeftPadded<Pattern> pattern
    ) : Cs,Expression    {
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
            return v.VisitSubpattern(this, p);
        }

        public Guid Id { get;  set; } = id;

        public Subpattern WithId(Guid newId)
        {
            return newId == Id ? this : new Subpattern(newId, Prefix, Markers, Name, _pattern);
        }
        public Space Prefix { get;  set; } = prefix;

        public Subpattern WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new Subpattern(Id, newPrefix, Markers, Name, _pattern);
        }
        public Markers Markers { get;  set; } = markers;

        public Subpattern WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new Subpattern(Id, Prefix, newMarkers, Name, _pattern);
        }
        public Expression? Name { get;  set; } = name;

        public Subpattern WithName(Expression? newName)
        {
            return ReferenceEquals(newName, Name) ? this : new Subpattern(Id, Prefix, Markers, newName, _pattern);
        }
        private JLeftPadded<Cs.Pattern> _pattern = pattern;
        public Cs.Pattern Pattern => _pattern.Element;

        public Subpattern WithPattern(Cs.Pattern newPattern)
        {
            return Padding.WithPattern(_pattern.WithElement(newPattern));
        }
        public sealed record PaddingHelper(Cs.Subpattern T)
        {
            public JLeftPadded<Cs.Pattern> Pattern { get => T._pattern;  set => T._pattern = value; }

            public Cs.Subpattern WithPattern(JLeftPadded<Cs.Pattern> newPattern)
            {
                return Pattern == newPattern ? T : new Cs.Subpattern(T.Id, T.Prefix, T.Markers, T.Name, newPattern);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Subpattern && other.Id == Id;
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