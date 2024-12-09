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
    /// Represents a switch statement section containing one or more case labels followed by a list of statements.
    /// <br/>
    /// For example:
    /// <code>
    /// switch(value) {
    ///     case 1:                    // single case label
    ///     case 2:                    // multiple case labels
    ///         Console.WriteLine("1 or 2");
    ///         break;
    ///     case int n when n &gt; 0:     // pattern case with when clause
    ///         Console.WriteLine("positive");
    ///         break;
    ///     case Person { Age: &gt; 18 }: // recursive pattern
    ///         Console.WriteLine("adult");
    ///         break;
    ///     default:                   // default label
    ///         Console.WriteLine("default");
    ///         break;
    /// }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class SwitchSection(
    Guid id,
    Space prefix,
    Markers markers,
    IList<SwitchLabel> labels,
    IList<JRightPadded<Statement>> statements
    ) : Cs, Statement, J<SwitchSection>, MutableTree<SwitchSection>
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
            return v.VisitSwitchSection(this, p);
        }

        public Guid Id => id;

        public SwitchSection WithId(Guid newId)
        {
            return newId == id ? this : new SwitchSection(newId, prefix, markers, labels, _statements);
        }
        public Space Prefix => prefix;

        public SwitchSection WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new SwitchSection(id, newPrefix, markers, labels, _statements);
        }
        public Markers Markers => markers;

        public SwitchSection WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new SwitchSection(id, prefix, newMarkers, labels, _statements);
        }
        public IList<Cs.SwitchLabel> Labels => labels;

        public SwitchSection WithLabels(IList<Cs.SwitchLabel> newLabels)
        {
            return newLabels == labels ? this : new SwitchSection(id, prefix, markers, newLabels, _statements);
        }
        private readonly IList<JRightPadded<Statement>> _statements = statements;
        public IList<Statement> Statements => _statements.Elements();

        public SwitchSection WithStatements(IList<Statement> newStatements)
        {
            return Padding.WithStatements(_statements.WithElements(newStatements));
        }
        public sealed record PaddingHelper(Cs.SwitchSection T)
        {
            public IList<JRightPadded<Statement>> Statements => T._statements;

            public Cs.SwitchSection WithStatements(IList<JRightPadded<Statement>> newStatements)
            {
                return T._statements == newStatements ? T : new Cs.SwitchSection(T.Id, T.Prefix, T.Markers, T.Labels, newStatements);
            }

        }

        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is SwitchSection && other.Id == Id;
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