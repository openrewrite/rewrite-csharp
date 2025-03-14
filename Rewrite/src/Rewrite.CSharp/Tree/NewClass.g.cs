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
    /// Represents a C# new class instantiation expression, which can optionally include an object/collection initializer.
    /// <br/>
    /// For example:
    /// <code>
    /// // Simple new class without initializer
    /// new Person("John", 25)
    /// // New class with object initializer
    /// new Person { Name = "John", Age = 25 }
    /// // New class with collection initializer
    /// new List<int> { 1, 2, 3 }
    /// // New class with constructor and initializer
    /// new Person("John") { Age = 25 }
    /// </code>
    /// The newClassCore field contains the basic class instantiation including constructor call,
    /// while the initializer field contains the optional object/collection initializer expressions
    /// wrapped in a JContainer to preserve whitespace around curly braces and between initializer expressions.
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class NewClass(
    Guid id,
    Space prefix,
    Markers markers,
    J.NewClass newClassCore,
    InitializerExpression? initializer
    ) : Cs, Statement, Expression, Expression<NewClass>, J<NewClass>, MutableTree<NewClass>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitNewClass(this, p);
        }

        public Guid Id => id;

        public NewClass WithId(Guid newId)
        {
            return newId == id ? this : new NewClass(newId, prefix, markers, newClassCore, initializer);
        }
        public Space Prefix => prefix;

        public NewClass WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new NewClass(id, newPrefix, markers, newClassCore, initializer);
        }
        public Markers Markers => markers;

        public NewClass WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new NewClass(id, prefix, newMarkers, newClassCore, initializer);
        }
        public J.NewClass NewClassCore => newClassCore;

        public NewClass WithNewClassCore(J.NewClass newNewClassCore)
        {
            return ReferenceEquals(newNewClassCore, newClassCore) ? this : new NewClass(id, prefix, markers, newNewClassCore, initializer);
        }
        public Cs.InitializerExpression? Initializer => initializer;

        public NewClass WithInitializer(Cs.InitializerExpression? newInitializer)
        {
            return ReferenceEquals(newInitializer, initializer) ? this : new NewClass(id, prefix, markers, newClassCore, newInitializer);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is NewClass && other.Id == Id;
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