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
    /// Represents a C# checked statement which enforces overflow checking for arithmetic operations
    /// and conversions. Operations within a checked block will throw OverflowException if arithmetic
    /// overflow occurs.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Basic checked block
    ///     checked {
    ///         int result = int.MaxValue + 1; // throws OverflowException
    ///     }
    ///     // Checked with multiple operations
    ///     checked {
    ///         int a = int.MaxValue;
    ///         int b = a + 1;     // throws OverflowException
    ///         short s = (short)a; // throws OverflowException if out of range
    ///     }
    ///     // Nested arithmetic operations
    ///     checked {
    ///         int result = Math.Abs(int.MinValue); // throws OverflowException
    ///     }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class CheckedStatement(
    Guid id,
    Space prefix,
    Markers markers,
    Keyword keyword,
    J.Block block
    ) : Cs, Statement, J<CheckedStatement>, MutableTree<CheckedStatement>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitCheckedStatement(this, p);
        }

        public Guid Id => id;

        public CheckedStatement WithId(Guid newId)
        {
            return newId == id ? this : new CheckedStatement(newId, prefix, markers, keyword, block);
        }
        public Space Prefix => prefix;

        public CheckedStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new CheckedStatement(id, newPrefix, markers, keyword, block);
        }
        public Markers Markers => markers;

        public CheckedStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CheckedStatement(id, prefix, newMarkers, keyword, block);
        }
        public Cs.Keyword Keyword => keyword;

        public CheckedStatement WithKeyword(Cs.Keyword newKeyword)
        {
            return ReferenceEquals(newKeyword, keyword) ? this : new CheckedStatement(id, prefix, markers, newKeyword, block);
        }
        public J.Block Block => block;

        public CheckedStatement WithBlock(J.Block newBlock)
        {
            return ReferenceEquals(newBlock, block) ? this : new CheckedStatement(id, prefix, markers, keyword, newBlock);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CheckedStatement && other.Id == Id;
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