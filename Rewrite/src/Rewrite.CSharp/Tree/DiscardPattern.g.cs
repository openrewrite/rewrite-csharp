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
    /// Represents a C# discard pattern (_), which matches any value and discards it.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Simple discard pattern in is expression
    ///     if (obj is _)
    ///     // In switch expressions
    ///     return value switch {
    ///         1 =&gt; "one",
    ///         2 =&gt; "two",
    ///         _ =&gt; "other"    // Discard pattern as default case
    ///     };
    ///     // With relational patterns
    ///     if (value is &gt; 0 and _)
    ///     // In property patterns
    ///     if (obj is { Id: _, Name: "test" })
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class DiscardPattern(
    Guid id,
    Space prefix,
    Markers markers,
    JavaType type
    ) : Cs.Pattern, Expression<DiscardPattern>, MutableTree<DiscardPattern>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitDiscardPattern(this, p);
        }

        public Guid Id => id;

        public DiscardPattern WithId(Guid newId)
        {
            return newId == id ? this : new DiscardPattern(newId, prefix, markers, type);
        }
        public Space Prefix => prefix;

        public DiscardPattern WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new DiscardPattern(id, newPrefix, markers, type);
        }
        public Markers Markers => markers;

        public DiscardPattern WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new DiscardPattern(id, prefix, newMarkers, type);
        }
        public JavaType Type => type;

        public DiscardPattern WithType(JavaType newType)
        {
            return newType == type ? this : new DiscardPattern(id, prefix, markers, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is DiscardPattern && other.Id == Id;
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