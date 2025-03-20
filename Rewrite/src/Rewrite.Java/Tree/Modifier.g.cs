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

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface J : Rewrite.Core.Tree
{
    #if DEBUG_VISITOR
    [DebuggerStepThrough]
    #endif
    public partial class Modifier(
    Guid id,
    Space prefix,
    Markers markers,
    string? keyword,
    Modifier.Types modifierType,
    IList<Annotation> annotations
    ) : J, J<Modifier>, MutableTree<Modifier>
    {
        public J? AcceptJava<P>(JavaVisitor<P> v, P p)
        {
            return v.VisitModifier(this, p);
        }

        public Guid Id { get;  set; } = id;

        public Modifier WithId(Guid newId)
        {
            return newId == Id ? this : new Modifier(newId, Prefix, Markers, Keyword, ModifierType, Annotations);
        }
        public Space Prefix { get;  set; } = prefix;

        public Modifier WithPrefix(Space newPrefix)
        {
            return newPrefix == Prefix ? this : new Modifier(Id, newPrefix, Markers, Keyword, ModifierType, Annotations);
        }
        public Markers Markers { get;  set; } = markers;

        public Modifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, Markers) ? this : new Modifier(Id, Prefix, newMarkers, Keyword, ModifierType, Annotations);
        }
        public string? Keyword { get;  set; } = keyword;

        public Modifier WithKeyword(string? newKeyword)
        {
            return newKeyword == Keyword ? this : new Modifier(Id, Prefix, Markers, newKeyword, ModifierType, Annotations);
        }
        public Types ModifierType { get;  set; } = modifierType;

        public Modifier WithModifierType(Types newModifierType)
        {
            return newModifierType == ModifierType ? this : new Modifier(Id, Prefix, Markers, Keyword, newModifierType, Annotations);
        }
        public IList<J.Annotation> Annotations { get;  set; } = annotations;

        public Modifier WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == Annotations ? this : new Modifier(Id, Prefix, Markers, Keyword, ModifierType, newAnnotations);
        }
        public enum Types
        {
            Default,
            Public,
            Protected,
            Private,
            Abstract,
            Static,
            Final,
            Sealed,
            NonSealed,
            Transient,
            Volatile,
            Synchronized,
            Native,
            Strictfp,
            Async,
            Reified,
            Inline,
            LanguageExtension,
        }
        #if DEBUG_VISITOR
        [DebuggerStepThrough]
        #endif
        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is Modifier && other.Id == Id;
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