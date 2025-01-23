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
    /// Represents a C# ref type, which indicates that a type is passed or returned by reference.
    /// Used in method parameters, return types, and local variable declarations.
    /// <br/>
    /// For example:
    /// <code>
    ///     // Method parameter
    ///     void Process(ref int value)
    ///     // Method return type
    ///     ref int GetValue()
    ///     // Local variable
    ///     ref int number = ref GetValue();
    ///     // Property
    ///     ref readonly int Property =&gt; ref field;
    /// </code>
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class RefType(
    Guid id,
    Space prefix,
    Markers markers,
    J.Modifier? readonlyKeyword,
    TypeTree typeIdentifier,
    JavaType? type
    ) : Cs, TypeTree, Expression, Expression<RefType>, TypedTree<RefType>, J<RefType>, TypeTree<RefType>, MutableTree<RefType>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitRefType(this, p);
        }

        public Guid Id => id;

        public RefType WithId(Guid newId)
        {
            return newId == id ? this : new RefType(newId, prefix, markers, readonlyKeyword, typeIdentifier, type);
        }
        public Space Prefix => prefix;

        public RefType WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new RefType(id, newPrefix, markers, readonlyKeyword, typeIdentifier, type);
        }
        public Markers Markers => markers;

        public RefType WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new RefType(id, prefix, newMarkers, readonlyKeyword, typeIdentifier, type);
        }
        public J.Modifier? ReadonlyKeyword => readonlyKeyword;

        public RefType WithReadonlyKeyword(J.Modifier? newReadonlyKeyword)
        {
            return ReferenceEquals(newReadonlyKeyword, readonlyKeyword) ? this : new RefType(id, prefix, markers, newReadonlyKeyword, typeIdentifier, type);
        }
        public TypeTree TypeIdentifier => typeIdentifier;

        public RefType WithTypeIdentifier(TypeTree newTypeIdentifier)
        {
            return ReferenceEquals(newTypeIdentifier, typeIdentifier) ? this : new RefType(id, prefix, markers, readonlyKeyword, newTypeIdentifier, type);
        }
        public JavaType? Type => type;

        public RefType WithType(JavaType? newType)
        {
            return newType == type ? this : new RefType(id, prefix, markers, readonlyKeyword, typeIdentifier, newType);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is RefType && other.Id == Id;
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