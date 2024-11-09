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
    /// Represents a C# constant pattern that matches against literal values or constant expressions.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Literal constant patterns
    ///     if (obj is null)
    ///     if (number is 42)
    ///     if (flag is true)
    ///     if (ch is 'A')
    ///     if (str is "hello")
    ///     // Constant expressions
    ///     const int MAX = 100;
    ///     if (value is MAX)
    ///     // In switch expressions
    ///     return value switch {
    ///         null =&gt; "undefined",
    ///         0 =&gt; "zero",
    ///         1 =&gt; "one",
    ///         _ =&gt; "other"
    ///     };
    ///     // With other pattern combinations
    ///     if (str is not null and "example")
    ///     // Enum constant patterns
    ///     if (day is DayOfWeek.Sunday)
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class ConstantPattern(
    Guid id,
    Space prefix,
    Markers markers,
    Expression value
    ) : Cs.Pattern, Expression<ConstantPattern>, MutableTree<ConstantPattern>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitConstantPattern(this, p);
        }

        public Guid Id => id;

        public ConstantPattern WithId(Guid newId)
        {
            return newId == id ? this : new ConstantPattern(newId, prefix, markers, value);
        }
        public Space Prefix => prefix;

        public ConstantPattern WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new ConstantPattern(id, newPrefix, markers, value);
        }
        public Markers Markers => markers;

        public ConstantPattern WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new ConstantPattern(id, prefix, newMarkers, value);
        }
        public Expression Value => value;

        public ConstantPattern WithValue(Expression newValue)
        {
            return ReferenceEquals(newValue, value) ? this : new ConstantPattern(id, prefix, markers, newValue);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is ConstantPattern && other.Id == Id;
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