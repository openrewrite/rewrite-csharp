//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
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
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class Constructor(
    Guid id,
    Space prefix,
    Markers markers,
    ConstructorInitializer? initializer,
    J.MethodDeclaration constructorCore
    ) : Cs, Statement, MutableTree<Constructor>
    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitConstructor(this, p);
        }

        public Guid Id => id;

        public Constructor WithId(Guid newId)
        {
            return newId == id ? this : new Constructor(newId, prefix, markers, initializer, constructorCore);
        }
        public Space Prefix => prefix;

        public Constructor WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Constructor(id, newPrefix, markers, initializer, constructorCore);
        }
        public Markers Markers => markers;

        public Constructor WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Constructor(id, prefix, newMarkers, initializer, constructorCore);
        }
        public Cs.ConstructorInitializer? Initializer => initializer;

        public Constructor WithInitializer(Cs.ConstructorInitializer? newInitializer)
        {
            return ReferenceEquals(newInitializer, initializer) ? this : new Constructor(id, prefix, markers, newInitializer, constructorCore);
        }
        public J.MethodDeclaration ConstructorCore => constructorCore;

        public Constructor WithConstructorCore(J.MethodDeclaration newConstructorCore)
        {
            return ReferenceEquals(newConstructorCore, constructorCore) ? this : new Constructor(id, prefix, markers, initializer, newConstructorCore);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Constructor && other.Id == Id;
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