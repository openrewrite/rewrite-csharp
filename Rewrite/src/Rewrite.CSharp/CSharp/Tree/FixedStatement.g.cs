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
    /// Represents a C# fixed statement which pins a moveable variable at a memory location.
    /// The fixed statement prevents the garbage collector from relocating a movable variable
    /// and declares a pointer to that variable.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Fixed statement with array
    ///     fixed (int* p = array) {
    ///         // use p
    ///     }
    ///     // Fixed statement with string
    ///     fixed (char* p = str) {
    ///         // use p
    ///     }
    ///     // Multiple pointers in one fixed statement
    ///     fixed (byte* p1 = &b1, p2 = &b2) {
    ///         // use p1 and p2
    ///     }
    ///     // Fixed statement with custom type
    ///     fixed (CustomStruct* ptr = &struct) {
    ///         // use ptr
    ///     }
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class FixedStatement(
    Guid id,
    Space prefix,
    Markers markers,
    J.ControlParentheses<J.VariableDeclarations> declarations,
    J.Block block
    ) : Cs,Statement    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitFixedStatement(this, p);
        }

        public Guid Id { get;  set; } = id;

        public FixedStatement WithId(Guid newId)
        {
            return newId == Id ? this : new FixedStatement(newId, Prefix, Markers, Declarations, Block);
        }
        public Space Prefix { get;  set; } = prefix;

        public FixedStatement WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new FixedStatement(Id, newPrefix, Markers, Declarations, Block);
        }
        public Markers Markers { get;  set; } = markers;

        public FixedStatement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new FixedStatement(Id, Prefix, newMarkers, Declarations, Block);
        }
        public J.ControlParentheses<J.VariableDeclarations> Declarations { get;  set; } = declarations;

        public FixedStatement WithDeclarations(J.ControlParentheses<J.VariableDeclarations> newDeclarations)
        {
            return ReferenceEquals(newDeclarations, Declarations) ? this : new FixedStatement(Id, Prefix, Markers, newDeclarations, Block);
        }
        public J.Block Block { get;  set; } = block;

        public FixedStatement WithBlock(J.Block newBlock)
        {
            return ReferenceEquals(newBlock, Block) ? this : new FixedStatement(Id, Prefix, Markers, Declarations, newBlock);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is FixedStatement && other.Id == Id;
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