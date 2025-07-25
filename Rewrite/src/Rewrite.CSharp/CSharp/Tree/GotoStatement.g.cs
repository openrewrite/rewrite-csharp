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
    /// Represents a C# goto statement, which performs an unconditional jump to a labeled statement,
    /// case label, or default label within a switch statement.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple goto statement
    ///     goto Label;
    ///     // Goto case in switch statement
    ///     goto case 1;
    ///     // Goto default in switch statement
    ///     goto default;
    ///     // With label declaration
    ///     Label:
    ///     Console.WriteLine("At label");
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class GotoStatement(
    Guid id,
    Space prefix,
    Markers markers,
    Keyword? caseOrDefaultKeyword,
    Expression? target
    ) : Cs,Statement    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitGotoStatement(this, p);
        }

        public Guid Id { get;  set; } = id;

        public GotoStatement WithId(Guid newId)
        {
            return newId == Id ? this : new GotoStatement(newId, Prefix, Markers, CaseOrDefaultKeyword, Target);
        }
        public Space Prefix { get;  set; } = prefix;

        public GotoStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new GotoStatement(Id, newPrefix, Markers, CaseOrDefaultKeyword, Target);
        }
        public Markers Markers { get;  set; } = markers;

        public GotoStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new GotoStatement(Id, Prefix, newMarkers, CaseOrDefaultKeyword, Target);
        }
        public Cs.Keyword? CaseOrDefaultKeyword { get;  set; } = caseOrDefaultKeyword;

        public GotoStatement WithCaseOrDefaultKeyword(Cs.Keyword? newCaseOrDefaultKeyword)
        {
            return ReferenceEquals(newCaseOrDefaultKeyword, CaseOrDefaultKeyword) ? this : new GotoStatement(Id, Prefix, Markers, newCaseOrDefaultKeyword, Target);
        }
        public Expression? Target { get;  set; } = target;

        public GotoStatement WithTarget(Expression? newTarget)
        {
            return ReferenceEquals(newTarget, Target) ? this : new GotoStatement(Id, Prefix, Markers, CaseOrDefaultKeyword, newTarget);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is GotoStatement && other.Id == Id;
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