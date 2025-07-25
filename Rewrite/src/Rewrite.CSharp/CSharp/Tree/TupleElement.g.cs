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
    /// Represents a single element within a tuple type, which may include an optional
    /// identifier for named tuple elements.
    /// </summary>
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class TupleElement(
    Guid id,
    Space prefix,
    Markers markers,
    TypeTree type,
    J.Identifier? name
    ) : Cs    {
        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitTupleElement(this, p);
        }

        public Guid Id { get;  set; } = id;

        public TupleElement WithId(Guid newId)
        {
            return newId == Id ? this : new TupleElement(newId, Prefix, Markers, Type, Name);
        }
        public Space Prefix { get;  set; } = prefix;

        public TupleElement WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new TupleElement(Id, newPrefix, Markers, Type, Name);
        }
        public Markers Markers { get;  set; } = markers;

        public TupleElement WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new TupleElement(Id, Prefix, newMarkers, Type, Name);
        }
        public TypeTree Type { get;  set; } = type;

        public TupleElement WithType(TypeTree newType)
        {
            return ReferenceEquals(newType, Type) ? this : new TupleElement(Id, Prefix, Markers, newType, Name);
        }
        public J.Identifier? Name { get;  set; } = name;

        public TupleElement WithName(J.Identifier? newName)
        {
            return ReferenceEquals(newName, Name) ? this : new TupleElement(Id, Prefix, Markers, Type, newName);
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is TupleElement && other.Id == Id;
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