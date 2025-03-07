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

        public Guid Id => id;

        public Modifier WithId(Guid newId)
        {
            return newId == id ? this : new Modifier(newId, prefix, markers, keyword, modifierType, annotations);
        }
        public Space Prefix => prefix;

        public Modifier WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new Modifier(id, newPrefix, markers, keyword, modifierType, annotations);
        }
        public Markers Markers => markers;

        public Modifier WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new Modifier(id, prefix, newMarkers, keyword, modifierType, annotations);
        }
        public string? Keyword => keyword;

        public Modifier WithKeyword(string? newKeyword)
        {
            return newKeyword == keyword ? this : new Modifier(id, prefix, markers, newKeyword, modifierType, annotations);
        }
        public Types ModifierType => modifierType;

        public Modifier WithModifierType(Types newModifierType)
        {
            return newModifierType == modifierType ? this : new Modifier(id, prefix, markers, keyword, newModifierType, annotations);
        }
        public IList<J.Annotation> Annotations => annotations;

        public Modifier WithAnnotations(IList<J.Annotation> newAnnotations)
        {
            return newAnnotations == annotations ? this : new Modifier(id, prefix, markers, keyword, modifierType, newAnnotations);
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